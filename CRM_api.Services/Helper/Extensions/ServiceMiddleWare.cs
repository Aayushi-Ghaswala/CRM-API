using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
//using Payroll.Utils.Auth;
//using Payroll.Utils.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CRM_api.Services.Helper.Extensions
{
    public class ServiceMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        //private readonly ITokenManager tokenManager;

        public ServiceMiddleWare(RequestDelegate next, ILogger<ServiceMiddleWare> logger)
        {
            this.next = next;
            this.logger = logger;
            //this.tokenManager = tokenManager;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
               // context.Response.ContentType = "application/json";
                //if (await tokenManager.IsActiveToken())
                //{
                    await next.Invoke(context); // calling next middleware
                //}
            }
            catch (Exception ex)
            {
                logger.LogError(ex.InnerException, ex.Message);
            }
        }

    }
}