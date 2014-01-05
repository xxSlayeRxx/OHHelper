using System;
using FluentAssertions;
using WatiN.Core;
using Xunit;
using Xunit.Extensions;

namespace Integration.Tests
{
    public class AnimeZone : AnimeTest
    {
        private const string UrlToAnime = "http://www.animezone.pl/odcinki-online_kikou-shoujo-wa-kizutsukanai";
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
            // Assert
            _browser.Url.Should().Be("http://www.animezone.pl/");
        }

        
        // Anime has right title
        [Fact]
        public void Anime_has_right_title()
        {
            // Arrange
            
            // Act
            _browser.GoTo(UrlToAnime);
            var title = _browser.Element(Find.BySelector("div.post:nth-child(1) > div:nth-child(1) > h2:nth-child(1)"));
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
            var linkTable = _browser.Table(Find.BySelector("table.border-c2"));
            // Assert
            linkTable.Links.Should().NotBeEmpty();
            
            foreach (var link in linkTable.Links)
            {
                link.GetAttributeValue("href").Should().Contain("odcinki-online_kikou-shoujo-wa-kizutsukanai_");
            }
        }
    }
}
