using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows;

namespace laba5
{
    public class Validation
    {
        public static string ValidateInt(TextBox textBox)
        {
            string input = textBox.Text;
            if (ContainsInvalidCharacters(input))
            {
                MessageBox.Show("Введены недопустимые символы. Пожалуйста, исправьте.");
                return null;
            }
            else return input;
        }

        public static string ValidateInput(TextBox textBox)
        {
            string input = textBox.Text;
            if (ContainsInvalidCharacters(input))
            {
                MessageBox.Show("Введены недопустимые символы. Пожалуйста, исправьте.");
                return null;
            }
            else if (IsNumeric(input))
            {
                MessageBox.Show("Строка не может быть исключительно числовым. Пожалуйста, исправьте.");
                return null;
            }
            else return input;
        }

        private static bool ContainsInvalidCharacters(string input)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9\s]");
            return regex.IsMatch(input);
        }

        private static bool IsNumeric(string input)
        {
            return input.All(char.IsDigit);
        }
    }
}
