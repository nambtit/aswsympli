namespace Application.Abstraction
{
	public interface IAppDataCache
	{
		bool TrySet<T>(string key, T value);

		bool TryGet<T>(string key, out T value);
	}
}
