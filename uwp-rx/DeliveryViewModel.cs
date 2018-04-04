using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using KenticoCloud.Delivery;
using UwpRx.Models;

namespace UwpRx
{
    public class DeliveryViewModel : INotifyPropertyChanged
    {
        private string _searchQuery;
        private List<string> _results;

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

        public List<string> Results
        {
            get
            {
                return _results;
            }
            set
            {
                _results = value;
                RaisePropertyChanged(nameof(Results));
            }
        }

        public DeliveryClient DeliveryClient => new DeliveryClient("975bf280-fd91-488c-994c-2f04416e5ee3");
        public event PropertyChangedEventHandler PropertyChanged;

        public DeliveryViewModel()
        {
            // Since Rx is "late to the party" in .NET, Observables are created from traditional .NET events.
            // Although there has to be an event created, the advantage of Rx is still in that there is just one subscriber to the event that has to be maintained--the Observable.
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
                .Subscribe(searchQuery => Results = null);

            // Use the SelectMany operator to:
            // 1. Listen to the user input via the searchQueryObservable,
            // 2. Attach another Observable onto it,
            // 3. Project each sequence of Kentico Cloud content item names into one flat observable sequence.
            var resultsObservable = searchQueryObservable.SelectMany(async searchQuery => (await GetKenticoCloudResults(searchQuery)));

            // Populate the Results upon each response from Kentico Cloud.
            resultsObservable
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(results => Results = results);
        }

        public async Task<List<string>> GetKenticoCloudResults(string searchQuery)
        {
            return (await DeliveryClient.GetItemsAsync<Article>(
                new ContainsFilter("elements.persona", searchQuery)
                )).Items.Select(i => i.Title).ToList();
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
