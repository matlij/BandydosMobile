using BandydosMobile.Models.Constants;
using System.Globalization;

namespace SportPlanner.Converters
{
    public class BooleanToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value
                ? IconConstants.OkSymbol
                : IconConstants.NotOkSymbol;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
