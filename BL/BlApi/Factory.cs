namespace BlApi;

public static class Factory
{
	private static IBl s_bl = new BlImplementation.Bl();
	public static IBl Get() => s_bl;
}
