using System.Linq;
using System.Runtime.InteropServices;
using FluentAssertions;
using WatiN.Core;
using Xunit;
using Xunit.Extensions;

namespace Integration.Tests
{
    public class AnimeCentrum : AnimeTest
    {
        private const string UrlToAnime = "http://anime-centrum.net/anime-online-pl/anime-jesien-2013-anime-odcinki-online-pl/716-machine-doll-wa-kizutsukanai-anime-odcinki-online-pl.html";
        private const string AnimeTitle = "Machine-Doll wa Kizutsukanai";


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
            // Assert
            _browser.Text.Should().Contain("HTTP 404");

            _browser.GoTo("http://anime-centrum.net/anime-online-pl/anime-jesien-2013-anime-odcinki-online-pl/7164-machine-doll-wa-kizutsukanai-amnmnnime-odcinki-online-pl.html");
            var div = _browser.Div(Find.ByClass("columLeft"));
            div.Links.Count.Should().Be(2);
        }


        // Anime has right title
        [Fact]
        public void Anime_has_right_title()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            var title = _browser.Element(Find.ByClass("mn_title_in")).Text;
            // Assert
            title.Should().Be(AnimeTitle);
        }

        // Page has links to eps
        [Fact]
        public void Page_has_links_to_eps()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            var div = _browser.Div(Find.BySelector("div.fn_box:nth-child(4) > span:nth-child(1) > div:nth-child(1)"));
            // Assert
            div.Links.Should().NotBeEmpty();

            foreach (var link in div.Links)
            {
                link.GetAttributeValue("href").Should().Contain("http://anime-centrum.net/unbreakable-machine-doll-");
            }
        }

    }
}
