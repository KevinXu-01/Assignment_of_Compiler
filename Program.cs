using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_of_Compiler
{
    struct _symbol
    {
        public int _class;
        public int _value;
    };

    struct _constant
    {
        public int _id;
        public double _value;
    };

    struct _string
    {
        public int _id;
        public string _value;
    };
    class Program
    {

        private List<_symbol> symbol_list; //符号表
        private List<_constant> constant_list; //常量表
        private List<_string> string_list; //字符（串）表
        static void init()
        {

        }
        static void Main(string[] args)
        {

        }

    }
}
