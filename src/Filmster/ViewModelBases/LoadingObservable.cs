namespace Filmster.ViewModelBases
{
    public class LoadingObservable : Observable
    {
        private bool _dataLoaded;
        public bool DataLoaded
        {
            get { return _dataLoaded; }
            set { Set(ref _dataLoaded, value); }
        }
    }
}
