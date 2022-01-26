using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.Converters
{
    [ValueConversion(typeof(ResultState), typeof(SolidColorBrush))]
    public sealed class ResultRectangleColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((ResultState)value == ResultState.Palindrome) return new SolidColorBrush(Colors.LightGreen);
            else if ((ResultState)value == ResultState.NotPalindrome) return new SolidColorBrush(Colors.IndianRed);
            else if ((ResultState)value == ResultState.ServerOverloaded) return new SolidColorBrush(Colors.Red);
            else return new SolidColorBrush(Colors.Orange);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
