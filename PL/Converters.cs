﻿using System.Globalization;
using System.Windows;
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
public class  ConvertIdToBool:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((int)value == 0||(int)value<200000000||(int)value>400000000) ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
    
}

