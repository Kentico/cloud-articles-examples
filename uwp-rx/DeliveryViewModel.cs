using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

using KenticoCloud.Delivery;
using KenticoCloud.Delivery.Rx;
using UwpRx.Models;

namespace UwpRx
{
    public class DeliveryViewModel : INotifyPropertyChanged
    {
        #region "Fields"

        private string _searchQuery;
        private readonly ObservableCollection<string> _results = new ObservableCollection<string>();

        #endregion

        #region "Properties"

        public event PropertyChangedEventHandler PropertyChanged;

        public IDeliveryClient DeliveryClient => new DeliveryClient("975bf280-fd91-488c-994c-2f04416e5ee3")
        {
            CodeFirstModelProvider = { TypeProvider = new CustomTypeProvider() }
        };

        public DeliveryObservableProxy DeliveryObservableProxy => new DeliveryObservableProxy(DeliveryClient);

        public string SearchQuery
        {
            get
            {
                return _searchQuery;
            }
            set
            {
                _searchQuery = value;
                RaisePropertyChanged(nameof(SearchQuery));
            }
        }

        public ObservableCollection<string> Results => _results;

        #endregion

        #region "Constructors"

        public DeliveryViewModel()
        {
            Results.CollectionChanged += Results_CollectionChanged;

            // Since Rx is "late to the party" in the .NET ecosystem, Observables are created from traditional .NET events.
            // Although there still has to be an event created, the advantage of Rx is in that there is just one subscriber to the event that has to be maintained--the Observable.
            // The Select operator extracts the query.
            // The DistinctUntilChanged operator guarantees that only unique search phrases are passed around.
            // The Throttle operator buffers the user keystrokes for one second before they become visible to the client code.
            var searchQueryObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, nameof(PropertyChanged))
                .Select(e => ((DeliveryViewModel)e.Sender).SearchQuery)
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromSeconds(1));

            // Clear the Results upon each (distinct and throttled) keystroke in the text box. Make sure the observer code executes in the UI thread via the ObserveOn method.
            searchQueryObservable
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(searchQuery => Results.Clear());

            // Use the SelectMany operator to:
            // 1. Listen to the user input via searchQueryObservable,
            // 2. Attach another newly created anonymous Observables onto it,
            // 3. Project each sequence of Kentico Cloud content items into one flat observable sequence.
            var resultsObservable = searchQueryObservable.SelectMany(searchQuery => GetArticlesObservable(searchQuery));

            // Populate the Results with the Kentico Cloud response.
            resultsObservable
                .ObserveOn(SynchronizationContext.Current)
                .Select(a => a.Title)
                .Subscribe(result => Results.Add(result));
        }

        #endregion

        #region "Public methods"

        public IObservable<Article> GetArticlesObservable(string searchQuery)
        {
            return DeliveryObservableProxy.GetItemsObservable<Article>(new ContainsFilter("elements.personas", searchQuery));
        }

        #endregion

        #region "Private methods"

        private void RaisePropertyChanged(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Results_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Results)));
        }

        #endregion
    }
}
