using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using WatiN.Core;

namespace OHHelper.AnimeServices
{
    public class AnimezoneService : IAnimeSevice
    {
        public Anime Parse(string url)
        {
            Settings.Instance.MakeNewIeInstanceVisible = false;
            var browser = new IE(url, false);

            if (browser.Url != url)
            {
                return null;
            }

            var eps = new List<Ep>();

            var anime = new Anime
            {
                Name =
                    browser.Element(Find.BySelector("div.post:nth-child(1) > div:nth-child(1) > h2:nth-child(1)"))
                        .Text,
                Url = url,
                Eps = eps
            };
            var links = browser.Table(Find.BySelector("table.border-c2")).Links.Reverse();
            var i = 1;
            foreach (var link in links)
            {
                eps.Add(new Ep
                {
                    IsCopied = false,
                    Number = i,
                    Url = link.Url
                });
                i++;
            }
            browser.ForceClose();
            return anime;
        }
    }
}