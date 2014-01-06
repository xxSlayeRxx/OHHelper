using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using WatiN.Core;
using WatiN.Core.Exceptions;
using Xunit;

namespace Integration.Tests
{
    public class DiffAnime : AnimeTest
    {
        private const string UrlToAnime = "http://diff-anime.pl/anime/298/kikou-shoujo-wa-kizutsukanai";
        private const string AnimeTitle = "Kikou Shoujo wa Kizutsukanai";


        // There is page in selected anime
        [Fact]
        public void There_is_page_in_selected_anime()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            // Assert
            Login();
            _browser.Url.Should().Be(UrlToAnime);
        }

        private void Login()
        {
            var logoutButton = _browser.Links.FirstOrDefault(l => l.ClassName == "button logout");
            if (logoutButton != null)
            {
                return;
            }

            _browser.TextField(Find.ByName("user_name")).TypeText("xxSlayeRxx");
            _browser.TextField(Find.ByName("user_pass")).TypeText("7qwerty7");
            _browser.Button(Find.ByName("login")).Click();
        }

        // An anime is not in wrong url
        [Fact]
        public void An_anime_is_not_in_wrong_url()
        {
            // Arrange

            // Act
            _browser.GoTo("http://diff-anime.pl/anime/2d98/kikou-shoujo-wa-kizutsukanaia");
            _browser.Text.Should().BeNullOrWhiteSpace();

            _browser.GoTo("http://diff-anime.pl/anime/29118/kikou-shoujo-wa-kizutsukanaia");
            _browser.Text.Should().Contain("Seria nie została znaleziona.");
        }


        // Anime has right title
        [Fact]
        public void Anime_has_right_title()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            Login();
            var title = _browser.Div(Find.BySelector("#pInfo > div:nth-child(1)"));
            // Assert
            title.Text.Should().Be(AnimeTitle);
        }

        // Page has links to eps
        [Fact]
        public void Page_has_links_to_eps()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            Login();
            var links = _browser.Div(Find.ById("pEpisodes")).Links.Where(l => l.Text.Contains("online"));
            // Assert
            var linkList = links as IList<Link> ?? links.ToList();
            linkList.Should().NotBeEmpty();

            foreach (var link in linkList)
            {
                link.GetAttributeValue("href")
                    .Should()
                    .Contain("/odcinek/")
                    .And.Contain("/kikou-shoujo-wa-kizutsukanai");
            }
        }
    }
}