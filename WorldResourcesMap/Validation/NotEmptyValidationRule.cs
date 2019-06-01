using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorldResourcesMap.Validation
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string s = value as string;

                if (s.Length != 0)
                    return new ValidationResult(true, null);

                return new ValidationResult(false, "Polje mora biti popunjeno.");
            }
            catch
            {
                return new ValidationResult(false, "Nepoznata greška.");
            }
        }
    }
}
