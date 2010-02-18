using System;
using System.IO;
using System.Web;
using System.Web.Routing;
using Cogs.Domain;
using Cogs.Server.Services;

namespace Cogs.Server.Handlers
{
	public class GetCogHandler : CogHandlerBase
	{
		public ICogRepository Cogs { get; set; }

		public GetCogHandler(ICogRepository cogs)
		{
			Cogs = cogs;
		}

		public override void ProcessRequest(HttpContext context)
		{
			var response = context.Response;

			Cog cog = GetRequestedCog(RouteData);

			response.AddHeader("content-disposition", String.Format("attachment; filename={0}-{1}.cog", cog.Package, cog.Version));
			response.AddHeader("content-length", cog.ContentStream.Length.ToString());
			response.ContentType = "application/zip";

			var buffer = new byte[1024];

			using (var reader = new BinaryReader(cog.ContentStream))
			{
				int count;

				do
				{
					count = reader.Read(buffer, 0, buffer.Length);
					response.OutputStream.Write(buffer, 0, count);
				}
				while (count > 0);
			}

			response.Flush();
		}

		private Cog GetRequestedCog(RouteData routeData)
		{
			string package = routeData.GetRequiredString("package");

			if (routeData.Values.ContainsKey("version"))
				return Cogs.GetCog(package, new Version(routeData.Values["version"].ToString()));
			else
				return Cogs.GetCog(package);
		}
	}
}