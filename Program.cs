using System;
using System.Collections.Generic;
using System.IO;


namespace Assignment_of_Compiler
{
    struct _keyword_and_symbol
    {
        public int _id;
        public string _value;
    };
    class Program
    {
        private static List<_keyword_and_symbol> keyword_and_symbol_list = new List<_keyword_and_symbol>(); //符号表和关键词表（一符一码）
        private static List<char> letter = new List<char>();
        private static int length;
        private static int num;
        private static List<_keyword_and_symbol> result = new List<_keyword_and_symbol>(); //词法分析结果

        static void init()
        {
            StreamReader file = new StreamReader("keyword_and_symbol.txt");
            string line;
            while((line = file.ReadLine()) != null) //读入符号表
            {
                _keyword_and_symbol temp;
                temp._id = Convert.ToInt32(line.Split('\t')[0]) + 1;
                temp._value = line.Split('\t')[1];
                keyword_and_symbol_list.Add(temp);
            }
            file.Close();
        }
        static void LoadCode()
        {
            length = 0;
            StreamReader file = new StreamReader("code.txt");
            string code = file.ReadToEnd();
            code = code.Replace("\n", "");
            code = code.Replace("\t", "");
            code = code.Replace("\r", "");
            file.Close();
            for(int i = 0; i < code.Length; i++)
            {
                if(code[i] != ' ')
                {
                    letter.Add(code[i]);
                    length++;
                }
            }
        }
        static int isKeywordOrSymbol(string s)   //判断关键字、运算符和界限符
        {
            for (int i = 0; i <= 93; i++)
            {
                if (s == keyword_and_symbol_list[i]._value)
                    return keyword_and_symbol_list[i]._id;
            }
            return -1;
        }
        static bool isNum(char s) //判断是否是数字
        {
            if (s >= '0' && s <= '9')
                return true;
            return false;
        }
        static bool isLetter(char s) //判断是否是字母
        {
            if (s >= 'a' && s <= 'z' || s >= 'A' && s <= 'Z')
                return true;
            return false;
        }

        static int typeword(char s) //返回单个字符的类型
        {
            if (s >= 'a' && s <= 'z' || s >= 'A' && s <= 'Z')
                return 1; //字母
            if (s >= '0' && s <= '9')
                return 2; //数字
            if (s == '#' || s == '{' || s == '}' || s == '(' || s == ')' || s == ';' || s == ',' || s == '\'' || s == '+' || s == '-' || s == '*' || s == '/' ||
                s == '=' || s == '<' || s == '>' || s == '!' || s == '^' || s == '&' || s == '|') //运算符和界符 
                return 3;
            return -1;
        }

        static string identifier(string s, int n) //判断是否是标识符
        {
            int j = n + 1;
            bool flag = true;
            while(flag)
            {
                if(isNum(letter[j]) || isLetter(letter[j]))
                {
                    s += letter[j];
                    if(isKeywordOrSymbol(s) != -1)
                    {
                        j++;
                        num = j;
                        return s;
                    }
                    j++;
                }
                else
                {
                    flag = false;
                }
            }
            num = j;
            return s;
        }
        
        static string symbolStr(string s, int n)
        {
            int j = n + 1;
            if (j >= length)
            {
                num = j;
                return s;
            }
            char str = letter[j];
            if (str == '>' || str == '=' || str == '<' || str == '!')
            {
                s += str;
                j++;
            }
            num = j;
            return s;
        }

        static string Number(string s, int n)
        {
            int j = n + 1;
            bool flag = true;
            while(flag)
            {
                if(isNum(letter[j]))
                {
                    s += letter[j];
                    j++;
                }
                else
                {
                    flag = false;
                }
            }
            num = j;
            return s;
        }

        static void TakeWord()
        {
            int k;
            for (num = 0; num < length;)
            {
                string str1;
                char str;
                str = letter[num];
                k = typeword(str);
                switch (k)
                {
                    case 1: //字母
                        {
                            str1 = identifier(Convert.ToString(str), num);
                            if (isKeywordOrSymbol(str1) != -1)
                            {
                                _keyword_and_symbol temp;
                                temp._value = str1;
                                temp._id = isKeywordOrSymbol(str1); //关键词
                                result.Add(temp);
                            }
                            else
                            {
                                _keyword_and_symbol temp;
                                temp._value = str1;
                                temp._id = 0; //标识符
                                result.Add(temp);
                            }
                            break;
                        }
                    case 2: //数字
                        {
                            str1 = Number(Convert.ToString(str), num);
                            _keyword_and_symbol temp;
                            temp._value = str1;
                            temp._id = keyword_and_symbol_list.Count + 1;
                            result.Add(temp);
                            break;
                        }
                    case 3:
                        {
                            str1 = symbolStr(Convert.ToString(str), num);
                            _keyword_and_symbol temp;
                            temp._value = str1;
                            temp._id = isKeywordOrSymbol(str1); //边界符/运算符
                            result.Add(temp);
                            break;
                        }
                    default: break;
                }
            }
         }
        static void Main(string[] args)
        {
            init();
            LoadCode();
            TakeWord();
            StreamWriter file = new StreamWriter("result.txt");
            for(int i = 0; i < result.Count; i++)
            {
                file.WriteLine(result[i]._value + "\t\t" + result[i]._id);
            }
            file.Flush();
            file.Close();
            Console.WriteLine("完成！");
        }

    }
}
