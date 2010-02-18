using System;
using System.Web;
using System.Web.Routing;

namespace Cogs.Server.Handlers
{
	public abstract class CogHandlerBase : IHttpHandler
	{
		public virtual RouteData RouteData { get; set; }

		public virtual bool IsReusable
		{
			get { return false; }
		}

		public abstract void ProcessRequest(HttpContext context);
	}
}