namespace PasswordManager.Server.MiddleWare
{
	public static class ApplicationMiddlewareExtension
	{
		public static IApplicationBuilder UseAudit(this IApplicationBuilder app)
		{
			app.UseMiddleware<AuditMiddleware>();
			return app;
		}

		public static IApplicationBuilder UseCookieAuthentication(this IApplicationBuilder app)
		{
			app.UseMiddleware<CookieAuthenticationMiddleware>();
			return app;
		}
		
	}
}
