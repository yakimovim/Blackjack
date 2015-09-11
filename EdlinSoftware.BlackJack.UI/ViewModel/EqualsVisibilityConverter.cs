using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EdlinSoftware.BlackJack.UI.ViewModel
{
    public class EqualsVisibilityConverter : IValueConverter
    {
        public object Value { get; set; }

        public bool Not { get; set; }
            
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ShouldBeVisible(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool ShouldBeVisible(object value)
        {
            var result = value?.Equals(Value) ?? Value == null;

            return Not ? !result : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
