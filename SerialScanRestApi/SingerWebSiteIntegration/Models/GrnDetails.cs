using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingerWebSiteIntegration.Models
{
    public class GrnDetails
    {


        public int shipment_id { get; set; }
      



    }


    public class DebitPartSerial
    {
        public string DebitNoteNo { get; set; }
        public string order_no { get; set; }
        public string site_code { get; set; }
        public string part_no { get; set; }
        public string serial_no { get; set; }
    }
}
