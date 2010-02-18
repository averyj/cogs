using System;
using System.Configuration;

namespace Cogs.Server.Services
{
	public class AppSettingsConfiguration : IConfiguration
	{
		public string PackagesPath
		{
			get { return GetValue("PackagesPath"); }
		}

		private string GetValue(string name)
		{
			return ConfigurationManager.AppSettings[name];
		}
	}
}
