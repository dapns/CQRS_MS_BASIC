using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Response
{
    public interface ICreatedResponse
    {
        public string Id { get; set; }
    }

    public interface ICreatedResponse<out TModel> : ICreatedResponse where TModel : class
    {
        TModel Data { get; }
    }
}
