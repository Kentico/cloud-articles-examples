using KenticoCloud.Delivery;

namespace MvcRx
{
    public class ProjectOptions
    {
        public DeliveryOptions DeliveryOptions { get; set; }
        public int CacheTimeoutSeconds { get; set; }
    }
}
