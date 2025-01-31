using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingerWebSiteIntegration
{
   public  interface IJwtTokenManager
    {
        string Authentication(string userName, string Password);
    }
}
