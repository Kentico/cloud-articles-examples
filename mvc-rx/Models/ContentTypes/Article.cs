// This code was generated by a cloud-generators-net tool 
// (see https://github.com/Kentico/cloud-generators-net).
// 
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated. 
// For further modifications of the class, create a separate file with the partial class.

using System;
using System.Collections.Generic;
using KenticoCloud.Delivery;

namespace MvcRx.Models
{
    public partial class Article
    {
        public IEnumerable<TaxonomyTerm> Personas { get; set; }
        public string Title { get; set; }
        public IEnumerable<Asset> TeaserImage { get; set; }
        public DateTime? PostDate { get; set; }
        public string Summary { get; set; }
        public string BodyCopy { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public ContentItemSystemAttributes System { get; set; }
    }
}