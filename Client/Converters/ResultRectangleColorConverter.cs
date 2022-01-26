using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.Converters
{
    public sealed class ResultRectangleColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((ResultState)value == ResultState.Palindrome) return new SolidColorBrush(Colors.LightGreen);
            else if ((ResultState)value == ResultState.NotPalindrome) return new SolidColorBrush(Colors.IndianRed);
            else if ((ResultState)value == ResultState.ServerOverloaded) return new SolidColorBrush(Colors.Red);
            else if((ResultState)value == ResultState.SentToCheck) return new SolidColorBrush(Colors.Yellow);
            else if((ResultState)value == ResultState.TryAgain) return new SolidColorBrush(Colors.Gray);
            else return new SolidColorBrush(Colors.Orange);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
