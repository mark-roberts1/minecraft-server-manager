using Microsoft.AspNetCore.Mvc.Filters;
using System;
using ServerManager.Rest.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger _logger;
        public ApiExceptionFilterAttribute()
        {
            _logger = Startup.LoggerFactory.GetLogger<ApiExceptionFilterAttribute>();
        }
        public override void OnException(ExceptionContext context)
        {
            _logger.Log(LogLevel.Error, context.Exception);

            base.OnException(context);
        }
    }
}
