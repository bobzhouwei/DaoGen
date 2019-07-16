using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DalClassGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtSql.Text.Equals(String.Empty))
            {
                return;
            }


            string sqlString = txtSql.Text.Replace("\\n", "");
            string[] separator = new string[] { "\n" };
            string[] lines = sqlString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (lines == null || lines.Length <= 0)
            {
                return;
            }

            string tableName = "";
            string primaryKey = "";
            Dictionary<string, ColumnInfo> colMap = new Dictionary<string, ColumnInfo>();
            foreach (string line in lines)
            {
                string l = line.TrimStart().TrimEnd();
                if (l.Contains("CREATE TABLE"))
                {
                    tableName = getTableName(line);
                    continue;
                }
                else if (l.Contains("PRIMARY KEY"))
                {
                    primaryKey = getPrimaryKey(line);
                    continue;
                }
                else if (l.StartsWith("`"))
                {
                    ColumnInfo columnInfo = GetColumnInfo(l);
                    if (columnInfo != null)
                    {
                        colMap.Add(columnInfo.ColName, columnInfo);
                    }
                    continue;
                }
            }
            if (colMap.Count > 0)
            {
                WriteResult(tableName, colMap, primaryKey);
            }
        }

        private List<string> GetHeaderCode(string tableName)
        {
            List<string> codes = new List<string>();
            codes.Add("import com.ctrip.platform.dal.dao.DalPojo;");
            codes.Add("import com.ctrip.platform.dal.dao.annotation.Database;");
            codes.Add("import com.ctrip.platform.dal.dao.annotation.Type;");
            codes.Add("import lombok.Getter;");
            codes.Add("import lombok.Setter;");
            codes.Add("");
            codes.Add("import javax.persistence.*;");
            codes.Add("import java.math.BigDecimal;");
            codes.Add("import java.sql.Timestamp;");
            codes.Add("import java.sql.Types;");
            codes.Add("");
            codes.Add("@Getter");
            codes.Add("@Setter");
            codes.Add("@Entity");
            codes.Add("@Database(name = \"" + dbTxt.Text + "\")");
            codes.Add("@Table(name = \"" + tableName + "\")");
            return codes;
        }

        private string getTableName(string line)
        {
            if (line.Equals(String.Empty))
            {
                return "";
            }
            string[] separator = new string[] { "`" };
            string[] cols = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (cols == null || cols.Length < 2)
            {
                return "";
            }
            return cols[1];
        }

        private string getPrimaryKey(string line)
        {
            if (line.Equals(String.Empty))
            {
                return "";
            }
            string[] separator = new string[] { "`" };
            string[] cols = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (cols == null || cols.Length < 2)
            {
                return "";
            }
            return cols[1];
        }

        private ColumnInfo GetColumnInfo(string line)
        {
            if (line.Equals(String.Empty))
            {
                return null;
            }

            ColumnInfo columnInfo = new ColumnInfo();

            string[] separator = new string[] { "`" };
            string[] cols = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (cols == null || cols.Length < 2)
            {
                return null;
            }

            // 获取字段名
            columnInfo.ColName = cols[0];

            string l2 = cols[1].TrimStart().TrimEnd();
            separator = new string[] { " " };
            string[] c2 = l2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (c2 == null || c2.Length < 1)
            {
                return null;
            }
            // 获取字段类型
            columnInfo.ColType = c2[0];

            // 是否自增
            if (l2.Contains("AUTO_INCREMENT"))
            {
                columnInfo.AutoIncrement = true;
            }

            // 获取备注
            int cmtPos = l2.IndexOf("COMMENT");
            if (cmtPos >= 0)
            {
                string cmt = l2.Substring(cmtPos + "COMMENT".Length).Trim();
                columnInfo.Comment = FormatComment(cmt);
            }

            return columnInfo;
        }

        private string FormatComment(string comment)
        {
            if (comment == null || comment.Equals(String.Empty))
            {
                return "";
            }

            if (comment.StartsWith("'"))
            {
                comment = comment.Substring(1, comment.Length - 1);
            }
            if (comment.EndsWith("'"))
            {
                comment = comment.Substring(0, comment.Length - 1);
            }
            if (comment.EndsWith("',"))
            {
                comment = comment.Substring(0, comment.Length - 2);
            }
            return comment;
        }

        private void WriteResult(string tableName, Dictionary<string, ColumnInfo> colMap, string primaryKey)
        {
            StringBuilder sb = new StringBuilder();

            List<string> headCodes = GetHeaderCode(tableName);
            foreach (string line in headCodes)
            {
                sb.Append(line + Environment.NewLine);
            }

            sb.Append("" + Environment.NewLine);

            string className = ConvertTableName(tableName);
            sb.Append("public class " + className + " implements DalPojo {" + Environment.NewLine);

            sb.Append("" + Environment.NewLine);

            foreach (KeyValuePair<string, ColumnInfo> keyValuePair in colMap)
            {
                string colName = keyValuePair.Key;
                string javaColName = ConvertColName(colName);

                if (!keyValuePair.Value.Comment.Equals(String.Empty))
                {
                    sb.Append("/**" + Environment.NewLine);
                    sb.Append("* " + keyValuePair.Value.Comment + Environment.NewLine);
                    sb.Append("*/" + Environment.NewLine);
                }

                if (colName.Equals(primaryKey))
                {
                    sb.Append("@Id" + Environment.NewLine);
                }
                sb.Append("@Column(name = \"" + colName + "\")" + Environment.NewLine);
                if (keyValuePair.Value.AutoIncrement)
                {
                    sb.Append("@GeneratedValue(strategy = GenerationType.AUTO)" + Environment.NewLine);
                }

                TypeInfo typeInfo = GetTypeInfo(keyValuePair.Value.ColType);
                if (typeInfo == null)
                {
                    continue;
                }

                sb.Append("@Type(value = Types." + typeInfo.SqlTypeName + ")" + Environment.NewLine);
                sb.Append("private " + typeInfo.JavaTypeName + " " + javaColName + ";" + Environment.NewLine);

                sb.Append("" + Environment.NewLine);
            }

            sb.Append("}" + Environment.NewLine);

            resTxt.Text = sb.ToString();
        }

        private string ConvertColName(string colName)
        {
            if (colName == null || colName.Equals(String.Empty))
            {
                return "";
            }

            string ret = "";
            if (colName.Contains("_"))
            {
                StringBuilder sb = new StringBuilder();
                string[] separator = new string[] { "_" };
                string[] names = colName.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i].Equals(String.Empty) || names[i].TrimStart().TrimEnd().Equals(String.Empty))
                    {
                        continue;
                    }

                    if (i > 0)
                    {
                        sb.Append(UpperFirstLetter(names[i]));
                    }
                    else
                    {
                        sb.Append(names[i]);
                    }
                }
                ret = sb.ToString();
            }
            else
            {
                ret = colName;
            }

            return ret;
        }

        private string ConvertTableName(string t_name)
        {
            if (t_name == null || t_name.Equals(String.Empty))
            {
                return "";
            }

            string ret = "";
            if (t_name.Contains("_"))
            {
                StringBuilder sb = new StringBuilder();
                string[] separator = new string[] { "_" };
                string[] names = t_name.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i].Equals(String.Empty) || names[i].TrimStart().TrimEnd().Equals(String.Empty))
                    {
                        continue;
                    }

                    if (i > 0)
                    {
                        sb.Append(UpperFirstLetter(names[i]));
                    }
                    else
                    {
                        sb.Append(names[i]);
                    }
                }
                ret = sb.ToString();
            }
            else
            {
                ret = t_name;
            }

            return UpperFirstLetter(ret);

        }

        private string UpperFirstLetter(string s)
        {
            if (s == null || s.Equals(String.Empty))
            {
                return "";
            }
            string ret = s.Substring(0, 1).ToUpper();
            if (s.Length > 1)
            {
                ret += s.Substring(1, s.Length - 1);
            }
            return ret;
        }

        private TypeInfo GetTypeInfo(string typeStr)
        {
            if (typeStr == null || typeStr.Equals(String.Empty))
            {
                return null;
            }

            TypeInfo typeInfo = new TypeInfo();
            if (typeStr.StartsWith("int"))
            {
                typeInfo.SqlTypeName = "INTEGER";
                typeInfo.JavaTypeName = "Integer";
                return typeInfo;
            }
            else if (typeStr.StartsWith("tinyint"))
            {
                typeInfo.SqlTypeName = "TINYINT";
                typeInfo.JavaTypeName = "Integer";
                return typeInfo;
            }
            else if (typeStr.StartsWith("varchar"))
            {
                typeInfo.SqlTypeName = "VARCHAR";
                typeInfo.JavaTypeName = "String";
                return typeInfo;
            }
            else if (typeStr.StartsWith("text"))
            {
                typeInfo.SqlTypeName = "LONGVARCHAR";
                typeInfo.JavaTypeName = "String";
                return typeInfo;
            }
            else if (typeStr.StartsWith("float"))
            {
                typeInfo.SqlTypeName = "REAL";
                typeInfo.JavaTypeName = "Float";
                return typeInfo;
            }
            else if (typeStr.StartsWith("decimal"))
            {
                typeInfo.SqlTypeName = "DECIMAL";
                typeInfo.JavaTypeName = "BigDecimal";
                return typeInfo;
            }
            else if (typeStr.StartsWith("datetime"))
            {
                typeInfo.SqlTypeName = "TIMESTAMP";
                typeInfo.JavaTypeName = "Timestamp";
                return typeInfo;
            }
            else
            {
                return null;
            }
        }
    }
}
