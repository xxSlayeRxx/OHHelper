using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using WatiN.Core;

namespace OHHelper.AnimeServices
{
    public class AnimeZoneService : AnimeServiceBase
    {
        public override Anime Parse(string url)
        {
            try
            {
                Wait(100);
                Browser = new IE(url, false);
                Wait(500);

                if (Browser.Url != url)
                {
                    throw new ArgumentException();
                }
                Wait(1);

                var eps = new List<Ep>();

                var anime = new Anime
                {
                    Name =
                        Browser.Element(Find.BySelector("div.post:nth-child(1) > div:nth-child(1) > h2:nth-child(1)"))
                            .Text,
                    Url = url,
                    Eps = eps
                };
                Wait(1);
                var links = Browser.Table(Find.BySelector("table.border-c2")).Links.Reverse();
                
                GenerateEps(eps, links);

                //Browser.ForceClose();
                ReturnAnimeObject = anime;
            }
            catch
            {
                ReturnAnimeObject = null;
            }
            finally
            {
                Browser.Close();
            }
            return ReturnAnimeObject;
        }
    }
}