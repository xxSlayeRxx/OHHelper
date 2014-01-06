using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WatiN.Core;

namespace OHHelper.AnimeServices
{
    public class DiffAnimeService : AnimeServiceBase
    {
        private readonly DiffAnimeCredentials _diffAnimeCredentials;

        public DiffAnimeService(DiffAnimeCredentials diffAnimeCredentials)
        {
            _diffAnimeCredentials = diffAnimeCredentials;
        }

        public override Anime Parse(string url)
        {
            try
            {
                Wait(1000);
                Browser = new IE(url, false);
                Wait(500);
                Login();
                
                var isValid = string.IsNullOrWhiteSpace(Browser.Text) || Browser.Text.Contains("Seria nie została znaleziona.");
                
                if (isValid)
                {
                    throw new ArgumentException();
                }

                Wait(1000);

                var eps = new List<Ep>();

                var animeTitle = Browser.Div(Find.BySelector("#pInfo > div:nth-child(1)")).Text;

                var anime = new Anime
                {
                    Name = animeTitle,
                    Url = url,
                    Eps = eps
                };
                Wait(1);
                var links = Browser.Div(Find.ById("pEpisodes")).Links.Where(l => l.Text.Contains("online"));

                GenerateEps(eps, links);

                ReturnAnimeObject = anime;
            }
            catch
            {
                ReturnAnimeObject = null;
            }
            finally
            {
                CleanUp();
            }
            return ReturnAnimeObject;
        }

        private void Login()
        {
            var logoutButton = Browser.Links.FirstOrDefault(l => l.ClassName == "button logout");
            if (logoutButton != null)
            {
                return;
            }

            Browser.TextField(Find.ByName("user_name")).TypeText(_diffAnimeCredentials.Login);
            Browser.TextField(Find.ByName("user_pass")).TypeText(_diffAnimeCredentials.Password);
            Browser.Button(Find.ByName("login")).Click();
        }
    }
}