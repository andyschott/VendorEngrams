using System.Collections.Generic;
using System.Threading.Tasks;

namespace VendorEngrams
{
    public interface IVendorEngramsClient
    {
         Task<IEnumerable<Vendor>> GetVendorDrops(bool visibleOnly = true);
    }
}