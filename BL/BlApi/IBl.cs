﻿using BO;

namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }

    public void InitializeDB();
    public void ResetDB();


    /// <summary> Suggest a date for a task </summary>
    /// <param name="task"> The task to suggest a date for </param>
    /// <param name="startProj"> The start date of the project </param>
    /// <returns> The suggested date for the task </returns>
    /// <exception cref="BlCannotUpdateException"> The task cann't be updated </exception>
    public DateTime? SuggestedDate(BO.TaskInList? task, DateTime startProj);

    /// <summary> Saves the start and end date of the project </summary>
    public void SaveScheduledDate();


#region Clock Object
    public DateTime Clock { get; }

    public void InitClock();
    public void AddHours(int hours);
    public void AddDays(int days);
#endregion
}
