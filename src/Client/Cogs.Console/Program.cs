using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cogs.Console.Services;

namespace Cogs.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            CogInstallationService cis = new CogInstallationService();
            string[] nameversion = cis.ParseCogName(args[1]);
            cis.InstallCog(new CogServerRepository(), nameversion[0], nameversion[1]);
        }
    }
}
