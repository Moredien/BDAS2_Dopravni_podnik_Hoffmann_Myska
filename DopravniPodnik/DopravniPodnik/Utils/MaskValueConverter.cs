using System.Globalization;
using System.Windows.Data;

namespace DopravniPodnik.Utils;

public class MaskValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Use the static IsMasked property from the MaskSettings class
        bool isMasked = UserSession.Instance.IsSafeModeOn;
        
        if (isMasked)
            return new string('*', value.ToString().Length);
        return value; // Return the original value
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // Not needed for one-way binding
    }
}