using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Filmster.Helpers
{
    public class SearchBaseDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Movie { get; set; }
        public DataTemplate Tv { get; set; }
        public DataTemplate Person { get; set; }
        public DataTemplate Unknown { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var searchBase = item as SearchBase;
            switch (searchBase.MediaType)
            {
                case MediaType.Movie:
                    return Movie;
                case MediaType.Tv:
                    return Tv;
                case MediaType.Person:
                    return Person;
                default:
                    return Unknown;
            }
        }
    }
}
