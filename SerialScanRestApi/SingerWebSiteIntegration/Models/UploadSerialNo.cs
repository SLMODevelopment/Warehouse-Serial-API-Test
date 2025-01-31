using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCD_Application_API.Models
{
    public class UploadSerialNo
    {
       public int shipment_id { get; set; }

        public string product_code { get; set; }

        public string user_no { get; set; }
    }
}
