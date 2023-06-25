using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ClientCall
{
    public interface IBaseClientAsync 
    {
        Task<ClientResponse<R>> Add<R>(R root, string uri);
        Task<ClientResponse<R>> Update<R>(R root, string uri);
        Task<ClientResponse<R>> Action<R>(string uri);
        Task<ClientResponse<R>> Get<R>(string uri);
        Task<ClientResponse<bool>> Delete(string uri);
        Task<ClientResponse<R>> UploadFile<R>(Stream stream, string file_name, string uri);
        Task<ClientResponse<bool>> DownloadFile(Stream stream, string uri);
    }
}
