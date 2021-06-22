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
    struct RESULT
    {
        public string _content;
        public int _class;
        public string _value;
    }
    class Program
    {
        private static List<_keyword_and_symbol> keyword_list = new List<_keyword_and_symbol>(); //关键词表（一符一码）
        private static List<_keyword_and_symbol> symbol_list = new List<_keyword_and_symbol>(); //符号表（一符一码）
        private static List<_keyword_and_symbol> identifier_list = new List<_keyword_and_symbol>(); //标识符表（一符一码）
        private static string code = "";
        private static int cur_pos = 0;
        private static bool isPassed = true;
        private static string error = "";
        private static List<RESULT> result = new List<RESULT>(); //词法分析结果

        static void init() //初始化符号表和关键词表
        {
            StreamReader keyword_file = new StreamReader("keyword.txt");
            string line;
            while((line = keyword_file.ReadLine()) != null) //读入关键词表
            {
                _keyword_and_symbol temp;
                temp._id = Convert.ToInt32(line.Split('\t')[0]);
                temp._value = line.Split('\t')[1];
                keyword_list.Add(temp);
            }
            keyword_file.Close();


            StreamReader symbol_file = new StreamReader("symbol.txt");
            while ((line = symbol_file.ReadLine()) != null) //读入符号表
            {
                _keyword_and_symbol temp;
                temp._id = Convert.ToInt32(line.Split('\t')[0]) + 1;
                temp._value = line.Split('\t')[1];
                symbol_list.Add(temp);
            }
            symbol_file.Close();
        }
        static void LoadCode() //加载代码
        {
            StreamReader file = new StreamReader("code.txt");
            string line = "";
            while((line = file.ReadLine()) != null)
            {
                if (line.Contains("@"))
                {
                    line = line.Split('@')[0]; //清除注释
                }
                code += line + "\n\r";
            }
            code = code.Replace("\n", " ");
            code = code.Replace("\t", " ");
            code = code.Replace("\r", " ");
            file.Close();
        }
        static int isKeywordOrSymbol(string s)   //判断关键字、运算符和界限符
        {
            for (int i = 2; i <= 25; i++)
            {
                if ( i <= 21 )
                {
                    if (s == symbol_list[i]._value)
                        return symbol_list[i]._id;
                }
                else
                {
                    if (s == keyword_list[i - 22]._value)
                        return keyword_list[i - 22]._id;
                }
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
            if (s >= 'a' && s <= 'z')
                return true;
            return false;
        }

        static int typeword(char s) //返回单个字符的类型
        {
            if (s >= 'a' && s <= 'z')
                return 1; //字母
            if (s >= '0' && s <= '9')
                return 2; //数字
            if (s == '#' || s == '{' || s == '}' || s == '(' || s == ')' || s == ';' || s == ':'  || s == '+' || s == '-' || s == '*' || s == '/' ||
                s == '=' || s == '<' || s == '>' || s == '!' || s == '&' || s == '|') //运算符和界符 
                return 3;
            if (s == ' ')
                return 4;
            return -1;
        }

        static string identifier(string s) //判断是否是标识符
        {
            int j = cur_pos + 1;
            bool flag = true;
            while(flag)
            {
                if(isNum(code[j]) || isLetter(code[j]))
                {
                    s += code[j];
                    if(isKeywordOrSymbol(s) != -1)
                    {
                        j++;
                        cur_pos = j;
                        return s;
                    }
                    j++;
                }
                else
                {
                    flag = false;
                }
            }
            cur_pos = j;
            return s;
        }
        
        static string symbolStr(string s)
        {
            if (s == ":" || s == "<" || s == ">" || s == "=" || s == "!")
            {
                int j = cur_pos + 1;
                if (j >= code.Length)
                {
                    cur_pos = j;
                    return s;
                }
                char str = code[j];
                if (str == '=')
                {
                    s += str;
                    j++;
                    cur_pos = j;
                }
                else if (isNum(str) || isLetter(str) || str == ' ')
                {
                    cur_pos++;
                }
                else
                {
                    string temp = s;
                    s = "undefined symbol: " + temp + str;
                }
            }
            else  if (s == "&" || s == "|")
            {
                int j = cur_pos + 1;
                if (j >= code.Length)
                {
                    cur_pos = j;
                    return s;
                }
                char str = code[j];
                if (s == "&" && str == '&' || s == "|" && str == '|')
                {
                    s += str;
                    j++;
                    cur_pos = j;
                }
                else if (isNum(str) || isLetter(str) || str == ' ')
                {
                    cur_pos++;
                }
                else
                {
                    string temp = s;
                    s = "undefined symbol: " + temp + str;
                }
            }
            else
            {
                cur_pos++;
            }
            return s;
        }

        static string Number(string s)
        {
            int j = cur_pos + 1;
            bool flag = true;
            while(flag)
            {
                if(isNum(code[j]))
                {
                    s += code[j];
                    j++;
                }
                else
                {
                    flag = false;
                }
            }
            cur_pos = j;
            return s;
        }
        
        static int indexOf(string identifier)
        {
            for(int i = 0; i < identifier_list.Count; i++)
            {
                if (identifier == identifier_list[i]._value)
                    return identifier_list[i]._id;
            }
            return -1;
        }

        static void TakeWord()
        {
            int k;
            while(cur_pos < code.Length)
            {
                string str1;
                char str;
                str = code[cur_pos];
                k = typeword(str);
                switch (k)
                {
                    case 1: //字母
                        {
                            str1 = identifier(Convert.ToString(str));
                            if (isKeywordOrSymbol(str1) != -1)
                            {
                                RESULT temp;
                                temp._content = str1; // str1
                                temp._value = "_"; // value
                                temp._class = isKeywordOrSymbol(str1); //class
                                result.Add(temp);
                            }
                            else
                            {
                                RESULT temp;
                                temp._content = str1;
                                temp._class = 0; //标识符

                                int index = indexOf(temp._content);
                                if (index == -1)
                                {
                                    _keyword_and_symbol tmp;
                                    tmp._id = identifier_list.Count;
                                    tmp._value = temp._content;
                                    identifier_list.Add(tmp);
                                }
                                index = index == -1 ? identifier_list.Count - 1 : index;
                                temp._value = Convert.ToString(identifier_list[index]._id);

                                result.Add(temp);
                            }
                            break;
                        }
                    case 2: //数字
                        {
                            str1 = Number(Convert.ToString(str));
                            RESULT temp;
                            temp._value = temp._content = str1;
                            temp._class = 1;
                            result.Add(temp);
                            break;
                        }
                    case 3:
                        {
                            str1 = symbolStr(Convert.ToString(str));

                            if(str1.Contains("undefined symbol:"))
                            {
                                isPassed = false; error = str1; return;
                            }

                            RESULT temp;
                            temp._content = str1;
                            temp._value = "_";
                            temp._class = isKeywordOrSymbol(str1); //边界符/运算符
                            result.Add(temp);
                            break;
                        }
                    case 4: cur_pos++; break;
                    default: isPassed = false; error = "undefined symbol: " + str; return;
                }
            }
         }
        static void Main(string[] args)
        {
            init();
            LoadCode();
            TakeWord();
            if(!isPassed)
            {
                Console.WriteLine("出错了！错误信息是" + error);
                return;
            }
            Console.WriteLine("下面是词法分析的结果：\ncontent\t\t<class, value>\n");
            StreamWriter file = new StreamWriter("result.txt");
            StreamWriter file1 = new StreamWriter("identifier.txt");

            file.WriteLine("content\t\t<class, value>\n");
            for (int i = 0; i < result.Count; i++)
            {
                Console.WriteLine(result[i]._content + "\t\t" + "<" + result[i]._class + ", " + result[i]._value + ">");
                file.WriteLine(result[i]._content + "\t\t" + "<" + result[i]._class + ", " + result[i]._value + ">");
            }

            Console.WriteLine("\n正在写入文件......");
            file1.WriteLine("id\t\tcontent\n");

            for (int i = 0; i < identifier_list.Count; i++)
            {
                file1.WriteLine(identifier_list[i]._id + "\t\t" + identifier_list[i]._value);
            }
            file.Flush();
            file1.Flush();
            file.Close();
            file1.Close();

            Console.WriteLine("写入完成！");
        }

    }
}
