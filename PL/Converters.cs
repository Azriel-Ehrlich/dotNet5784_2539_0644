using BlApi;
using System.Globalization;
using System.Windows.Data;

namespace PL;

/// <summary>
///  convert the id to content of the button
/// </summary>
internal class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// convert the id to bool
/// to check if we are adding or updating
/// and know if we can esit the id of the Engineer or not
/// </summary>
public class ConvertIdToBool : IValueConverter
{
	// static variable to know if we can change the id or not.
	// maybe we do the same result using the XAML and binding
	// the id to the button, but we dont know how yet
	public static bool CanChangeID = false;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return CanChangeID;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
