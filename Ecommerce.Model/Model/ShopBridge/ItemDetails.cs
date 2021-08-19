using System;

namespace Ecommerce.Model
{
    public class ItemDetails : CommonProperties
    {
        
        public Guid ItemId { get; set; }
        public double Cost { get; set; }
    }
}
