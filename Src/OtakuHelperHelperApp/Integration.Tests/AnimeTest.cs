using WatiN.Core;
using Xunit;

namespace Integration.Tests
{
    public class AnimeTest : IUseFixture<BrowserFixture>
    {
        protected IE _browser;

        public void SetFixture(BrowserFixture data)
        {
            _browser = data.BrowserClass;
        }
    }
}