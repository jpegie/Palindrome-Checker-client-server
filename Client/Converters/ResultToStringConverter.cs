using System;
using System.Globalization;
using System.Windows.Data;

namespace Client.Converters
{
    [ValueConversion(typeof(ResultState), typeof(string))]
    public sealed class ResultToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((ResultState)value == ResultState.Palindrome) return "Палиндром";
            else if ((ResultState)value == ResultState.NotPalindrome) return "Не палиндром";
            else if ((ResultState)value == ResultState.ServerOverloaded) return "Сервер перегружен,\nпопробуйте позже";
            else return "Не проверено";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
