using System;
using System.Collections.Generic;
using WatiN.Core;

namespace OHHelper.AnimeServices
{
    public class AnimeCentrumService : AnimeServiceBase
    {
        public override Anime Parse(string url)
        {
            Wait(100);
            using (Browser = new IE(url))
            {
                try
                {
                    Wait(500);
                    var isValid = !Browser.Text.Contains("HTTP 404") || Browser.Div(Find.ByClass("columLeft")).Links.Count <= 2;
                    if (!isValid)
                    {
                        throw new ArgumentException();
                    }
                    Wait(1);

                    var eps = new List<Ep>();

                    var animeTitle = Browser.Element(Find.ByClass("mn_title_in")).Text;
                    var anime = new Anime()
                    {
                        Name = animeTitle,
                        Url = url,
                        Eps = eps
                    };

                    var links = Browser.Div(Find.BySelector("div.fn_box:nth-child(4) > span:nth-child(1) > div:nth-child(1)")).Links;

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