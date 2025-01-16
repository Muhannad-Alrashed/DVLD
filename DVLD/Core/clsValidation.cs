using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLD
{
    internal class clsValidation
    {
        public static bool ValidateEmail(string email)
        {
            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            var regEx = new Regex(pattern);

            return regEx.IsMatch(email);

        }

        public static bool ValidateDouble(string input)
        {
            double minValue = double.MinValue;
            double maxValue = double.MaxValue;
            double result;

            if (double.TryParse(input, out result))
            {
                if (result >= minValue && result <= maxValue)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
