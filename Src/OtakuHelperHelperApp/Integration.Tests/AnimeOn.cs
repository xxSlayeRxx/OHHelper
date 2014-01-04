using FluentAssertions;
using WatiN.Core;
using Xunit;
using Xunit.Extensions;

namespace Integration.Tests
{
    public class AnimeOn : AnimeTest
    {
        private string AnimeTitle = "Machine-Doll wa Kizutsukanai";
        private const string UrlToAnime = "http://animeon.pl/anime/machine_doll_wa_kizutsukanai/";


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
            _browser.GoTo("http://animeon.pl/anime/machine_doll_wa_kizutsukanjkljjkai/");
            // Assert
            _browser.Url.Should().Be("http://animeon.pl/anime/");

            _browser.GoTo(UrlToAnime + "asda");
            _browser.Text.Should().Contain("404 Not Found");
        }


        // Anime has right title
        [Fact]
        public void Anime_has_right_title()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            var title = _browser.Element(Find.BySelector("h1.float-left"));
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
            var linkTable = _browser.Div(Find.BySelector("div.anime-desc:nth-child(5)"));
            // Assert
            linkTable.Links.Should().NotBeEmpty();

            foreach (var link in linkTable.Links)
            {
                link.GetAttributeValue("href").Should().Contain("http://animeon.pl/anime.php");
            }
        }
    }
}
