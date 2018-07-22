using System;
using System.Net.Http;

using Blazor.Shared.Models;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Blazor.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
                //services.Configure<DeliveryOptions>((options) =>
                //{
                //    options.ProjectId = "c7f97958-715f-4d12-8ac2-7432cb124bf9";
                //});

                services.AddSingleton(new DeliveryOptions
                {
                    ProjectId = "a97bde64-f954-4b41-b48f-322056c523ba"
                });

                services.AddTransient<IDeliveryClient, DeliveryClient>(sp => new DeliveryClient(
                    sp.GetRequiredService<DeliveryOptions>())
                    {
                        HttpClient = sp.GetRequiredService<HttpClient>(),
                        CodeFirstModelProvider = { TypeProvider = new CustomTypeProvider() }
                    });
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
