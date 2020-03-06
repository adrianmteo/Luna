using System.Globalization;
using System.Windows.Controls;

namespace Darky
{
    public class NumberRule : ValidationRule
    {
        public int Min { get; set; }

        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse((string)value, out int hour))
            {
                return new ValidationResult(false, "Input is not a number");
            }

            if (hour < Min || hour > Max)
            {
                return new ValidationResult(false, $"Input must be between {Min} and {Max}");
            }

            return ValidationResult.ValidResult;
        }
    }
}
