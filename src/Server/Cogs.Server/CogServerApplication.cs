using System;
using System.Web;
using System.Web.Routing;
using Cogs.Common;
using Cogs.Server.Handlers;
using Cogs.Server.Services;

namespace Cogs.Server
{
	public class CogServerApplication : HttpApplication
	{
		public static IComponentContainer Container { get; set; }

		public override void Init()
		{
			Container = new ComponentContainer();
			RegisterServices();
			RegisterRoutes(RouteTable.Routes);
		}

		public void RegisterRoutes(RouteCollection routes)
		{
			routes.Add(CreateRoute<GetCogHandler>("package/{package}", "GET"));
			routes.Add(CreateRoute<GetCogHandler>("package/{package}/{version}", "GET"));
			routes.Add(CreateRoute<PostCogHandler>("package/{package}/{version}", "POST"));
		}

		public void RegisterServices()
		{
			Container.Add<IConfiguration, AppSettingsConfiguration>();
			Container.Add<ICogRepository, FileSystemCogRepository>();
		}

		private Route CreateRoute<T>(string pattern, string httpMethod)
			where T : CogHandlerBase
		{
			var defaults = new RouteValueDictionary();
			var constraints = new RouteValueDictionary();

			constraints["httpMethod"] = new HttpMethodConstraint(httpMethod);

			return new Route(pattern, defaults, constraints, new CogRouteHandler<T>(Container));
		}
	}
}