using System;
using System.Threading;
using System.Windows.Threading;
using WatiN.Core;

namespace OHHelper.AnimeServices
{
    public abstract class AnimeServiceBase : IAnimeSevice
    {
        protected IE Browser;
        protected Anime ReturnAnimeObject;

        protected AnimeServiceBase()
        {
            Settings.Instance.MakeNewIeInstanceVisible = false;
        }

        public abstract Anime Parse(string url);

        protected void Wait(int miliseconds)
        {
            var frame = new DispatcherFrame();
            new Thread((ThreadStart)(() =>
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(miliseconds));
                frame.Continue = false;
            })).Start();
            Dispatcher.PushFrame(frame);
        }
    }
}