using BlApi;
using BO;
using System.Globalization;
using System.Windows;
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
/// <summary> convert the date to x position in the gant chart </summary>
public class StatusToColorConverter : IValueConverter
{
	static SolidColorBrush GetBrushFromHex(string hexValue)
	{
		SolidColorBrush brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(hexValue))!;
		return brush;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is Status status)
		{
			return status switch
			{
				Status.Unscheduled => GetBrushFromHex("#C8A2C8"),
				Status.Scheduled => GetBrushFromHex("#90EE90"),
				Status.OnTrack => GetBrushFromHex("#87CEEB"),
				Status.Done => GetBrushFromHex("#B0E0E6"),
				_ => GetBrushFromHex("#ADD8E6"),
			};
		}
		return GetBrushFromHex("#ADD8E6");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}


/// <summary> convert task id to width of the rectangle in the gant chart </summary>
public class TaskIdToWidth : IValueConverter
{
	static readonly IBl s_bl = BlApi.Factory.Get();

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		BO.Task task = s_bl.Task.Read((int)value);

		if (task.ScheduledDate is null || task.ForecastDate is null)
			return 30;

		return (int)(((DateTime)task.ForecastDate - (DateTime)task.ScheduledDate).TotalDays) / ConstantValues.GANTT_CHART_MAGIC_NUMBER;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

/// <summary> convert task id to margin of the rectangle in the gantt chart </summary>
public class TaskIdToMargin : IValueConverter
{
	static readonly IBl s_bl = BlApi.Factory.Get();

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		DateTime? scheduledDate = s_bl.Task.Read((int)value).ScheduledDate;
		int left = 0;
		if (scheduledDate is not null)
		{
			DateTime? startProj= s_bl.GetStartProject();
			left = (int)((DateTime)scheduledDate - (DateTime)startProj!).TotalDays / ConstantValues.GANTT_CHART_MAGIC_NUMBER;
		}
		return $"{left},0,0,0";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

/// <summary> convert task id to full data of the task </summary>
public class TaskIdToString : IValueConverter
{
	static readonly IBl _bl = BlApi.Factory.Get();

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		BO.Task task = _bl.Task.Read((int)value);
		string text = task.Description;
		if (task.ScheduledDate is not null && task.ForecastDate is not null)
			text += $"\nScheduled Date: {task.ScheduledDate}\nForecast Date: {task.ForecastDate}\n";
		return text;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

/// <summary> convert the status to enable of the button </summary>
public class ConvertStatusToEnable : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (BO.Status)value == BO.Status.Unscheduled;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

/// <summary> convert the task to visibility of the button according to its id and status </summary>
public class ConvertTaskToVisibility : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is null)
			return Visibility.Collapsed;
		BO.Task task = (BO.Task)value;
		if (task.Id == ConstantValues.NO_ID)
			return Visibility.Collapsed;
		return task.Status == BO.Status.Unscheduled ? Visibility.Visible : Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

/// <summary> convert boolean value to visibility of control </summary>
public class ConvertBoolToVisibilityKey : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (bool)value ? Visibility.Visible : Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

/// <summary> return button text according to the value (Delete/Restore) </summary>
public class ConvertActiveToContent : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (bool)value ? "Delete" : "Restore";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}


/// <summary> return button visibility according to engineer id (in case engineer has task he couldn't be removed </summary>
public class ConvertEgineerIdToVisibilityKey : IValueConverter
{
	static readonly IBl _bl = BlApi.Factory.Get();
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		IEnumerable<BO.TaskInList> tasks = _bl.Task.ReadAll(t => t.Engineer is not null && t.Engineer.Id == (int)value);
		return tasks.Any() ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
