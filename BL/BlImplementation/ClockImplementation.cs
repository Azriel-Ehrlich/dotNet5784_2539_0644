namespace BlImplementation;

internal class ClockImplementation : BlApi.IClock
{
    static object s_Lock = new object(); // Lock for the clock
    bool s_runClockThread = false;
    static DateTime s_Clock = DateTime.Now.Date;

    DalApi.IDal _dal = DalApi.Factory.Get;

    public DateTime CurrentTime
    {
        get { lock (s_Lock) { return s_Clock; } }
        private set { lock (s_Lock) { s_Clock = value; } }
    }

    public void InitClock() { CurrentTime = DateTime.Now; }
    public void AddHours(int hours) { CurrentTime = CurrentTime.AddHours(hours); }
    public void AddDays(int days) { CurrentTime = CurrentTime.AddDays(days); }
    public void AddMinutes(int minutes) { CurrentTime = CurrentTime.AddMinutes(minutes); }
    public void AddSeconds(int seconds) { CurrentTime = CurrentTime.AddSeconds(seconds); }
    
    /// <inheritdoc/>
    public void StartClockThread(Action onChange)
    {
        if (!s_runClockThread)
        {
            new Thread(() =>
            {
                s_runClockThread = true;
                while (s_runClockThread)
                {
                    Thread.Sleep(1000);
                    AddMinutes(10);
                    AddSeconds(1);
                    onChange();
                }
            }).Start();
        }
    }
    /// <inheritdoc/>
    public void StopClockThread()
    {
        s_runClockThread = false;
        // Wait for the thread to finish
        while (s_runClockThread)
            Thread.Sleep(100);
    }

	/// <inheritdoc/>
	public void SaveClock()
    {
        _dal.SaveClock(CurrentTime);
    }

	/// <inheritdoc/>
	public void LoadClock()
    {
        CurrentTime = _dal.LoadClock();
	}
}
