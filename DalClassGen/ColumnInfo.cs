using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalClassGen
{
    public class ColumnInfo
    {
        private string colName = "";
        private string colType = "";
        private string comment = "";
        private bool autoIncrement = false;

        public string ColName { get => colName; set => colName = value; }
        public string ColType { get => colType; set => colType = value; }
        public string Comment { get => comment; set => comment = value; }
        public bool AutoIncrement { get => autoIncrement; set => autoIncrement = value; }
    }
}
