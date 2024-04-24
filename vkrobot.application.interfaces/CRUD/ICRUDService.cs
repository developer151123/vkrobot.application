using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace vkrobot.application.interfaces.CRUD
{
    public interface ICRUDService<K, T>
    {
        Task<LoadResult> GetWithLoadOptions(DataSourceLoadOptionsBase loadOptions);
        Task<IEnumerable<T>?> GetAsync();
        Task<T?> GetAsync(K id);
        Task<T?> CreateAsync(T dto);
        Task UpdateAsync(K id, T dto);        
        Task DeleteAsync(K id);
    }
}
