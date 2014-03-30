using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OrgPortal.Converters
{
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool && (bool)value == true)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility)
            {
                switch ((Visibility)value)
                {
                    case Visibility.Visible:
                        return false;
                    case Visibility.Collapsed:
                        return true;
                }
            }

            return false;
        }
    }
}
