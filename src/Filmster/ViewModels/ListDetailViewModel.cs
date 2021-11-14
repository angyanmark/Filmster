using Filmster.Common.Services;
using Filmster.Services;
using Filmster.ViewModelBases;
using System.Threading.Tasks;
using TMDbLib.Objects.Lists;

namespace Filmster.ViewModels
{
    public class ListDetailViewModel : MediaViewModelBase
    {
        private GenericList _genericList;
        public GenericList GenericList
        {
            get { return _genericList; }
            set { Set(ref _genericList, value); }
        }

        public ListDetailViewModel()
        {
        }

        public async Task LoadList(int id)
        {
            GenericList = await TMDbService.GetListAsync(id);
            if (GenericList == null)
            {
                NavigationService.GoBack();
                return;
            }
        }
    }
}
