using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace OHHelper.AnimeServices
{
    public class ShindenHentaiService : AnimeServiceBase
    {
        public override Anime Parse(string url)
        {
            Wait(100);
            using (Browser = new IE(url))
            {
                try
                {
                    Wait(500);
                    bool isValid = !Browser.Text.Contains("strona 404.html nie jest dla ciebie dostępna: ") ||
                                   !Browser.Text.Contains(
                                       "Nie można odnaleźć artykułu! Prawdopodobnie został on usunięty lub adres do niego został zmieniony. Proszę użyć wyszukiwarki.") ||
                                   !Browser.Text.Contains(
                                       " nie jest dla ciebie dostępna: być może adres strony został zmieniony lub usunięty. Proszę skorzystać z wyszukiwarki.");
                    if (!isValid)
                    {
                        throw new ArgumentException();
                    }
                    Wait(1);

                    var eps = new List<Ep>();

                    string animeTitle = Browser.Div(Find.ByClass("title-block")).Text.Replace("(Online)", "").Trim();
                    var anime = new Anime
                    {
                        Name = animeTitle,
                        Url = url,
                        Eps = eps
                    };
                    var animeTitleRegex = new Regex("http://shinden-hentai.info/\\d{0,5}-");
                    var animeTitleRegex2 = new Regex("http://shinden-hentai.info//\\d{0,5}-");
                    
                    Div div = Browser.Div(Find.ByClass("shot-text2"));
                    List<Link> links =
                        div.Links.Where(
                            l =>
                                !string.IsNullOrWhiteSpace(l.Text) && !l.Url.Contains("#") && (animeTitleRegex.IsMatch(l.Url) || animeTitleRegex2.IsMatch(l.Url)))
                            .ToList();

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