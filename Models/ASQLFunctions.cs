using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;


namespace mvc_MilesTracker.Models
{
    public abstract class ASQLFunctions : IDisposable
    {
        private SqlConnection myConnection;
        private SqlDataAdapter myCommand;

        public ASQLFunctions()
        {
            myConnection = new SqlConnection(Properties.Settings.Default.SQLServer);
        }


        public IList<Models.Auto> GetAllAutos()
        {
            DataSet ds = new DataSet();

            myCommand = new SqlDataAdapter("Select * from Auto", myConnection);

            ds = new DataSet();

            myCommand.Fill(ds);

            IList<Models.Auto> AutoList = new List<Models.Auto>();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                AutoList.Add(new Auto
                {
                    AutoNumber = item["AutoNumber"].ToString(),
                    AutoName = item["AutoName"].ToString()
                });
            }

            ds.Dispose();
            ds = null;

            return AutoList;
        }

        public IList<Models.Gas> GetMilesForAuto(string AutoID)
        {
            
            DataSet ds = new DataSet();

            myCommand = new SqlDataAdapter("SELECT Gas.Miles,Gas.Gallons,Gas.Price,Auto.AutoName " + 
                                            "FROM [Gas].[dbo].[Gas] " + 
                                            "inner join Auto " +
                                            "on Auto.AutoNumber = Gas.AutoNumber where Gas.AutoNumber = '" + AutoID + "' order by Gas.Miles", myConnection);

            ds = new DataSet();

            myCommand.Fill(ds);

            IList<Models.Gas> GasList = new List<Models.Gas>();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                GasList.Add(new Gas
                {
                    AutoName = item["AutoName"].ToString(),
                    Miles = (int)item["Miles"],
                    Gallons = (double)item["Gallons"],
                    Price = (decimal)item["Price"]
                });
            }

            ds.Dispose();
            ds = null;
            
            return GasList;
        }

        public void AddAuto(string AutoName)
        {
            SqlCommand myCommand;

            string SQLInsertNewAuto = "INSERT INTO Auto ([AutoName]) VALUES ('{0}')";

            string insertbill = string.Format(SQLInsertNewAuto, AutoName);

            myCommand = new SqlCommand(insertbill, myConnection);

            myConnection.Open();

            myCommand.ExecuteNonQuery();

        }


        public void AddGas(string AutoNumber, string Miles, string Price, string Gallons)
        {
            SqlCommand myCommand;

            if (Price.Length.Equals(0))
                Price = "0.0";
            

            string SQLInsertNewGas = "INSERT INTO Gas ([AutoNumber],[Miles],[Price],[Gallons]) VALUES ('{0}','{1}',{2},{3})";

            string insertbill = string.Format(SQLInsertNewGas, AutoNumber,Miles,Price,Gallons);
                       
            myCommand = new SqlCommand(insertbill, myConnection);

            myConnection.Open();

            myCommand.ExecuteNonQuery();

        }


        public void Dispose()
        {
            myConnection.Close();
            
            myCommand.Dispose();
            myConnection.Dispose();
        }

      
    }
}