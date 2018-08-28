# Webhook Cache Invalidation

The example code for the [Clearing Obsolete Cache Entries with Webhooks](https://kenticocloud.com/blog/clearing-obsolete-cache-entries-with-webhooks) article in the [Kentico Cloud blog](https://kenticocloud.com/blog).

The example ASP.NET MVC Core app showcases how webhooks in Kentico Cloud can be used to invalidate content cached in web app's memory cache.

## How To Run the Code

Clone the repo, open the [WebhookCacheInvalidationMvc.sln](https://github.com/Kentico/cloud-articles-examples/blob/master/cloud-example-webhook-cache-invalidation/WebhookCacheInvalidationMvc.sln) solution. Edit the "KenticoCloudProjectId" and "KenticoCloudWebhookSecret" values in the [appsettings.json](https://github.com/Kentico/cloud-articles-examples/blob/master/cloud-example-webhook-cache-invalidation/WebhookCacheInvalidationMvc/appsettings.json) file. The latter one should be generated in Kentico Cloud > main menu > Webhooks > Create new Webhook. In that dialog, also insert the publicly routable URL address of your app with "/webhook" at its end (e.g. http://example.com/webhook).

![Analytics](https://kentico-ga-beacon.azurewebsites.net/api/UA-69014260-4/Kentico/cloud-articles-examples/master/cloud-example-webhook-cache-invalidation/README.md?pixel)
