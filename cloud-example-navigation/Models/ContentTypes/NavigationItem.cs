﻿// This code was generated by a cloud-generators-net tool 
// (see https://github.com/Kentico/cloud-generators-net).
// 
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated. 
// For further modifications of the class, create a separate file with the partial class.

using System;
using System.Collections.Generic;
using KenticoCloud.Delivery;

namespace NavigationMenusMvc.Models
{
    public partial class NavigationItem
    {
        public const string Codename = "navigation_item";
        public const string TitleCodename = "title";
        public const string ContentItemCodename = "content_items";
        public const string RedirectToItemTypedCodename = "local_redirect";
        public const string ChildNavigationItemsTypedCodename = "child_navigation_items";
        public const string RedirectToUrlCodename = "other_redirect";
        public const string UrlSlugCodename = "url_slug";

        public string Title { get; set; }
        public IEnumerable<object> ContentItem { get; set; }
        //public IEnumerable<object> RedirectToItem { get; set; }
        //public IEnumerable<object> ChildNavigationItems { get; set; }
        public string RedirectToUrl { get; set; }
        public string UrlSlug { get; set; }
        public ContentItemSystemAttributes System { get; set; }
    }
}