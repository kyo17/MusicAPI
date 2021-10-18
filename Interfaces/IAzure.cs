using System;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IAzure
    {
        Task<string> save(byte[] content, string ext, string folder);
        Task remove(string path, string folder);
        Task<string> edit(byte[] content, string ext, string folder, string path);
    }
}
