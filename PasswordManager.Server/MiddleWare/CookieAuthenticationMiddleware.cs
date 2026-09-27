using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Memory;
using DataTransferObjects.Adfs;
using System.Text;

namespace PasswordManager.Server.MiddleWare
{
	public class CookieAuthenticationMiddleware : IMiddleware
	{
		private readonly IMemoryCache _cache;
		public CookieAuthenticationMiddleware(IMemoryCache cache) {
			_cache = cache;
		}
		public async Task InvokeAsync(HttpContext context, RequestDelegate _next)
		{
			if(context.Request.Path == "/login/oauth/callback")
			{
                await _next(context);
            }
			else
			{
                AccessTokenResponse token = _cache.Get<AccessTokenResponse>(Guid.Parse(context.Request.Cookies["_id"]));
                context.Request.Headers.Authorization = $"Bearer {token.access_token}" ;
				context.Response.Headers.Remove("Authorization");
                await _next(context);
            }

            

		}
	}
}
