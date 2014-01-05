using System;
using System.Collections.Generic;
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
            Settings.Instance.AutoMoveMousePointerToTopLeft = false;
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

        protected void GenerateEps(List<Ep> eps, IEnumerable<Link> links)
        {
            int i = 1;
            foreach (Link link in links)
            {
                var ep = new Ep
                {
                    IsCopied = false,
                    Number = i,
                    Url = link.Url
                };
                eps.Add(ep);
                i++;
                Wait(1);
            }
        }
    }
}