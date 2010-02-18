using System;
using Cogs.Domain;

namespace Cogs.Server.Services
{
	public interface ICogRepository
	{
		Cog GetCog(string package);
		Cog GetCog(string package, Version version);
	}
}
