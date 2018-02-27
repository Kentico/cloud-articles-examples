using KenticoCloud.Delivery;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UwpRx.Models;

namespace UwpRx
{
    public class DeliveryViewModel : INotifyPropertyChanged
    {
        private string _searchQuery;
        private List<string> _results;

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                RaisePropertyChanged(nameof(SearchQuery));
            }
        }

        public List<string> Results
        {
            get { return _results; }
            set
            {
                _results = value;
                RaisePropertyChanged(nameof(Results));
            }
        }

        public DeliveryClient DeliveryClient { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DeliveryViewModel()
        {
            DeliveryClient = new DeliveryClient("975bf280-fd91-488c-994c-2f04416e5ee3");

            var searchQueryObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, nameof(PropertyChanged))
                .Select(e => ((DeliveryViewModel)e.Sender).SearchQuery)
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromSeconds(1));

            // Option 1: Directly populate the Results via an ordinary async/await call to DeliveryClient.
            //var searchQuerySubscription = searchQueryObservable.Subscribe(async searchQuery => Results = (await DeliveryClient.GetItemsAsync<Article>(new ContainsFilter("elements.persona", searchQuery))).Items.Select(i => i.Title).ToList());

            // Option 2: Make each call to the DeliveryClient also an observable (with that same async call as an observer).
            var resultsObservable = searchQueryObservable.SelectMany(async searchQuery => (await DeliveryClient.GetItemsAsync<Article>(new ContainsFilter("elements.personas", searchQuery), new ElementsParameter("title"))).Items.Select(i => i.Title).ToList());

            // Make the observer code execute through the UI thread's dispatcher and clear the Results upon each (distinct and throttled) keystroke in the text box.
            searchQueryObservable.ObserveOn(SynchronizationContext.Current).Subscribe(searchQuery => Results = null);

            // Do the same thread synchronization for the resultsObservable and populate the Results upon each response from Kentico Cloud.
            resultsObservable.ObserveOn(SynchronizationContext.Current).Subscribe(results => Results = results);
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
