using MPP.Service.Models;

namespace MPP.Service.Interface
{
    public interface IDataService
    {
        IEnumerable<T_MsBusinessUnit> GetDataBusinessUnit();
        IEnumerable<RegionCodeDistinct> GetDataRegionDistinct(IEnumerable<T_MsBusinessUnit> val);
        
    }
}