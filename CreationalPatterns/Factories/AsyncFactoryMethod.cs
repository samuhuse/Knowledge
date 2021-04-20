using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CreationalPatterns.Factories
{
    public class AsyncFactoryMethod
    {
        public class PageViewer
        {
            public string Page { get; private set; }
            private PageViewer()
            {
            }

            private async Task<PageViewer> InitAsync(string url)
            {
                var httpClient = new HttpClient();

                this.Page = await httpClient.GetStringAsync(url);

                return this;
            }

            public static Task<PageViewer> CreateAsync(string url)
            {
                PageViewer viewer = new PageViewer();
                return viewer.InitAsync(url);
            }
        }

        [Test]
        public void TryAsynFactoty()
        {
            async Task Try()
            {
                PageViewer viewer = await PageViewer.CreateAsync("www.google.com");
            }

            Try();
        }
    }
}
