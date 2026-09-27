using Services.Audit;

namespace PasswordManager.Server.MiddleWare
{
	public class AuditMiddleware : IMiddleware
	{
		private JwtManager _jwtManager;
		private UserSession _userSession;

        public AuditMiddleware( JwtManager jwtManager, UserSession userSession)
		{
			_jwtManager = jwtManager;
			_userSession = userSession;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate _next)
		{

            //TO DO: This need to be developed properly not just for testing purposes
            //Either process JWT token or get data from the User context of the HTTP Reuqest
            _userSession.SetBasicInfo(_jwtManager.GetUserDetails(context.Request.Headers.Authorization));

            if (context.Request.Method.ToUpper() == "POST" || context.Request.Method.ToUpper() == "PUT" || context.Request.Method.ToUpper() == "DELETE")
			{
                
                _userSession.httpMethod = context.Request.Method.ToUpper();
                _userSession.HttpRequestTime = DateTime.Now;

			}

			// Continue processing the request
			await _next(context);
		}
	}
}
