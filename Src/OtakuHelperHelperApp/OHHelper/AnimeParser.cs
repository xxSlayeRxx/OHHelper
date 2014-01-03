using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace OHHelper
{
    public class AnimeParser : IAnimeParser
    {
        private List<ServiceWithLink> _services;

        public AnimeParser()
        {
            _services = new List<ServiceWithLink>();
        }

        public Anime Parse(string url)
        {
            var service = _services.SingleOrDefault(s => url.Contains(s.PartOfLink));
            return service == null ? null : service.AnimeSevice.Parse(url);
        }

        public AnimeParser WithService(ServiceWithLink service)
        {
            _services.Add(service);
            return this;
        }

        public AnimeParser And(ServiceWithLink service)
        {
            _services.Add(service);
            return this;
        }
    }
}