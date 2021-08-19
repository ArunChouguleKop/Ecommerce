using System;
using System.Collections.Generic;

namespace Ecommerce.Model

{
    public class CommonProperties
    {
        
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        public List<String> Errors { get; set; } = new List<string>();
        public bool IsUsed{ get; set; }
        public bool IsDeleted { get; set; }
        public int StatusType { get; set; }


    }
}
