using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.IO;

namespace Cogs.Console.Services.Tests
{
    public class WhenCogInstallationServiceIsCalled
    {
        
        [Fact]
        public void ShouldUnpackCogToCogDirectory()
        {
            CogInstallationService service = new CogInstallationService();
            service.InstallCog(new LocalCogRepository(), "ninject", "1.0");

            Assert.True(Directory.Exists(CogInstallationService.COGSPATH + "ninject-1.0"), "ninject-1.0 directory not created");
            Assert.True(File.Exists(CogInstallationService.COGSPATH + "ninject-1.0/cog/Ninject.Core.dll"), "files not created");
            
        }

        [Fact]
        public void ShouldUnpackCogFromRemoteRepositoryToCogDirectory()
        {
            CogInstallationService service = new CogInstallationService();
            service.InstallCog(new CogServerRepository(), "ninject", "1.0");

            Assert.True(Directory.Exists(CogInstallationService.COGSPATH + "ninject-1.0"), "ninject-1.0 directory not created");
            Assert.True(File.Exists(CogInstallationService.COGSPATH + "ninject-1.0/cog/Ninject.Core.dll"), "files not created");

        }
    }

    public class WhenParseCogNameIsCalled
    {
        [Fact]
        public void ShouldReturnNameAndVersionArray()
        {
            CogInstallationService service = new CogInstallationService();
            string version = "ninject-1.0";
            string[] nameversion = service.ParseCogName(version);
            Assert.Equal<string>(nameversion[0], "ninject");
            Assert.Equal<string>(nameversion[1], "1.0");
        }

        [Fact]
        public void ShouldReturnNameAndEmptyVersionArray()
        {
            CogInstallationService service = new CogInstallationService();
            string version = "ninject";
            string[] nameversion = service.ParseCogName(version);
            Assert.Equal<string>(nameversion[0], "ninject");
            Assert.Equal<string>(nameversion[1], "");
        }
    }
}
