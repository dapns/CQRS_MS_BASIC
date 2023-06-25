using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Response
{
    public class CreatedResponse<TModel> : BaseResponse, ICreatedResponse<TModel> where TModel : class
    {
        public CreatedResponse(TModel model, List<KeyValuePair<string, string[]>> validationErrors = null) : base(validationErrors)
        {
            Data = model;
        }

        public string Id { get; set; }
        public TModel Data { get; }

    }
}
