using Microsoft.Extensions.Caching.Memory;
using DataTransferObjects.Adfs;


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
			if(context.Request.Path != "/login/oauth/callback")
			{
				string cookieString = context.Request.Cookies["_id"];
                AccessTokenResponse token = _cache.Get<UserProfileDetails>(Guid.Parse(cookieString)).token_details;
                context.Request.Headers.Authorization = $"Bearer {token.access_token}";
                context.Response.Headers.Remove("Authorization");

            }
            await _next(context);


        }
	}
}
