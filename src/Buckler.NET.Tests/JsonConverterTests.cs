using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Buckler.NET.Models;

namespace Buckler.NET.Tests
{
    public class JsonConverterTests
    {
        [SetUp]
        public void SetUp() { }

        [Test]
        public void CanConvert() 
        {
            var testData = File.ReadAllText(@"TestData\ConverterData\profile.json");
            var parsedData = JsonDocument.Parse(testData).RootElement.GetProperty("pageProps");
            var jsonObj = JsonSerializer.Deserialize<FighterBanner>(parsedData);
        }
    }
}
