using BO;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PL;

/// <summary> convert the id to content of the button </summary>
internal class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == ConstantValues.NO_ID ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary> convert the id to enable of the textbox </summary>
internal class ConvertIdToEnable : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == ConstantValues.NO_ID;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
/// <summary>
/// convert the date to x position in the gant chart
/// </summary>
public class StatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Status status)
        {
            switch (status)
            {
                case Status.Unscheduled:
                    return Brushes.Gray;
                case Status.Scheduled:
                    return Brushes.Yellow;
                case Status.OnTrack:
                    return Brushes.Green;
                case Status.Done:
                    return Brushes.Blue;
                default:
                    return Brushes.Gray;
            }
        }
        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
