using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationReader.Abstraction {
    public interface IStorageProvider<TKey> {
        Task<IEnumerable<IConfiguration<TKey>>> Configurations();
        Task Add(ConfigurationModel model);
        Task<bool> Update(TKey id, ConfigurationModel model);
        Task<IConfiguration<TKey>> Get(string applicatonName, string name);
        Task<IConfiguration<TKey>> Get(TKey id);
    }
}