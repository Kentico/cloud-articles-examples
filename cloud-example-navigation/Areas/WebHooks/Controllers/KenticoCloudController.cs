﻿using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using NavigationMenusMvc.Filters;
using NavigationMenusMvc.Helpers;
using NavigationMenusMvc.Models;
using NavigationMenusMvc.Services;
using NavigationMenusMvc.Areas.WebHooks.Models;

namespace NavigationMenusMvc.Areas.WebHooks.Controllers
{
    [Area("WebHooks")]
    public class KenticoCloudController : Controller
    {
        protected IWebhookListener WebhookListener { get; }

        public KenticoCloudController(IWebhookListener kenticoCloudWebhookListener)
        {
            WebhookListener = kenticoCloudWebhookListener ?? throw new ArgumentNullException(nameof(kenticoCloudWebhookListener));
        }

        [HttpPost]
        [ServiceFilter(typeof(KenticoCloudSignatureActionFilter))]
        public IActionResult Index([FromBody] KenticoCloudWebhookModel model)
        {
            switch (model.Message.Type)
            {
                case KenticoCloudCacheHelper.CONTENT_ITEM_SINGLE_IDENTIFIER:
                case KenticoCloudCacheHelper.CONTENT_ITEM_VARIANT_SINGLE_IDENTIFIER:
                case KenticoCloudCacheHelper.CONTENT_TYPE_SINGLE_IDENTIFIER:
                    return RaiseNotificationForSupportedOperations(model.Message.Operation, model.Message.Type, model.Data.Items);
                case KenticoCloudCacheHelper.TAXONOMY_GROUP_SINGLE_IDENTIFIER:
                    return RaiseNotificationForSupportedOperations(model.Message.Operation, model.Message.Type, model.Data.Taxonomies);
                default:
                    // For all other types of artifacts, return OK to avoid webhook re-submissions.
                    return Ok();
            }
        }

        private IActionResult RaiseNotificationForSupportedOperations(string operation, string artefactType, IEnumerable<ICodenamedData> data)
        {
            foreach (var item in data)
            {
                WebhookListener.RaiseWebhookNotification(
                    this,
                    operation,
                    new IdentifierSet
                    {
                        Type = artefactType,
                        Codename = item.Codename
                    });
            }

            return Ok();
        }
    }
}
