using System;
using System.Collections.Generic;
using WatiN.Core;

namespace OHHelper.AnimeServices
{
    public class AnimeShinden : AnimeServiceBase
    {
        public override Anime Parse(string url)
        {
            Wait(100);
            using (Browser = new IE(url))
            {
                try
                {
                    Wait(500);
                    var isValid = !Browser.Text.Contains("Żądana strona nie została odnaleziona.") || !Browser.Text.Contains(
                    "Nie można odnaleźć artykułu! Prawdopodobnie został on usunięty lub adres do niego został zmieniony. Proszę użyć wyszukiwarki.");
                    if (!isValid)
                    {
                        throw new ArgumentException();
                    }
                    Wait(1);

                    var eps = new List<Ep>();

                    var link = Browser.Link(Find.ByUrl(url));
                    var animeTitle = link.Text.Replace(" (Online)", string.Empty).Trim();
                    var anime = new Anime()
                    {
                        Name = animeTitle,
                        Url = url,
                        Eps = eps
                    };

                    var links = Browser.Div(Find.ById("news-id-26417")).Links;

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