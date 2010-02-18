using System;
using System.IO;
using System.Linq;
using Cogs.Domain;

namespace Cogs.Server.Services
{
	public class FileSystemCogRepository : ICogRepository
	{
		public IConfiguration Configuration { get; set; }

		public FileSystemCogRepository(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public Cog GetCog(string package)
		{
			var cog = new Cog { Package = package, Version = GetCurrentPackageVersion(package) };
			cog.ContentStream = new FileStream(GetCogFileName(cog), FileMode.Open, FileAccess.Read);

			return cog;
		}

		public Cog GetCog(string package, Version version)
		{
			var cog = new Cog { Package = package, Version = version };
			cog.ContentStream = new FileStream(GetCogFileName(cog), FileMode.Open, FileAccess.Read);

			return cog;
		}

		private Version GetCurrentPackageVersion(string package)
		{
			return Directory.GetFiles(GetPackageBasePath(package), "*.cog").Max(f => GetVersionForFilename(f));
		}

		private Version GetVersionForFilename(string file)
		{
			return new Version(Path.GetFileNameWithoutExtension(file));
		}

		private string GetPackageBasePath(string package)
		{
			return Path.Combine(Configuration.PackagesPath, package);
		}

		private string GetCogFileName(Cog cog)
		{
			return Path.Combine(GetPackageBasePath(cog.Package), cog.Version + ".cog");
		}
	}
}
