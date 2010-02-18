using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cogs.Domain
{
    public class CogConfiguration
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public List<CogDependency> CogDependencies { get; set; }
    }
}
