using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.IO;

namespace Cogs.Console.Services.Tests
{
    class LocalCogRepositoryFacts
    {
        [Fact]
        public void should_return_stream()
        {
            LocalCogRepository cogRepository = new LocalCogRepository();

            Assert.IsType<FileStream>(cogRepository.RetrieveCogFileStream("ninject", "1.0"));
        }


    }
}
