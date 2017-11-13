// This code was generated by a cloud-generators-net tool 
// (see https://github.com/Kentico/cloud-generators-net).
// 
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated. 
// For further modifications of the class, create a separate file with the partial class.

using System;
using System.Collections.Generic;
using KenticoCloud.Delivery;

namespace NavigationMenusMvc.Models
{
    public partial class HeroUnit
    {
        public const string Codename = "hero_unit";
        public const string TitleCodename = "title";
        public const string ImageCodename = "image";
        public const string MarketingMessageCodename = "marketing_message";

        public string Title { get; set; }
        public IEnumerable<Asset> Image { get; set; }
        public string MarketingMessage { get; set; }
        public ContentItemSystemAttributes System { get; set; }
    }
}