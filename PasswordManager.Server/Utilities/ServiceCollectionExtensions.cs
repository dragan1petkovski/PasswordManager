using System.Reflection;


namespace PasswordManager.Server.Utilities
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddScopedFromBaseClass(this IServiceCollection service, System.Type _type)
		{
			var derivedServiceList = Assembly.GetAssembly(_type).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.BaseType == _type).ToList();
			foreach( var derivedService in derivedServiceList)
			{
				service.AddScoped(derivedService);
			}
			return service;
		}
	}
}
