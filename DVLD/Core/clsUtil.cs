using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Core
{
    internal class clsUtil
    {
        public static List<string> Split(string S1 , string Seperator="#//#")
        {
            List<string> result = new List<string>();
            int index;

            if (S1 != null)
            {
                while((index = S1.IndexOf(Seperator)) != -1)
                {
                    string Token = S1.Substring(0, index);
                    result.Add(Token);
                    S1 = S1.Substring(index + Seperator.Length);
                }

                if (S1.Length > 0)
                {
                    result.Add(S1);
                }
            }

            return result;
        }
    }
}
