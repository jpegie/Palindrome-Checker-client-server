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
            if ((States)value == States.Palindrome) return new SolidColorBrush(Colors.LightGreen);
            else if ((States)value == States.NotPalindrome) return new SolidColorBrush(Colors.IndianRed);
            else if ((States)value == States.ServerOverloaded) return new SolidColorBrush(Colors.Red);
            else if((States)value == States.SentToCheck) return new SolidColorBrush(Colors.Yellow);
            else if((States)value == States.TryAgain) return new SolidColorBrush(Colors.Gray);
            else return new SolidColorBrush(Colors.Orange);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
