using System;
using System.Collections.Generic;
using System.Text;

namespace FTPTool
{
    class Security
    {
        private static int[] mKey;

        private static void Let(string str)
        {

            int i = 0;
            int mSeed = 0;
            mKey = new int[31];
            for (i = 0; i < str.Length; i++)
            {
                mSeed = mSeed + (((short)(Convert.ToChar(str.Substring(i, 1)))) * (i + 1)) % 177;
            }

            for (i = 0; i < 31; i++)
            {
                mKey[i] = (mSeed * i) % (128 - i);
            }
        }

        public static string Decrypt(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                int i = 0;
                int pos = 0;
                string AdjPos = "";
                string data = "";
                string[] DataArry = new string[100];
                int cnt = 0;
                string tmp = "";
                Let("Liteon");
                cnt = (short)Convert.ToChar((str.Substring(0, 1))) - 35;

                AdjPos = str.Substring(1, cnt);
                data = str.Substring(cnt + 1);
                for (i = 0; i < cnt; i++)
                {
                    pos = (short)Convert.ToChar((AdjPos.Substring(i, 1))) - 35;
                    data = data.Substring(0, pos - 1) + (char)((short)Convert.ToChar(data.Substring(pos - 1, 1)) - 35) + data.Substring(pos);
                }
                for (i = 0; i < data.Length; i++)
                {
                    tmp = tmp + (char)(((short)Convert.ToChar(data.Substring(i, 1)) ^ mKey[(i + 1) % 31]));
                }
                return tmp;
            }
            else
                return string.Empty;
        }

        public static string CreateEncryptString(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string AdjPos = "";
                short code;
                Int16 cnt = 0;
                Let("Liteon");
                string tmp = "";
                for (int i = 0; i < str.Length; i++)
                {
                    code = (short)((short)Convert.ToChar(str.Substring(i, 1)) ^ mKey[(i + 1) % 31]);
                    if (code < 35)
                    {
                        //skip ASC 1~35
                        code += 35;
                        cnt += 1;
                        AdjPos = AdjPos + (char)(35 + i + 1);
                    }
                    tmp = tmp + (char)(code);
                }
                //skip ASC 1~35
                return ((char)(35 + cnt)) + AdjPos + tmp;
            }
            else
                return string.Empty;
        }
    }
}
