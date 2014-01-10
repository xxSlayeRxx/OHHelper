using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Native.InternetExplorer;

namespace OHHelper.AnimeServices
{
    public class AnimeOdcinkiService : AnimeServiceBase
    {
        public override Anime Parse(string url)
        {
            try
            {
                Wait(100);
                Browser = new IE(url);
                Wait(500);
                Browser.TextFields.First().TypeText("delay");
                bool isInvalidLink = Browser.Text.Contains("Ecchi") && Browser.Text.Contains("Josei") &&
                                     Browser.Text.Contains("Seinen") &&
                                     Browser.Text.Contains("Hentai");

                if (isInvalidLink)
                {
                    throw new ArgumentException();
                }
                Wait(1);

                var eps = new List<Ep>();

                Element actualAnimeName = Browser.Link(l => l.Url == url);

                Wait(1);
                var anime = new Anime
                {
                    Name = actualAnimeName.Text,
                    Url = url,
                    Eps = eps
                };
                Wait(1);
                var td = Browser.TableCell(
                    Find.BySelector("table.spacer:nth-child(5) > tbody:nth-child(1) > tr:nth-child(1) > td:nth-child(1)"));

                var epList = td.Div(d => d.GetAttributeValue("align") == "center").Links.Reverse();
                GenerateEps(eps,epList);
                //Browser.ForceClose();
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
    }
}