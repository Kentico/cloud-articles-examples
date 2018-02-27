using System;
using KenticoCloud.Delivery;

namespace MvcRx.Models
{
    public class CustomTypeProvider : ICodeFirstTypeProvider
    {
        public Type GetType(string contentType)
        {
            switch (contentType)
            {
                case "article":
                    return typeof(Article);
                default:
                    return null;
            }
        }
    }
}