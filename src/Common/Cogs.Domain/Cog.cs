using System;
using System.IO;

namespace Cogs.Domain
{
	public class Cog
	{
		public string Package { get; set; }
		public Version Version { get; set; }
		public Stream ContentStream { get; set; }
	}
}
