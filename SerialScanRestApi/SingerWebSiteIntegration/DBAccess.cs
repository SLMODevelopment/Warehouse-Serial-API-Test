using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Transactions;
using SingerWebSiteIntegration.Models;
using static SingerWebSiteIntegration.Models.DBConnection;

namespace SingerWebSiteIntegration
{
    public class DBAccess
    {
        #region Private Variables

        DBConnection dbConnection = new DBConnection();
        private DataSet oDataSet;
        private DataTable oDataTable = null;
        private DataRow O_dRow = null;

        #endregion


        public DataTable GetPartNumberBySiteId(string siteId)
        {
            DataTable resultTable = new DataTable();

            // Define the query with a parameterized placeholder for CONTRACT
            string query = @"SELECT 
                        t.CATALOG_NO AS Sales_Part_no, 
                        t.CATALOG_DESC AS Description, 
                        t.LIST_PRICE AS Price, 
                        t.LIST_PRICE_INCL_TAX AS Price_Incl_Tax 
                     FROM ifsapp.sales_part t 
                     WHERE t.CONTRACT = :siteId ";  // Parameterized to avoid SQL injection

            try
            {

                OracleConnection oOracleConnection = dbConnection.GetConnection();
                OracleParameter[] param = new OracleParameter[]
                {
            DBConnection.AddParameter("siteId", siteId)
                };

                DataTable dt = (DataTable)dbConnection.Executes(query, DBConnection.ReturnType.DataTable, param, CommandType.Text, oOracleConnection);

                oOracleConnection.Close();

                return dt;

            }
            catch (OracleException ex)
            {
                Console.WriteLine("Oracle error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }

            return resultTable;

        }

        public DataTable GetBudgetBookIdBySiteId(string siteId)
        {
            DataTable resultTable = new DataTable();

            // Define the query with a parameterized placeholder for CONTRACT
            string query = @"SELECT  t.CONTRACT CONTRACT,
			   t.BUDGET_BOOK_ID BUDGET_BOOK_ID,
			   t.DESCRIPTION BB_DESCRIPTION,
			   t.HPNRET_BB_TYPE HPNRET_BB_TYPE
		         FROM   ifsapp.HPNRET_BB_LOV t
                    WHERE t.CONTRACT = :siteId ";  // Parameterized to avoid SQL injection

            try
            {

                OracleConnection oOracleConnection = dbConnection.GetConnection();
                OracleParameter[] param = new OracleParameter[]
                {
            DBConnection.AddParameter("siteId", siteId)
                };

                DataTable dt = (DataTable)dbConnection.Executes(query, DBConnection.ReturnType.DataTable, param, CommandType.Text, oOracleConnection);

                oOracleConnection.Close();

                return dt;



            }
            catch (OracleException ex)
            {
                Console.WriteLine("Oracle error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }

            return resultTable;

        }



        public DataTable GetBudgetPriceListBySiteId(string siteId)
        {
            DataTable resultTable = new DataTable();

            // Define the query with a parameterized placeholder for CONTRACT
            string query = @"select t.Price_list_no price_list_no,
	   t.description pl_description,
	   t.currency_code currency_code,
	   t.valid_to_date valid_to_date
from   ifsapp.sales_price_list_lov2 t
where  ((t.price_list_no in (select price_list_no from ifsapp.sales_price_list_site where contract = :siteId)))";  // Parameterized to avoid SQL injection

            try
            {

                OracleConnection oOracleConnection = dbConnection.GetConnection();
                OracleParameter[] param = new OracleParameter[]
                {
            DBConnection.AddParameter("siteId", siteId)
                };

                DataTable dt = (DataTable)dbConnection.Executes(query, DBConnection.ReturnType.DataTable, param, CommandType.Text, oOracleConnection);

                oOracleConnection.Close();

                return dt;



            }
            catch (OracleException ex)
            {
                Console.WriteLine("Oracle error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }

            return resultTable;

        }


        public DataTable GetExtendedWarrantyPlus()
        {
            DataTable resultTable = new DataTable();

            // Define the query with a parameterized placeholder for CONTRACT
            string query = @"select t.ext_warranty_type ext_warranty_type,
	 t.description ext_war_description,
	 t.applicable_price_db applicable_price,
	 t.charge_type charge_type,
	 t.warrantysales_type warrantysales_type
from   ifsapp.ext_warranty_plus t
where  ((t.warrantysales_type in ('Both', 'Hire') and t.sellable = :sellable))";  // Parameterized to avoid SQL injection

            try
            {
                string sellable = "TRUE";

                OracleConnection oOracleConnection = dbConnection.GetConnection();
                OracleParameter[] param = new OracleParameter[]
                {
            DBConnection.AddParameter("sellable", sellable)
                };

                DataTable dt = (DataTable)dbConnection.Executes(query, DBConnection.ReturnType.DataTable, param, CommandType.Text, oOracleConnection);

                oOracleConnection.Close();

                return dt;



            }
            catch (OracleException ex)
            {
                Console.WriteLine("Oracle error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }

            return resultTable;

        }

        public DataTable GetExtendedWarrantyPlusAmount(string ExtWarPlusType, string partNo, string priceList, double cashPrice, int LOC)
        {
            DataTable resultTable = new DataTable();

            // Define the query with a parameterized placeholder for CONTRACT
            string query = @"SELECT column_value as Ext_Plus_Amount 
                            FROM TABLE(Hire_Sales_Handling_SVC.Rd_Fetch_Warr_Plus
                              (:ExtWarPlusType, 
                               :partNo, 
                               :priceList, 
                               :cashPrice, 
                               :LOC, 
                               unbound## => ''))";  // Parameterized to avoid SQL injection

            try
            {

                OracleConnection oOracleConnection = dbConnection.GetConnection();
                OracleParameter[] param = new OracleParameter[]
                {
            DBConnection.AddParameter("ExtWarPlusType", ExtWarPlusType),
            DBConnection.AddParameter("partNo", partNo),
            DBConnection.AddParameter("priceList", priceList),
            DBConnection.AddParameter("cashPrice", cashPrice),
            DBConnection.AddParameter("LOC", LOC),
                };

                DataTable dt = (DataTable)dbConnection.Executes(query, DBConnection.ReturnType.DataTable, param, CommandType.Text, oOracleConnection);

                oOracleConnection.Close();

                return dt;



            }
            catch (OracleException ex)
            {
                Console.WriteLine("Oracle error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }

            return resultTable;

        }



        public DataTable GetPromotionRuleNo(string partNo, string priceListNo)
        {
            DataTable resultTable = new DataTable();

            // Define the query with a parameterized placeholder for CONTRACT
            string query = @"select t.part_no part_no,
                            t.rule_no rule_no,
                            t.discount_amount discount_amount,
                            t.discount discount,
                            t.price_list_no price_list_no
                            from   ifsapp.promotion_overview t
                            where  t.part_no = :partNo and t.price_list_no = :priceListNo and t.rule_type in ('Discount', 'Trade In') ";  // Parameterized to avoid SQL injection

            try
            {

                OracleConnection oOracleConnection = dbConnection.GetConnection();
                OracleParameter[] param = new OracleParameter[]
                {
            DBConnection.AddParameter("partNo", partNo),
            DBConnection.AddParameter("priceListNo", priceListNo),

                };

                DataTable dt = (DataTable)dbConnection.Executes(query, DBConnection.ReturnType.DataTable, param, CommandType.Text, oOracleConnection);

                oOracleConnection.Close();

                return dt;



            }
            catch (OracleException ex)
            {
                Console.WriteLine("Oracle error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }

            return resultTable;

        }




        public DataTable GetHPCashPrice(string partNo, string priceListNo)
        {
            DataTable resultTable = new DataTable();

            // Define the query with a parameterized placeholder for CONTRACT
            string query = @"select column_value as hp_cash from table(ifsapp.hire_sales_handling_svc.rd_fetch_hp_cash_price(:partNo, :priceListNo, unbound## => '')) ";  // Parameterized to avoid SQL injection

            try
            {

                OracleConnection oOracleConnection = dbConnection.GetConnection();
                OracleParameter[] param = new OracleParameter[]
                {
            DBConnection.AddParameter("partNo", partNo),
            DBConnection.AddParameter("priceListNo", priceListNo),

                };

                DataTable dt = (DataTable)dbConnection.Executes(query, DBConnection.ReturnType.DataTable, param, CommandType.Text, oOracleConnection);

                oOracleConnection.Close();

                return dt;



            }
            catch (OracleException ex)
            {
                Console.WriteLine("Oracle error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }

            return resultTable;

        }

        public DataTable GetDISCCashPrice(double cashPrice, double reqDiscount, double promoDiscount)
        {
            DataTable resultTable = new DataTable();

            // Define the query with a parameterized placeholder for CONTRACT
            string query = @"SELECT 
                     column_value as DISC_CASH 
                     FROM TABLE(
                          Hire_Sales_Handling_SVC.Rd_Fetch_Disc_Cash_Price
                          (:cashPrice, :reqDiscount, :promoDiscount, unbound## => ''))";  // Parameterized to avoid SQL injection

            try
            {

                OracleConnection oOracleConnection = dbConnection.GetConnection();
                OracleParameter[] param = new OracleParameter[]
                {
            DBConnection.AddParameter("cashPrice", cashPrice),
            DBConnection.AddParameter("reqDiscount", reqDiscount),
            DBConnection.AddParameter("promoDiscount", promoDiscount),

                };

                DataTable dt = (DataTable)dbConnection.Executes(query, DBConnection.ReturnType.DataTable, param, CommandType.Text, oOracleConnection);

                oOracleConnection.Close();

                return dt;



            }
            catch (OracleException ex)
            {
                Console.WriteLine("Oracle error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }

            return resultTable;

        }


        public DataTable GetWarInsAmount(string partNo, string priceList, double cashPrice, int LOC)
        {
            DataTable resultTable = new DataTable();

            // Define the query with a parameterized placeholder for CONTRACT
            string query = @"SELECT 
                     EXT_WARRANTY_AMT, 
                     INSURANCE_AMT FROM 
                     TABLE(Hire_Sales_Handling_SVC.Rd_Fetch_Insu_Warr(:partNo, :priceList, :cashPrice, :LOC, unbound## => ''))";  // Parameterized to avoid SQL injection

            try
            {

                OracleConnection oOracleConnection = dbConnection.GetConnection();
                OracleParameter[] param = new OracleParameter[]
                {
            DBConnection.AddParameter("partNo", partNo),
            DBConnection.AddParameter("priceList", priceList),
            DBConnection.AddParameter("cashPrice", cashPrice),
            DBConnection.AddParameter("LOC", LOC),

                };

                DataTable dt = (DataTable)dbConnection.Executes(query, DBConnection.ReturnType.DataTable, param, CommandType.Text, oOracleConnection);

                oOracleConnection.Close();

                return dt;



            }
            catch (OracleException ex)
            {
                Console.WriteLine("Oracle error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }

            return resultTable;

        }


        public DataTable GetServiceCharge_PriceList(string budgetBookId)
        {
            DataTable resultTable = new DataTable();

            // Define the query with a parameterized placeholder for CONTRACT
            string query = @"SELECT ifsapp.Hpnret_Bb_Main_Head_API.Get_Price_List_No(:budgetBookId) Price_List,
       ifsapp.Hpnret_Bb_Main_Head_API.Get_Serv_Charge_In(:budgetBookId) Service_Charge
from dual";  // Parameterized to avoid SQL injection

            try
            {

                OracleConnection oOracleConnection = dbConnection.GetConnection();
                OracleParameter[] param = new OracleParameter[]
                {
            DBConnection.AddParameter("budgetBookId", budgetBookId),

                };

                DataTable dt = (DataTable)dbConnection.Executes(query, DBConnection.ReturnType.DataTable, param, CommandType.Text, oOracleConnection);

                oOracleConnection.Close();

                return dt;



            }
            catch (OracleException ex)
            {
                Console.WriteLine("Oracle error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }

            return resultTable;

        }


        public DataTable GetDebitNotePart(string DebitNote, string Debit_site)
        {
            DataTable resultTable = new DataTable();

            // get the product code details
            string query = @"select n.debit_note_no  , n.order_no , j.contract , j.part_no , (j.qty -  nvl((SELECT nvl(SUM(h.scan_qty), 0) qty
       FROM ifsapp.sin_bar_bulk_re_transit_head h
      WHERE  h.status IN ('Received','Planned')
        AND h.part_no = j.part_no
        AND h.debit_note_no = n.debit_note_no),0)) QTY
from ifsapp.Inv_Transit_Tracking j ,
     ifsapp.Serial_Trans_History n
     where j.contract = n.customer_code
     and j.delivering_contract = n.contract
     and j.part_no = n.part_no
     and j.order_no = n.order_no
     and n.debit_note_no = :DebitNote
     and j.contract = :Debit_site
     and j.line_no = n.line_no
  group by n.debit_note_no  , n.order_no , j.contract , j.part_no ,j.qty";

            try
            {
                using (OracleConnection oOracleConnection = dbConnection.GetConnection())
                {
                    using (OracleCommand command = new OracleCommand(query, oOracleConnection))
                    {
                        command.BindByName = true;
                        command.Parameters.Add(new OracleParameter(":DebitNote", OracleDbType.Varchar2)).Value = DebitNote;
                        command.Parameters.Add(new OracleParameter(":Debit_site", OracleDbType.Varchar2)).Value = Debit_site;

                        using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                        {
                            adapter.Fill(resultTable);
                        }
                    }
                }

                return resultTable;
            }
            catch (OracleException ex)
            {
                Console.WriteLine("Oracle error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw; // Re-throw the exception after logging
            }

            return resultTable;

        }

    }

}
