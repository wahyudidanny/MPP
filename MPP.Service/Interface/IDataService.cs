using MPP.Service.Models;

namespace MPP.Service.Interface
{
    public interface IDataService
    {
        IEnumerable<T_MsBusinessUnit> GetDataBusinessUnit();
    }
}