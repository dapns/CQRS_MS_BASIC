using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        public LoggingBehaviour(ILogger logger)
        {
            _logger = logger;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //Query
            //_logger.Information($"Handling {typeof(TRequest).Name}");
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");
            Type myType = request.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(request, null);
                //_logger.Information("{Property} : {@Value}", prop.Name, propValue);
                _logger.LogInformation("{Property} : {@Value}", prop.Name, propValue);
            }
            var response = await next();
            //FilterResponse
            //_logger.Information($"Handled {typeof(TResponse).Name}");
            _logger.LogInformation($"Handled {typeof(TResponse).Name}");
            return response;

        //    _logger.LogInformation($"[START] Handling {typeof(TRequest).Name}");
        //    var stopwatch = Stopwatch.StartNew();
        //    TResponse response;
        //    try
        //    {
        //        try
        //        {
        //            var requestData = JsonSerializer.Serialize(request);
        //            _logger.LogInformation($"[DATA] With data: {requestData}");
        //        }
        //        catch (System.Exception)
        //        {
        //            _logger.LogInformation("[Serialization ERROR] Could not serialize the request.");
        //        }
        //        response = await next();
        //    }
        //    finally
        //    {
        //        stopwatch.Stop();
        //        _logger.LogInformation(
        //            $"[END] Handled {typeof(TResponse).Name}; Execution time = {stopwatch.ElapsedMilliseconds}ms");
        //    }
        //    return response;
        }
    }
}
