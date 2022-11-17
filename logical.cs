using System;
using System.Collections.Generic;
using System.Text;

namespace MIPS
{
    internal class logical
    {
        private string num_bi;
        public logical(string num) { 
            this.num_bi = num;
        }
        public string logical_or(string num2)
        {
            string string1 = this.num_bi;
            string string2 = num2;
            if (this.num_bi.Length != num2.Length)
            {
                string fill = "";
                if (num_bi.Length > num2.Length)
                {
                    for (int i = 0; i < num_bi.Length-num2.Length; i++)
                    {
                        fill = fill + "0";
                    }
                    string2 = fill + string2;

                }
                else
                {
                    for (int i = 0; i <   num2.Length- num_bi.Length; i++)
                    {
                        fill = fill + "0";
                    }
                    string1 = fill + string1;
                }

            }

            string output = "";
            for (int i = 0; i < string1.Length; i++)
            {
                if (string1[i] == '1' || string2[i] == '1')
                {
                    output = output + "1";
                }
                else
                    output = output + "0";
            }
            return output;
        
        }
        public string logical_and(string num2)
        {
            string string1 = this.num_bi;
            string string2 = num2;
            if (this.num_bi.Length != num2.Length)
            {
                string fill = "";
                if (num_bi.Length > num2.Length)
                {
                    for (int i = 0; i < num_bi.Length - num2.Length; i++)
                    {
                        fill = fill + "0";
                    }
                    string2 = fill + string2;

                }
                else
                {
                    for (int i = 0; i < num2.Length - num_bi.Length; i++)
                    {
                        fill = fill + "0";
                    }
                    string1 = fill + string1;
                }

            }

            string output = "";
            for (int i = 0; i < string1.Length; i++)
            {
                if (string1[i] == '1' && string2[i] == '1')
                {
                    output = output + "1";
                }
                else
                    output = output + "0";
            }
            return output;

        }
    }
}
