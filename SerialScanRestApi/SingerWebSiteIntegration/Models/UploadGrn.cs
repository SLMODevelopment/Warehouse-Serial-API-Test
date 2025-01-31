using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCD_Application_API.Models
{
    public class UploadGrn
    {
        public int shipment_id { get; set; }

        public string product_code { get; set; }
        public List<string> serial_no { get; set; }
        public string update_user { get; set; }
        public string status { get; set; }


    }
}
