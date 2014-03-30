using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OrgPortal.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool && (bool)value == true)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility)
            {
                switch ((Visibility)value)
                {
                    case Visibility.Visible:
                        return true;
                    case Visibility.Collapsed:
                        return false;
                }
            }

            return false;
        }
    }
}
