using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using FluentAssertions;
using WatiN.Core;
using Xunit;

namespace Integration.Tests
{
    public class ShindenHentai : AnimeTest
    {
        private const string UrlToAnime = "http://shinden-hentai.info/153-choukou-tenshi-escalayer-online.html";
        private const string AnimeTitle = "Choukou Tenshi Escalayer";


        // There is page in selected anime
        [Fact]
        public void There_is_page_in_selected_anime()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            // Assert
            _browser.Url.Should().Be(UrlToAnime);
        }

        // An anime is not in wrong url
        [Fact]
        public void An_anime_is_not_in_wrong_url()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime + "aasd");
            Thread.Sleep(500);

            // Assert
            _browser.Text.Should().Contain("strona 404.html nie jest dla ciebie dostępna: ");

            _browser.GoTo("http://shinden-hentai.info/15233-choukou-tenshi-escalayer-online.html");
            Thread.Sleep(500);
            _browser.Text.Should()
                .Contain(
                    "Nie można odnaleźć artykułu! Prawdopodobnie został on usunięty lub adres do niego został zmieniony. Proszę użyć wyszukiwarki.");

            _browser.GoTo("http://shinden-hentai.info/15ddd3-choukou-tenshi-escalayer-online.html");
            Thread.Sleep(500);
            _browser.Text.Should()
                .Contain(
                    " nie jest dla ciebie dostępna: być może adres strony został zmieniony lub usunięty. Proszę skorzystać z wyszukiwarki.");
        }


        // Anime has right title
        [Fact]
        public void Anime_has_right_title()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            string animeTitle = _browser.Div(Find.ByClass("title-block")).Text.Replace("(Online)", "").Trim();
            // Assert
            animeTitle.Should().Be(AnimeTitle);
        }

        // Regex is good for looking for link part to remove
        [Fact]
        public void Regex_is_good_for_looking_for_link_part_to_remove()
        {
            //ARRANGE
            var animeTitleRegex = new Regex("http://shinden-hentai.info/\\d{0,5}-");

            //ACT
            animeTitleRegex.IsMatch(UrlToAnime).Should().BeTrue();
            animeTitleRegex.Match(UrlToAnime).Value.Should().Be("http://shinden-hentai.info/153-");
            //ASSERT
        }

        // Page has links to eps
        [Fact]
        public void Page_has_links_to_eps()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            //Thread.Sleep(1000);
            Div div = _browser.Div(Find.ByClass("shot-text2"));

            var animeTitleRegex = new Regex("http://shinden-hentai.info/\\d{0,5}-");
            var animeTitleRegex2 = new Regex("http://shinden-hentai.info//\\d{0,5}-");
            
            // Assert
            

            List<Link> animeLinks =
                div.Links.Where(
                    l =>
                        !string.IsNullOrWhiteSpace(l.Text) && !l.Url.Contains("#") && (animeTitleRegex.IsMatch(l.Url) || animeTitleRegex2.IsMatch(l.Url)))
                       .ToList();
            animeLinks.Should().NotBeNull().And.NotBeEmpty();
        }
    }
}