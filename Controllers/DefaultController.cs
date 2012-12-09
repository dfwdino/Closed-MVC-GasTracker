using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grocery_List.Controllers
{
    public class DefaultController : Controller
    {
        static System.Data.SqlClient.SqlConnection conn;

        public DefaultController()
        {
            if (conn == null)
            {
                conn = new System.Data.SqlClient.SqlConnection();
                conn.ConnectionString = "integrated security=SSPI;data source=MFCOLL123X25\\SQLSNEWSOM;persist security info=False;initial catalog=Grocery";
                conn.Open();
            }
        }
        public void CleanUp()
        {
            conn.Close();
            conn.Dispose();
        }

        public ActionResult Index()
        {
            string SQLGetItems = "select GotItem, IndexNumber, Item, Store, CONVERT(money, Price) as Price from List {0} order by Item";


            if (Request.QueryString.AllKeys.Contains("ID") && Request.QueryString.AllKeys.Contains("GotItem"))
            {
                SqlCommand myCommand = new SqlCommand("update List set GotItem = " + Request.QueryString["GotItem"] + " where IndexNumber = '" + Request.QueryString["ID"] + "'", conn);

                int Updated = myCommand.ExecuteNonQuery();
                
            }
            
            if (Request.QueryString.AllKeys.Contains("View"))
                SQLGetItems = string.Format(SQLGetItems, "");
            else
                SQLGetItems = string.Format(SQLGetItems, "where Gotitem=0");
            
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(SQLGetItems, conn);

                da.Fill(dt);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            
        return View(dt);
        }
        public ActionResult AddItem()
        {
           
            if (!Request.Form.Keys.Count.Equals(0))
            {
                string SQLAddItem = "INSERT INTO [Grocery].[dbo].[List] ([Item],[Store],[Price]) VALUES ('{0}','{1}','{2}')";

                SqlCommand myCommand = new SqlCommand(string.Format(SQLAddItem, Request.Form["Item"], Request.Form["Store"], Request.Form["Price"]), conn);

                int Updated = myCommand.ExecuteNonQuery();
            

                return View();

            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from Store order by Name", conn);

                DataTable dt = new DataTable();

                da.Fill(dt);

                return View(dt);
            }
                
        }
        
        public ActionResult AddStore()
        {
           

            if (!Request.Form.Keys.Count.Equals(0))
            {
                string SQLAddStore = "INSERT INTO [Grocery].[dbo].[Store] ([Name]) VALUES ('{0}')";

                SqlCommand myCommand = new SqlCommand(string.Format(SQLAddStore, Request.Form["Name"]), conn);

                int Updated = myCommand.ExecuteNonQuery();

            }

            return View();
            
        }

        public ActionResult DisplayStore()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from Store order by Name", conn);

                da.Fill(dt);

            }
            catch (Exception ex)
            {

            }
            return View(dt);
        }

        public ActionResult DeleteStore()
        {
            if (Request.QueryString.AllKeys.Contains("ID"))
            {
                SqlCommand myCommand = new SqlCommand("Delete store where IndexNumber = '" + Request.QueryString["ID"] + "'", conn);

                int Updated = myCommand.ExecuteNonQuery();

            }


            DisplayStore();
            return View("DisplayStore");
        }

        public ActionResult DeleteItem()
        {

            if (Request.QueryString.AllKeys.Contains("ID"))
            {
                SqlCommand myCommand = new SqlCommand("Delete List where IndexNumber = '" + Request.QueryString["ID"] + "'", conn);

                int Updated = myCommand.ExecuteNonQuery();

            }
            
            Index();
            return View("Index");
        }


    }
}
