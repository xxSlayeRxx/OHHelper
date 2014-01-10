using FluentAssertions;
using WatiN.Core;
using Xunit;

namespace Integration.Tests
{
    public class WbijamMainAnime : AnimeTest
    {
        private const string UrlToAnime = "http://www.hunter.wbijam.pl/hunter_x_hunter_2011.html";
        private const string AnimeTitle = "Odcinki serii HunterxHunter 2011";


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

            _browser.GoTo("http://www.hunter.wbijam.pl/huntaer_x_hunter_2011.html");
            var div = _browser.Div(Find.ById("tresc_lewa"));
            div.Text.Should().BeNullOrWhiteSpace();

            _browser.GoTo("http://www.husnter.wbijam.pl/hunter_x_hunter_2011.html");
            _browser.Text.Should().Contain("Nie można wyświetlić tej strony");
        }


        // Anime has right title
        [Fact]
        public void Anime_has_right_title()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime);
            var title = _browser.Para(Find.ByClass("pod_naglowek")).Text.TrimEnd(':');
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
            var div = _browser.Table(Find.ByClass("lista"));
            // Assert
            div.Links.Should().NotBeEmpty();

            foreach (var link in div.Links)
            {
                link.Text.Should().Contain("HunterxHunter2011");
            }
        }

    }
}