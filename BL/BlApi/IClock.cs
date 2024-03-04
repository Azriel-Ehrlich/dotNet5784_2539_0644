namespace BlApi;

public interface IClock
{
    public DateTime CurrentTime { get; }

    public void InitClock();

    public void AddHours(int hours);
    public void AddDays(int days);
    public void AddMinutes(int minutes);
    public void AddSeconds(int seconds);

    /// <summary> Start the clock thread </summary>
    /// <param name="onChange"> The action to do when the clock changes </param>
    public void StartClockThread(Action onChange);

    /// <summary> Stop the clock thread </summary>
    public void StopClockThread();
}
