using System;
using System.Collections.Generic;
using WatiN.Core;

namespace OHHelper.AnimeServices
{
    public class AnimeOnService : AnimeServiceBase
    {
        public override Anime Parse(string url)
        {
            Wait(100);
            using (Browser = new IE(url))
            {
                try
                {
                    Wait(500);
                    if (Browser.Url == "http://animeon.pl/anime/" || Browser.Text.Contains("404 Not Found"))
                    {
                        throw new ArgumentException();
                    }
                    Wait(1);

                    var eps = new List<Ep>();

                    var animeTitle = Browser.Element(Find.BySelector("h1.float-left")).Text;

                    var anime = new Anime()
                    {
                        Name = animeTitle,
                        Url = url,
                        Eps = eps
                    };

                    var links = Browser.Div(Find.BySelector("div.anime-desc:nth-child(5)")).Links;

                    GenerateEps(eps, links);

                    ReturnAnimeObject = anime;

                }
                catch (Exception e)
                {
                    ReturnAnimeObject = null;
                }
                finally
                {
                    Browser.Close();
                }
            }

            return ReturnAnimeObject;
        }
    }
}