using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ClientCall
{
    public class BaseClientAsync : HttpClient, IBaseClientAsync
    {
        public Task<ClientResponse<R>> Action<R>(string uri)
        {
            throw new NotImplementedException();
        }

        public Task<ClientResponse<R>> Add<R>(R root, string uri)
        {
            throw new NotImplementedException();
        }

        public Task<ClientResponse<bool>> Delete(string uri)
        {
            throw new NotImplementedException();
        }

        public Task<ClientResponse<bool>> DownloadFile(Stream stream, string uri)
        {
            throw new NotImplementedException();
        }

        public Task<ClientResponse<R>> Get<R>(string uri)
        {
            throw new NotImplementedException();
        }

        public Task<ClientResponse<R>> Update<R>(R root, string uri)
        {
            throw new NotImplementedException();
        }

        public Task<ClientResponse<R>> UploadFile<R>(Stream stream, string file_name, string uri)
        {
            throw new NotImplementedException();
        }
    }
}
