using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingerWebSiteIntegration
{
    public class JsonParser
    {
        public String getSalesRegisterRes(String status, String message, String internetOrderNo, String lineNo , String ErrorMeg,String IncomingCusOrderNo)
        {
            JObject obj = new JObject();
            obj.Add("status", new JValue(status));
            obj.Add("message", new JValue(message));
            obj.Add("internetOrderNo", new JValue(internetOrderNo));
            obj.Add("lineNo", new JValue(lineNo));
            obj.Add("ErrorMeg", new JValue(ErrorMeg));
            obj.Add("IncomingCusOrderNo", new JValue(IncomingCusOrderNo));
            string output = Convert.ToString(obj);
            return output;
        }
        public String getTokenRequestBody()
        {
            JObject obj = new JObject();
            obj.Add("client_id", "0b6acdf3-135e-4a04-973d-5458c4e6b11c");
            obj.Add("client_secret", "L0yBtkuOXIF0veyGR0G2YWdlBzpLAAHm");
            obj.Add("resource", "0b6acdf3-135e-4a04-973d-5458c4e6b11c");
            obj.Add("scope", "openid");
            obj.Add("username", "SingerAdmin");
            obj.Add("password", "Password");
            obj.Add("grant_type", "password");
            obj.Add("response_type", "id_token token");

            string output = Convert.ToString(obj);
            return output;
        }
        public String getDutyFreePriceRes(String status, double currencyRateUSD, String validFrom)
        {
            JObject obj = new JObject();
            obj.Add("status", status);
            obj.Add("currencyRateUSD", currencyRateUSD);
            obj.Add("validFrom", validFrom);

            string output = Convert.ToString(obj);
            return output;
        }
        public String getNewProductRes(string status, string message, String productCode)
        {
            JObject obj = new JObject();
            obj.Add("status", status);
            obj.Add("message", message);
            obj.Add("productCode", productCode);

            string output = Convert.ToString(obj);
            return output;
        }
    }
}
