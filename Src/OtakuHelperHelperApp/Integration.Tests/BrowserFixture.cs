using System;
using WatiN.Core;

namespace Integration.Tests
{
    public class BrowserFixture : IDisposable
    {
        private IE _browserClass = new IE();

        public BrowserFixture()
        {
        }

        public IE BrowserClass
        {
            get { return _browserClass; }
            private set { _browserClass = value; }
        }

        public void Dispose()
        {
            BrowserClass.Close();
            BrowserClass = null;
        }
    }
}