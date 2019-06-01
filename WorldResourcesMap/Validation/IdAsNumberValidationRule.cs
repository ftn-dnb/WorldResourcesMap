using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorldResourcesMap.Validation
{
    public class IdAsNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string s = value as string;

                int r;
                if (int.TryParse(s, out r))
                {
                    return new ValidationResult(true, null);
                }

                return new ValidationResult(false, "Unesite broj kao oznaku tipa resursa.");
            }
            catch
            {
                return new ValidationResult(false, "Nepoznata greška.");
            }
        }
    }
}
