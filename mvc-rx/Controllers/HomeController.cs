using MvcRx.Models;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System;

namespace MvcRx.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IDeliveryClient deliveryClient) : base(deliveryClient)
        {
            
        }

        public async Task<ViewResult> Index()
        {
            // Use the stock factory method to make the async call an observable.
            var responseObservable = Observable.FromAsync(() => DeliveryClient.GetItemsAsync<Article>(
                new EqualsFilter("system.type", "article"),
                new LimitParameter(3),
                new DepthParameter(0),
                new OrderParameter("elements.post_date")
            ));

            // Returning the MVC action results cannot be made an observer of the responseObservable, hence converting back to a Task.
            return View((await responseObservable.ToTask()).Items);
        }
    }
}
