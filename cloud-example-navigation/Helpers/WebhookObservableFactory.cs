using System;
using System.Linq;
using System.Reactive.Linq;

using NavigationMenusMvc.Models;

namespace NavigationMenusMvc.Helpers
{
    public static class WebhookObservableFactory
    {
        public static IObservable<WebhookNotificationEventArgs> GetObservable(object sender, string eventName)
        {
            return Observable.FromEventPattern<WebhookNotificationEventArgs>(sender, eventName).Select(e => e.EventArgs);
        }
    }
}
