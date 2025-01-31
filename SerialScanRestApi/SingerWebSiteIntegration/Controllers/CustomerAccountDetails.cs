using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using SingerWebSiteIntegration.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static SingerWebSiteIntegration.Models.DBConnection;

namespace SingerWebSiteIntegration.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class CustomerAccountDetails : ControllerBase
    {
        private DBConnection dbConnection = new DBConnection();
        //private DataSet oDataSet;
        //private DataTable oDataTable = null;
        //private DataRow O_dRow = null;
        [HttpPost("GetCustomer")]
        public IActionResult GetCustomerDetails([FromBody] Customer customer)
        {
            if (customer.account_no == "" && customer.account_rev == "")
            {
               
                return Ok(JsonConvert.SerializeObject("Invalid Input Parameters"));
            }
            else
            {

                try
                {
                    OracleConnection oOracleConnection = dbConnection.GetConnection();
                    string accounts_query = @"SELECT                 id id,
                        original_acc_no,
                        account_no,
                       ifsapp.Customer_Info_API.Get_Name(id) customer_name,
                        ifsapp.customer_info_address_api.Get_Address(ifsapp.Hpnret_Hp_Head_API.Get_Id(account_no,account_rev),'1') Customer_address,                      sales_date sales_date,
                       original_sales_date original_sales_date,
                       closed_date closed_date,
                       SALESMAN_CODE || '-' ||
                       ifsapp.Commission_Receiver_API.Get_Commission_Receiver_Group(SALESMAN_CODE) salesman_code,
                       budget_book_id budget_book_id,
                       length_of_contract length_of_contract,
                       ifsapp.Hpnret_Customer_Guarantor_API.Get_Bad_Reason(ID) bad_reason,
                       ifsapp.Comm_Method_API.Get_Phone_Mobile_No(id) land_phone_no,
                       ifsapp.Comm_Method_API.Get_Mobile(id) mobile_phone_no,
                       ifsapp.Evaluation_Acc_Info_API.Get_Credit_Score(account_no) credit_score,
                       ifsapp.Loyalty_Customer_Info_API.Point_Balance(id) loyalty_points,
                       ifsapp.Hpnret_Hp_Head_API.Get_Products_Count(account_no) no_of_products,
                       ifsapp.Hpnret_Hp_Head_API.Get_Product_Code(account_no) product_code,
                       ifsapp.Hpnret_Hp_Head_API.Get_Nic_No(ID) nic_no,
                       agreed_date_db agreed_date,
                       ifsapp.Hpnret_Pay_Dtl_API.Get_Acc_Arrear_Months(company,
                                                                account_no,
                                                                account_rev) arrear_months,
                       CASE
                         WHEN ifsapp.Hpnret_Field_Collection_API.Get_Customer_Arrears(ACCOUNT_NO,
                                                                               ACCOUNT_REV,
                                                                               SYSDATE) >= 0 THEN
                          ifsapp.Hpnret_Field_Collection_API.Get_Customer_Arrears(ACCOUNT_NO,
                                                                           ACCOUNT_REV,
                                                                           SYSDATE)
                         ELSE
                          0
                       END customer_arrears,
                       updated_date updated_date,
                       ifsapp.Hpnret_Hp_Head_API.Get_Total_Suraksha(account_no,
                                                             account_rev) total_suraksha,
                       ifsapp.Hpnret_Hp_Head_API.Get_Total_Serv_Chg(account_no,
                                                             account_rev) total_service_charge,
                       ifsapp.Hpnret_Hp_Head_API.Get_Total_Sanasuma(account_no,
                                                             account_rev) total_sanasuma,
                       ifsapp.Hpnret_Pay_Dtl_API.Get_Acc_Outstnding_Bal(company,
                                                                 account_no,
                                                                 account_rev) total_outstanding_balance,
                       ifsapp.Hpnret_Hp_Head_API.Get_Total_Monthly_Payment(account_no,
                                                                    account_rev) total_monthly_payment,
                       ifsapp.Hpnret_Hp_Head_API.Get_Total_List_Price(account_no,
                                                               account_rev) total_list_price,
                       pay_term_id pay_term_id,
                       extended_warranty extended_warranty,
                       DECODE(insurance, 1, 'TRUE', 0, 'FALSE') insurance_client,
                       ifsapp.Hpnret_Pay_Dtl_API.Get_Acc_Month_End_Arrear(company,
                                                                   account_no,
                                                                   account_rev) month_end_arrears,
                      
                       promised_date promised_date,
                       promised_date_usr promised_date_usr,
                       ifsapp.Hpnret_Hp_Head_API.Get_Total_Amt_Fin(account_no,
                                                            account_rev) total_amount_financed,
                       ifsapp.Hpnret_Pay_Dtl_API.Get_Acc_Tot_Arrear(company,
                                                             account_no,
                                                             account_rev) total_arrears,
                       DECODE(extended_warranty, 1, 'TRUE', 0, 'FALSE') extended_warranty_client,
                       ifsapp.Hpnret_Hp_Head_API.Get_Sum_Of_Cash_Prices(account_no,
                                                                 account_rev) total_discounted_cash_price,
                       ifsapp.Hpnret_Hp_Head_API.Get_Total_Bb_Hire_Price(account_no,
                                                                  account_rev) total_hire_price,
                       ifsapp.Hpnret_Hp_Head_API.Get_Total_Ext_Plus(account_no,
                                                             account_rev) total_extended_warranty_plus,
                       ifsapp.Hpnret_Hp_Head_API.Get_Total_Full_Serv_Price(account_no,
                                                                    account_rev) total_full_service_price,
                       ifsapp.Hpnret_Hp_Head_API.Get_Total_First_Payment(account_no,
                                                                  account_rev) total_first_payment,
                       ifsapp.Hpnret_Hp_Head_API.Get_Total_Down_Pay(account_no,
                                                             account_rev) total_down_payment,
                       NVL(ifsapp.Hpnret_Pay_Dtl_Receipt_API.Get_Acc_Total_Paid(company,
                                                                         account_no,
                                                                         account_rev),
                           0) total_amount_paid,
                       ifsapp.Hpnret_Hp_Head_API.Get_Gross_Hire_Value(account_no,
                                                               account_rev) total_gross_hire_value_base
                      
                  FROM ifsapp.HPNRET_HP_HEAD_ALL
                 WHERE original_acc_no = '" + customer.account_no+"' AND account_rev = '"+customer.account_rev+"'";
                    OracleCommand cmd_non_serial = new OracleCommand(accounts_query, oOracleConnection);
                    using (OracleDataReader sdr = cmd_non_serial.ExecuteReader())
                    {

                        if (sdr.Read())
                        {

                            customer.nic_no = sdr["nic_no"].ToString();
                            customer.original_acc_no = sdr["original_acc_no"].ToString();
                            customer.account_no = sdr["account_no"].ToString();                      
                            customer.customer_name = sdr["customer_name"].ToString();
                            customer.Customer_address = sdr["Customer_address"].ToString();
                            customer.sales_date = sdr["sales_date"].ToString();
                            customer.original_sales_date = sdr["original_sales_date"].ToString();
                            customer.closed_date = sdr["closed_date"].ToString();
                            customer.salesman_code = sdr["salesman_code"].ToString();
                            customer.budget_book_id = sdr["budget_book_id"].ToString();
                            customer.length_of_contract = sdr["length_of_contract"].ToString();
                            customer.bad_reason = sdr["bad_reason"].ToString();
                            customer.land_phone_no = sdr["land_phone_no"].ToString();
                            customer.mobile_phone_no = sdr["mobile_phone_no"].ToString();
                            customer.credit_score = sdr["credit_score"].ToString();
                            customer.loyalty_points = sdr["loyalty_points"].ToString();
                            customer.no_of_products = sdr["no_of_products"].ToString();
                            customer.product_code = sdr["product_code"].ToString();
                            customer.nic_no = sdr["nic_no"].ToString();
                            customer.agreed_date = sdr["agreed_date"].ToString();
                            customer.arrear_months = sdr["arrear_months"].ToString();
                            customer.customer_arrears = sdr["customer_arrears"].ToString();
                            customer.updated_date = sdr["updated_date"].ToString();
                            customer.total_suraksha = sdr["total_suraksha"].ToString();
                            customer.total_service_charge = sdr["total_service_charge"].ToString();
                            customer.total_sanasuma = sdr["total_sanasuma"].ToString();
                            customer.total_outstanding_balance = sdr["total_outstanding_balance"].ToString();
                            customer.total_monthly_payment = sdr["total_monthly_payment"].ToString();
                            customer.total_list_price = sdr["total_list_price"].ToString();
                            customer.pay_term_id = sdr["pay_term_id"].ToString();
                            customer.extended_warranty = sdr["extended_warranty"].ToString();
                            customer.insurance_client = sdr["insurance_client"].ToString();
                            customer.month_end_arrears = sdr["month_end_arrears"].ToString();
                            //customer.payment_schedule_active = sdr["payment_schedule_active"].ToString();
                            customer.promised_date = sdr["promised_date"].ToString();
                            customer.promised_date_usr = sdr["promised_date_usr"].ToString();
                            customer.total_amount_financed = sdr["total_amount_financed"].ToString();
                            customer.total_arrears = sdr["total_arrears"].ToString();
                            customer.extended_warranty_client = sdr["extended_warranty_client"].ToString();
                            customer.total_discounted_cash_price = sdr["total_discounted_cash_price"].ToString();
                            customer.total_hire_price = sdr["total_hire_price"].ToString();
                            customer.total_extended_warranty_plus = sdr["total_extended_warranty_plus"].ToString();
                            customer.total_full_service_price = sdr["total_full_service_price"].ToString();
                            customer.total_first_payment = sdr["total_first_payment"].ToString();
                            customer.total_down_payment = sdr["total_down_payment"].ToString();
                            customer.total_amount_paid = sdr["total_amount_paid"].ToString();
                            customer.total_gross_hire_value_base = sdr["total_gross_hire_value_base"].ToString();
                           

                            return Ok(JsonConvert.SerializeObject(customer));
                        }
                        else
                        {
                            return Ok(JsonConvert.SerializeObject("Data Not Found!"));
                        }
                    }



                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
            }
            //end
           

        }


    }
}

