﻿using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace STS.Web.Middlewares
{
    public class AdminSafeListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AdminSafeListMiddleware> _logger;
        private readonly string _adminSafeList;

        public AdminSafeListMiddleware(RequestDelegate next, ILogger<AdminSafeListMiddleware> logger, string adminSafeList)
        {
            _adminSafeList = adminSafeList;
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != "GET")
            {
                IPAddress remoteIp = context.Connection.RemoteIpAddress;
                _logger.LogDebug($"Request from Remote IP address: {remoteIp}");

                string[] ip = _adminSafeList.Split(';');

                byte[] bytes = remoteIp.GetAddressBytes();
                bool badIp = true;
                foreach (string address in ip)
                {
                    var testIp = IPAddress.Parse(address);
                    if (testIp.GetAddressBytes().SequenceEqual(bytes))
                    {
                        badIp = false;
                        break;
                    }
                }

                if (badIp)
                {
                    _logger.LogInformation($"Forbidden Request from Remote IP address: {remoteIp}");
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}