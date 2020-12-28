using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon.Tests.EndToEnd.Helpers
{
    public class WebTestBed : TestBed
    {
        private readonly IisExpress iis;
        private readonly HttpClient client;

        public WebTestBed(Type testType, TestContext context, string webAppPath) : base(testType, context)
        {
            iis = new IisExpress(Path.Combine(GetSolutionDir(), webAppPath), (line, _) => context.WriteLine(line));

            client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:" + iis.Port),
            };
        }

        public async Task AssertEqualHttpAsync(string url, string fileName)
        {
            var response = await client.GetStreamAsync(url);
            AssertEqual(response, fileName);
        }

        public override void Dispose()
        {
            iis?.Dispose();
            client?.Dispose();
        }
    }
}
