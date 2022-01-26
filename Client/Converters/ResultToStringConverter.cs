using System;
using System.Globalization;
using System.Windows.Data;

namespace Client.Converters
{
    public sealed class ResultToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((ResultState)value == ResultState.Palindrome) return "Палиндром";
            else if ((ResultState)value == ResultState.NotPalindrome) return "Не палиндром";
            else if ((ResultState)value == ResultState.ServerOverloaded) return "Сервер перегружен,\nпопробуйте позже";
            else if ((ResultState)value == ResultState.SentToCheck) return "Отправлено\nна проверку";
            else if ((ResultState)value == ResultState.TryAgain) return "Попробуйте снова";
            else return "Не проверено";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
