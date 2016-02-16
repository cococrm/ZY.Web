using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Web.MVC.Models
{
    public class OAuthToken
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public string token_type { get; set; }

        public string error { get; set; }

        public string error_description { get; set; }
    }
}
