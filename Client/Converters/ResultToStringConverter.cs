using System;
using System.Globalization;
using System.Windows.Data;

namespace Client.Converters
{
    public sealed class ResultToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((States)value == States.Palindrome) return "Палиндром";
            else if ((States)value == States.NotPalindrome) return "Не палиндром";
            else if ((States)value == States.ServerOverloaded) return "Сервер перегружен,\nпопробуйте позже";
            else if ((States)value == States.SentToCheck) return "Отправлено\nна проверку";
            else if ((States)value == States.TryAgain) return "Попробуйте снова";
            else return "Не проверено";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
