using System;
using System.Web;
using System.Web.Routing;
using Cogs.Common;
using Cogs.Server.Handlers;

namespace Cogs.Server
{
	public class CogRouteHandler<T> : IRouteHandler
		where T : CogHandlerBase
	{
		public IComponentContainer Container { get; set; }

		public CogRouteHandler(IComponentContainer container)
		{
			Container = container;
		}

		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			var handler = Container.Get<T>();
			handler.RouteData = requestContext.RouteData;
			return handler;
		}
	}
}