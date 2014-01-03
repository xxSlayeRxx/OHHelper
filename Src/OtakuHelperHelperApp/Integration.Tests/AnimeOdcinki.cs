using FluentAssertions;
using WatiN.Core;
using Xunit;

namespace Integration.Tests
{
    /*
     * http://anime-odcinki.pl/articles.php?article_id=378
     *  	Kikou Shoujo wa Kizutsukanai
     *  	
     */

    public class AnimeOdcinki : IUseFixture<BrowserFixture>
    {
        private IE _browser;
        private const string UrlToAnime = @"http://anime-odcinki.pl/articles.php?article_id=378";
        private const string AnimeTitle = "Kikou Shoujo wa Kizutsukanai";
        

        [Fact]
        public void There_is_page_in_selected_url()
        {
            // Arrange
            _browser.GoTo(UrlToAnime);
            // Assert
            _browser.Url.Should().Be(UrlToAnime, "Page does not exist");
        }

        // In selected url is right anime
        [Fact]
        public void In_selected_url_is_right_anime()
        {
            // Arrange
            _browser.GoTo(UrlToAnime);
            // Act
            // Assert
            var actualAnimeName = _browser.Element(Find.BySelector("table.spacer:nth-child(5) > tbody:nth-child(1) > tr:nth-child(1) > td:nth-child(1) > div:nth-child(3) > center:nth-child(1) > h2:nth-child(1)"));
            string actualName = actualAnimeName.Text;
            actualName.Should().Be(AnimeTitle);
        }

        // In selected url is not anime
        [Fact]
        public void Anime_is_not_in_selected_url()
        {
            // Arrange

            // Act
            _browser.GoTo(UrlToAnime + 'a');
            // Assert
            _browser.Text.Should().Contain("Ecchi").And.Contain("Josei").And.Contain("Seinen").And.Contain("Hentai");
            
        }

        // Selected url contains anime links
        [Fact]
        public void Selected_url_contains_anime_links()
        {
                // Arrange
                var epList = _browser.Div(
                        Find.BySelector(
                            "table.spacer:nth-child(5) > tbody:nth-child(1) > tr:nth-child(1) > " +
                            "td:nth-child(1) > div:nth-child(3) > " +
                            "center:nth-child(2) > div:nth-child(2)"));
                // Act
                LinkCollection links = epList.Links;
                
                // Assert


                foreach (var link in links)
                {
                    link.Text.Should().Contain(AnimeTitle);
                    link.GetAttributeValue("href").Should().Contain("viewpage.php?page_id");
                }
            
        }


        public void SetFixture(BrowserFixture data)
        {
            _browser = data.BrowserClass;
        }

    }
}