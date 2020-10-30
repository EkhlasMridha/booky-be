using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace AuthorizationService.CorsConfig
{
    class CorsPolicyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICorsService _corsService;
        private readonly ICorsPolicyProvider _corsPolicyProvider;
        private readonly string _corsPolicyName = "CustomPolicy";

        public CorsPolicyMiddleware(RequestDelegate next,
                                    ICorsService corsService,
                                    ICorsPolicyProvider corsPolicyProvider)
        {
            _next = next;
            _corsService = corsService;
            _corsPolicyProvider = corsPolicyProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(CorsConstants.Origin))
            {
                var policy = await _corsPolicyProvider.GetPolicyAsync(context, _corsPolicyName);
                if (policy != null)
                {
                    var result = _corsService.EvaluatePolicy(context, policy);
                    _corsService.ApplyResult(result, context.Response);

                    var accessControlRequestMethod =
                   context.Request.Headers[CorsConstants.AccessControlRequestMethod];

                    if(string.Equals(context.Request.Method,CorsConstants.PreflightHttpMethod,StringComparison.Ordinal)
                                        && !StringValues.IsNullOrEmpty(accessControlRequestMethod))
                    {
                        context.Response.StatusCode = StatusCodes.Status204NoContent;
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
