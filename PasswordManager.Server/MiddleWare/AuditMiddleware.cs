using DataTransferObjects.Adfs;
using DataTransferObjects.User;
using Microsoft.Extensions.Caching.Memory;
using Services.Audit;

namespace PasswordManager.Server.MiddleWare
{
	public class AuditMiddleware : IMiddleware
	{
		private UserSession _userSession;
        private IMemoryCache _cache;

        public AuditMiddleware(UserSession userSession, IMemoryCache cache)
		{
			_userSession = userSession;
            _cache = cache;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate _next)
		{

            if( context.Request.Path != "/login/oauth/callback")
			{
                _userSession.SetBasicInfo(_cache.Get<UserProfileDetails>(Guid.Parse(context.Request.Cookies["_id"])).userSession);
                if (context.Request.Method.ToUpper() == "POST" || context.Request.Method.ToUpper() == "PUT" || context.Request.Method.ToUpper() == "DELETE")
                {

                    _userSession.httpMethod = context.Request.Method.ToUpper();
                    _userSession.HttpRequestTime = DateTime.Now;

                }
                
            }
            //TO DO: This need to be developed properly not just for testing purposes
            //Either process JWT token or get data from the User context of the HTTP Reuqest

            // Continue processing the request
            await _next(context);
		}
	}
}
