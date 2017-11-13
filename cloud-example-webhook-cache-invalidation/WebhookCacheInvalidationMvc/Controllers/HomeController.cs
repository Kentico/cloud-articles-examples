﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using KenticoCloud.Delivery;
using WebhookCacheInvalidationMvc.Models;

namespace WebhookCacheInvalidationMvc.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IDeliveryClient deliveryClient) : base(deliveryClient)
        {
            
        }

        public async Task<ViewResult> Index()
        {
            var response = await DeliveryClient.GetItemsAsync<Article>(
                new EqualsFilter("system.type", "article"),
                new LimitParameter(3),
                new DepthParameter(1),
                new OrderParameter("elements.post_date")
            );

            return View(response.Items);
        }
    }
}
