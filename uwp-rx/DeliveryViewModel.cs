using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
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
            // The Select operator extracts the query. See http://www.introtorx.com/Content/v1.0.10621.0/08_Transformation.html#Select
            // The Throttle operator buffers the user keystrokes for one second before they become visible to the client code. See http://www.introtorx.com/Content/v1.0.10621.0/13_TimeShiftedSequences.html#Throttle
            // The DistinctUntilChanged operator guarantees that only unique search phrases are passed around. See http://www.introtorx.com/Content/v1.0.10621.0/05_Filtering.html#Distinct
            var searchQueryObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, nameof(PropertyChanged))
                .Select(e => ((DeliveryViewModel)e.Sender).SearchQuery)
                .Throttle(TimeSpan.FromSeconds(1))
                .DistinctUntilChanged();

            // The ObserveOn operator makes sure the observer code listens for notifications in the UI thread. See http://www.introtorx.com/Content/v1.0.10621.0/15_SchedulingAndThreading.html#SubscribeOnObserveOn
            // The Subscribe operator declares the observer code. It clears Results upon each (distinct and throttled) keystroke in the text box.
            searchQueryObservable
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(searchQuery => Results.Clear());

            // Use the SelectMany operator to:
            // 1. Listen to the user input via searchQueryObservable,
            // 2. Attach another newly created anonymous Observables onto it,
            // 3. Project each sequence of Kentico Cloud content items into one flat observable sequence.
            var resultsObservable = searchQueryObservable.SelectMany(searchQuery => GetArticlesObservable(searchQuery));

            // The SubscribeOn operator offloads the work to a background thread. See http://www.introtorx.com/Content/v1.0.10621.0/15_SchedulingAndThreading.html#SubscribeOnObserveOn
            // It is also required to use the ObserveOn operator to get notified back on the UI thread.
            // The observer populates Results with the Kentico Cloud response.
            resultsObservable
                .SubscribeOn(NewThreadScheduler.Default)
                .ObserveOn(SynchronizationContext.Current)
                .Select(a => a.Title)
                .Subscribe(result => Results.Add(result));
        }

        #endregion

        #region "Public methods"

        public IObservable<Article> GetArticlesObservable(string searchQuery)
        {
            return DeliveryObservableProxy.GetItemsObservable<Article>(new ContainsFilter("elements.personas", searchQuery), new DepthParameter(0));
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
