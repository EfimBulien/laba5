using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace laba5
{
    public class Validation
    {
        private const int MIN_PASSWORD_LENGTH = 6;

        private const int MAX_PASSWORD_LENGTH = 50;

        private const int MIN_YEAR = 1900;

        public static string ValidateLogin(TextBox textBox)
        {
            string input = textBox.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Логин не может быть пустым.");
                return null;
            }

            if (ContainsInvalidCharacters(input))
            {
                MessageBox.Show("Логин может содержать только английские буквы, цифры и пробелы.");
                return null;
            }

            if (input.Contains(" "))
            {
                MessageBox.Show("Логин не может содержать пробелы.");
                return null;
            }
            
            if (input.Length < MIN_PASSWORD_LENGTH)
            {
                MessageBox.Show($"Логин должен содержать минимум {MIN_PASSWORD_LENGTH} символов. Текущая длина: {input.Length}");
                return null;
            }
            
            if (input.Length > MAX_PASSWORD_LENGTH)
            {
                MessageBox.Show($"Логин не может превышать {MAX_PASSWORD_LENGTH} символов.");
                return null;
            }

            return input;
        }

        public static string ValidateInt(TextBox textBox)
        {
            string input = textBox.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Поле не может быть пустым.");
                return null;
            }

            if (!IsNumeric(input))
            {
                MessageBox.Show("Поле должно содержать только цифры.");
                return null;
            }

            return input;
        }

        /// <summary>
        /// Валидация пароля:
        /// - Только английские буквы (A-Z, a-z)
        /// - Только цифры (0-9)
        /// - Минимальная длина 6 символов
        /// - Максимальная длина 50 символов
        /// - Не может быть пустым
        /// - Не может содержать пробелы
        /// - Не может содержать специальные символы
        /// </summary>
        public static string ValidatePassword(TextBox textBox)
        {
            string input = textBox.Text;
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Пароль не может быть пустым.");
                return null;
            }
            
            if (input.Contains(" "))
            {
                MessageBox.Show("Пароль не может содержать пробелы.");
                return null;
            }
            
            if (input.Length < MIN_PASSWORD_LENGTH)
            {
                MessageBox.Show($"Пароль должен содержать минимум {MIN_PASSWORD_LENGTH} символов. Текущая длина: {input.Length}");
                return null;
            }
            
            if (input.Length > MAX_PASSWORD_LENGTH)
            {
                MessageBox.Show($"Пароль не может превышать {MAX_PASSWORD_LENGTH} символов.");
                return null;
            }
            
            Regex allowedCharsRegex = new Regex(@"^[a-zA-Z0-9]+$");
            if (!allowedCharsRegex.IsMatch(input))
            {
                MessageBox.Show("Пароль может содержать только английские буквы (A-Z, a-z) и цифры (0-9). Специальные символы и пробелы запрещены.");
                return null;
            }
            
            if (!input.Any(char.IsLetter))
            {
                MessageBox.Show("Пароль должен содержать хотя бы одну английскую букву.");
                return null;
            }
            
            if (!input.Any(char.IsDigit))
            {
                MessageBox.Show("Пароль должен содержать хотя бы одну цифру.");
                return null;
            }
            
            return input;
        }

        /// <summary>
        /// Общая валидация текстового поля для русского языка
        /// </summary>
        public static string ValidateRussianInput(TextBox textBox)
        {
            Regex _russianLettersRegex = new Regex(@"^[а-яА-ЯёЁ\s\-]+$");

            string input = textBox.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Поле не может быть пустым.");
                return null;
            }

            if (!_russianLettersRegex.IsMatch(input))
            {
                MessageBox.Show("Поле может содержать только русские буквы, пробелы и дефисы.");
                return null;
            }
            
            return input;
        }

        /// <summary>
        /// Валидация текстового поля для английского языка
        /// </summary>
        public static string ValidateInput(TextBox textBox)
        {
            string input = textBox.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Поле не может быть пустым.");
                return null;
            }

            if (ContainsInvalidCharacters(input))
            {
                MessageBox.Show("Поле может содержать только английские буквы, цифры и пробелы.");
                return null;
            }

            return input;
        }

        /// <summary>
        /// Валидация номера телефона
        /// </summary>
        public static string ValidatePhone(TextBox textBox)
        {
            Regex _phoneRegex = new Regex(@"^(\+7|8)?[\s\-]?\(?[0-9]{3}\)?[\s\-]?[0-9]{3}[\s\-]?[0-9]{2}[\s\-]?[0-9]{2}$");

            string input = textBox.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Номер телефона не может быть пустым.");
                return null;
            }
            
            
            if (!_phoneRegex.IsMatch(input.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "")))
            {
                MessageBox.Show("Введите корректный номер телефона (например: +7-999-123-45-67 или 89991234567)");
                return null;
            }
            
            return input;
        }

        /// <summary>
        /// Валидация российского автомобильного номера
        /// Форматы:
        /// - А123ВС777 (буква, 3 цифры, 2 буквы, 2-3 цифры региона)
        /// - А123ВС 777
        /// - А123ВС-777
        /// - 1234АВ-7 (для такси и мотоциклов)
        /// </summary>
        public static string ValidateCarNumber(TextBox textBox)
        {
            string input = textBox.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Номер автомобиля не может быть пустым.");
                return null;
            }

            string cleanNumber = input.Replace(" ", "").Replace("-", "");

            Regex carNumberRegex = new Regex(@"^[АВЕКМНОРСТУХABEKMHOPCTYX]{1}\d{3}[АВЕКМНОРСТУХABEKMHOPCTYX]{2}\d{2,3}$", RegexOptions.IgnoreCase);

            Regex motoNumberRegex = new Regex(@"^\d{4}[АВЕКМНОРСТУХABEKMHOPCTYX]{2}\d{1}$", RegexOptions.IgnoreCase);

            Regex trailerNumberRegex = new Regex(@"^[АВЕКМНОРСТУХABEKMHOPCTYX]{1}\d{4}[АВЕКМНОРСТУХABEKMHOPCTYX]{2}\d{2,3}$", RegexOptions.IgnoreCase);

            Regex transitNumberRegex = new Regex(@"^Т\d{3}[АВЕКМНОРСТУХABEKMHOPCTYX]{2}\d{2,3}$", RegexOptions.IgnoreCase);

            if (!carNumberRegex.IsMatch(cleanNumber) &&
                !motoNumberRegex.IsMatch(cleanNumber) &&
                !trailerNumberRegex.IsMatch(cleanNumber) &&
                !transitNumberRegex.IsMatch(cleanNumber))
            {
                MessageBox.Show("Введите корректный российский номер автомобиля.\n" +
                               "Форматы:\n" +
                               "- Легковой: А123ВС777\n" +
                               "- Мотоцикл/такси: 1234АВ7\n" +
                               "- Прицеп: А1234ВС777\n" +
                               "- Транзит: Т123ВС777");
                return null;
            }

            return input.ToUpper();
        }

        /// <summary>
        /// Валидация года выпуска автомобиля
        /// Правила:
        /// - Должен быть целым числом
        /// - Диапазон от 1900 до текущего года + 1
        /// - Не может быть пустым
        /// - Не может содержать буквы
        /// </summary>
        public static string ValidateCarYear(TextBox textBox)
        {
            string input = textBox.Text.Trim();
            int currentYear = DateTime.Now.Year;

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Год выпуска не может быть пустым.");
                return null;
            }

            if (!IsNumeric(input))
            {
                MessageBox.Show("Год выпуска должен содержать только цифры.");
                return null;
            }

            if (input.Length != 4)
            {
                MessageBox.Show("Год выпуска должен состоять из 4 цифр (например: 2020).");
                return null;
            }

            if (!int.TryParse(input, out int year))
            {
                MessageBox.Show("Введите корректный год выпуска.");
                return null;
            }

            if (year < MIN_YEAR)
            {
                MessageBox.Show($"Год выпуска не может быть ранее {MIN_YEAR} года.");
                return null;
            }

            if (year > currentYear + 1)
            {
                MessageBox.Show($"Год выпуска не может быть позже {currentYear + 1} года.");
                return null;
            }

            return input;
        }

        /// <summary>
        /// Валидация пробега автомобиля
        /// </summary>
        public static string ValidateMileage(TextBox textBox)
        {
            string input = textBox.Text.Trim();

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Пробег не может быть пустым.");
                return null;
            }

            if (!IsNumeric(input))
            {
                MessageBox.Show("Пробег должен содержать только цифры.");
                return null;
            }

            if (!int.TryParse(input, out int mileage))
            {
                MessageBox.Show("Введите корректное значение пробега.");
                return null;
            }

            if (mileage < 0)
            {
                MessageBox.Show("Пробег не может быть отрицательным.");
                return null;
            }

            if (mileage > 1000000)
            {
                MessageBox.Show("Пробег не может превышать 1 000 000 км.");
                return null;
            }

            return input;
        }

        /// <summary>
        /// Валидация цены автомобиля
        /// </summary>
        public static string ValidatePrice(TextBox textBox)
        {
            string input = textBox.Text.Trim();

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Цена не может быть пустой.");
                return null;
            }

            Regex priceRegex = new Regex(@"^\d+(?:[.,]\d{1,2})?$");
            if (!priceRegex.IsMatch(input))
            {
                MessageBox.Show("Введите корректную цену (например: 1000000 или 1500000.50)");
                return null;
            }

            string normalizedInput = input.Replace(',', '.');

            if (!decimal.TryParse(normalizedInput, out decimal price))
            {
                MessageBox.Show("Введите корректное значение цены.");
                return null;
            }

            if (price <= 0)
            {
                MessageBox.Show("Цена должна быть больше 0.");
                return null;
            }

            if (price > 100000000)
            {
                MessageBox.Show("Цена не может превышать 100 000 000.");
                return null;
            }

            return input;
        }

        private static bool ContainsInvalidCharacters(string input)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9\s\-]");
            return regex.IsMatch(input);
        }

        private static bool IsNumeric(string input)
        {
            return input.All(char.IsDigit);
        }
    }
}