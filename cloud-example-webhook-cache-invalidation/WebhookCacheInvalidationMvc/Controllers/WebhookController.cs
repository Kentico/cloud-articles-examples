﻿using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc;
using WebhookCacheInvalidationMvc.Filters;
using WebhookCacheInvalidationMvc.Helpers;
using WebhookCacheInvalidationMvc.Models;
using WebhookCacheInvalidationMvc.Services;

namespace WebhookCacheInvalidationMvc.Controllers
{
    public class WebhookController : BaseController
    {
        protected readonly ICacheManager _cacheManager;

        public WebhookController(IDeliveryClient deliveryClient, ICacheManager cacheManager) : base(deliveryClient) => _cacheManager = cacheManager;

        [ServiceFilter(typeof(KenticoCloudSignatureActionFilter))]
        public IActionResult Index([FromBody] KenticoCloudWebhookModel model)
        {
            switch (model.Message.Type)
            {
                case CacheHelper.CONTENT_ITEM_TYPE_CODENAME:
                case CacheHelper.CONTENT_ITEM_VARIANT_TYPE_CODENAME:
                    switch (model.Message.Operation)
                    {
                        case "archive":
                        case "publish":
                        case "unpublish":
                        case "upsert":
                            foreach (var item in model.Data.Items)
                            {
                                _cacheManager.InvalidateEntry(new IdentifierSet
                                {
                                    Type = model.Message.Type,
                                    Codename = item.Codename
                                });
                            }

                            break;
                        default:
                            return Ok();
                    }

                    // For all other operations, return OK to avoid webhook re-submissions.
                    return Ok();
                default:
                    // For all other types of artifacts, return OK, for the same reason as above.
                    return Ok();
            }
        }
    }
}
