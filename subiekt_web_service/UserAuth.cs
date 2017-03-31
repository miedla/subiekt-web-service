using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

namespace subiekt_web_service
{
    public class UserAuth : SoapHeader
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}