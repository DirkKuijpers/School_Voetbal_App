using Microsoft.UI.Xaml.Data;

namespace SchoolVoetbalApp
{
    public class BoolToWinLossConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            if (value is bool b)
            {
                return b ? "Win" : "Loss";
            }
            return "Unknown";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
