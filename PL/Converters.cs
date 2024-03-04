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


/// <summary> convert task id to width of the rectangle in the gant chart </summary>
public class TaskIdToWidth : IValueConverter
{
	static readonly IBl s_bl = BlApi.Factory.Get();

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		BO.Task task = s_bl.Task.Read((int)value);

		if (task.ScheduledDate is null || task.ForecastDate is null)
			return 10;

		return (int)(((DateTime)task.ForecastDate - (DateTime)task.ScheduledDate).TotalDays) / ConstantValues.GANTT_CHART_MAGIC_NUMBER;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

/// <summary> convert task id to Cnvas.Left of the rectangle in the gant chart </summary>
public class TaskIdToLeft : IValueConverter
{
	static readonly IBl s_bl = BlApi.Factory.Get();

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		int taskId = (int)value;
		BO.Task task = s_bl.Task.Read(taskId);

		if (task.ScheduledDate is null)
			return 0;

		return (int)((DateTime)task.ScheduledDate - s_bl.Clock.CurrentTime).TotalDays / ConstantValues.GANTT_CHART_MAGIC_NUMBER;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

/// <summary> convert task id to Cnvas.Top of the rectangle in the gant chart </summary>
public class TaskIdToTop : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return ((int)value + 1) * 30;
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
		return $"Alias: {task.Alias}\n"
			+ $"Description: {task.Description}\n"
			+ $"Status: {task.Status}\n"
			+ $"Scheduled Date: {task.ScheduledDate}\n"
			+ $"Forecast Date: {task.ForecastDate}\n";
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

/// <summary> convert the status to visibility of the button </summary>
public class ConvertStatusToVisibility : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (BO.Status)value == BO.Status.Unscheduled ? Visibility.Visible : Visibility.Collapsed;
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
