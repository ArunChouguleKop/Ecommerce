using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Model
{
       public class User : CommonProperties
      {
       
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Token { get; set; }
        public bool IsAdmin { get; set; }
      }
}
