namespace OHHelper
{
    public class ServiceWithLink
    {
        public ServiceWithLink(IAnimeSevice animeSevice, string partOfLink)
        {
            AnimeSevice = animeSevice;
            PartOfLink = partOfLink;
        }

        public IAnimeSevice AnimeSevice { get; set; }

        public string PartOfLink { get; set; }
        
    }
}