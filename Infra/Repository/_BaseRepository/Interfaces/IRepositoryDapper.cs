using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infra.Repository._BaseRepository.Interfaces
{
    public interface IRepositoryDapper<T> where T: class
    {
        IEnumerable<T> GetData(string qry);
    }
}
