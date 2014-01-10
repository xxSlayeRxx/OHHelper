using System.Linq;
using System.Threading;
using FluentAssertions;
using WatiN.Core;
using Xunit;
using Xunit.Extensions;

namespace Integration.Tests
{
    public class AnimeShinden : AnimeTest
    {
        private const string UrlToAnime = "http://www.anime-shinden.info/26417-kikou-shoujo-wa-kizutsukanai-online.html";
        private const string AnimeTitle = "Kikou Shoujo wa Kizutsukanai";


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
            _browser.Text.Should().Contain("Żądana strona nie została odnaleziona.");

            _browser.GoTo("http://www.anime-shinden.info/264172-kikou-shoujo-wa-kizutsasdsukanai-onliane.html");
            Thread.Sleep(500);
            _browser.Text.Should()
                .Contain(
                    "Nie można odnaleźć artykułu! Prawdopodobnie został on usunięty lub adres do niego został zmieniony. Proszę użyć wyszukiwarki.");
        }


        // Anime has right title
        [Fact]
        public void Anime_has_right_title()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            var animeLink = UrlToAnime.Replace("http://www.anime-shinden.info/",
                        "http://www.anime-shinden.info/online-glowna/");
            var link = _browser.Link(Find.ByUrl(animeLink));
            var animeTitle = link.Text.Replace(" (Online)",string.Empty).Trim();
            // Assert
            link.GetAttributeValue("href").Should().Be(animeLink);
            animeTitle.Should().Be(AnimeTitle);
        }

        // Page has links to eps
        [Fact]
        public void Page_has_links_to_eps()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            var div = _browser.Div(Find.ById(d => d.Contains("news-id-")));
            // Assert

            div.Links.Should().NotBeEmpty();

            foreach (var link in div.Links)
            {
                link.Url.Should().Contain("-kikou-shoujo-wa-kizutsukanai-");
            }
        }
    }
}
