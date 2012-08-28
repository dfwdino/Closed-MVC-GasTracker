﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace mvc_MilesTracker.Models
{
    public abstract class ASQLFunctions
    {

        public IList<Models.Auto> GetAllAutos()
        {

            SqlConnection myConnection;
            SqlDataAdapter myCommand;

            DataSet ds = new DataSet();

            //var currentSession = HttpContext.Current.Session;
            //string myValue = currentSession["SQLInfo"].ToString();
            //myConnection = new SqlConnection(myValue);
            myConnection = new SqlConnection(@"server=MASOCHIST\MASOCHISTSQL;database=Gas;User Id=Bills;Password=Bills");

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

            return AutoList;
        }

        public IList<Models.Gas> GetMilesForAuto(string AutoID)
        {

            SqlConnection myConnection;
            SqlDataAdapter myCommand;

            DataSet ds = new DataSet();

            //var currentSession = HttpContext.Current.Session;
            //string myValue = currentSession["SQLInfo"].ToString();
            //myConnection = new SqlConnection(myValue);
            myConnection = new SqlConnection(@"server=MASOCHIST\MASOCHISTSQL;database=Gas;User Id=Bills;Password=Bills");

            myCommand = new SqlDataAdapter("SELECT Gas.Miles,Gas.Gallons,Gas.Price,Auto.AutoName " + 
                                            "FROM [Gas].[dbo].[Gas] " + 
                                            "inner join Auto " +
                                            "on Auto.AutoNumber = Gas.AutoNumber where'" + AutoID + "' order by Miles", myConnection);

            ds = new DataSet();

            myCommand.Fill(ds);

            IList<Models.Gas> GasList = new List<Models.Gas>();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                GasList.Add(new Gas
                {
                    AutoName = item["AutoName"].ToString(),
                    Miles = (int)item["Miles"],
                    Gallons = (float)item["Gallons"],
                    Price = (double)item["Price"]
                });
            }

            return GasList;
        }

    }
}