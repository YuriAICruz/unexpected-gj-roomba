namespace Roomba.Systems
{
    public interface IDataRepository
    {
        void Save(string key, object value);

        T Load<T>(string key);
    }
}