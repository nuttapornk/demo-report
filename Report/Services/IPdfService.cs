using System.Threading.Tasks;

namespace Report.Services
{
    public interface IPdfService
    {
        Task<byte[]> CreateDemoReport();
    }
}
