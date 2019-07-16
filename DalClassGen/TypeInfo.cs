using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalClassGen
{
    public class TypeInfo
    {
        private string sqlTypeName;
        private string javaTypeName;

        public string SqlTypeName { get => sqlTypeName; set => sqlTypeName = value; }
        public string JavaTypeName { get => javaTypeName; set => javaTypeName = value; }
    }
}
