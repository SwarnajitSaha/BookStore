using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Utility
{
    public class StripeSettings
    {
        //must to have the same spelling in the Connection String
        public string SecretKey {  get; set; }
        public string PublishableKey { get; set; }
    }
}
