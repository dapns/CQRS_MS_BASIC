using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Response
{
    /// <summary>
    /// Marker interface to define a Single Response
    /// </summary>
    public interface ISingleResponse { }

    /// <summary>
    /// Define a single response with a payload
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface ISingleResponse<out TModel> : ISingleResponse where TModel : class
    {
        TModel Data { get; }
    }
}
