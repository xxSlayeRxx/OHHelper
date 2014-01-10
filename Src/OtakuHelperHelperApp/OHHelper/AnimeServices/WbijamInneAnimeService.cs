using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;

namespace OHHelper.AnimeServices
{
    public class WbijamInneAnimeService : AnimeServiceBase
    {
        public override Anime Parse(string url)
        {
            Wait(100);
            using (Browser = new IE(url))
            {
                try
                {
                    Wait(500);
                    var isValid = !Browser.Text.Contains("HTTP 404") || !string.IsNullOrWhiteSpace(Browser.Div(Find.ById("tresc_lewa")).Text) || !Browser.Text.Contains("Nie można wyświetlić tej strony");
                    if (!isValid)
                    {
                        throw new ArgumentException();
                    }
                    Wait(1);

                    var eps = new List<Ep>();

                    var animeTitle = Browser.Para(Find.ByClass("pod_naglowek")).Text.TrimEnd(':');
                    var anime = new Anime()
                    {
                        Name = animeTitle,
                        Url = url,
                        Eps = eps
                    };

                    var div = Browser.Div(Find.ById("tresc_lewa"));

                    var links = div.Links.Where(l => l.GetAttributeValue("href").Contains("wideo-vk-")).Reverse();

                    GenerateEps(eps, links);

                    ReturnAnimeObject = anime;

                }
                catch (Exception e)
                {
                    ReturnAnimeObject = null;
                }
                finally
                {
                    CleanUp();
                }
            }

            return ReturnAnimeObject;
        }
 
    }
}