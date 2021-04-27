using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Data.SqlClient;

namespace GetOntario
{
    class Program
    {
        static void Main(string[] args)
        {
            DeleteData();
            //string url = "https://data.ontario.ca/dataset/f4112442-bdc8-45d2-be3c-12efae72fb27/resource/455fd63b-603d-4608-8216-7d8647f43350/download/conposcovidloc.csv";
            string url = "https://data.ontario.ca/dataset/1115d5fe-dd84-4c69-b5ed-05bf0c0a0ff9/resource/d1bfe1ad-6575-4352-8302-09ca81f7ddfc/download/cases_by_status_and_phu.csv";
            string ontarioCsvPath = @"D:\Github2020\covid19-api\Data\ontario.csv";
            System.Net.WebClient client = new WebClient();
            client.DownloadFile(url, ontarioCsvPath);

            using (var reader = new StreamReader(ontarioCsvPath))
            {
                //  0 -  4 : Row_ID, Accurate_Episode_Date, Case_Reported_Date, Test_Reported_Date, Specimen_Date,
                //  5 -  9 : Age_Group, Client_Gender, Case_AcquisitionInfo, Outcome1, Outbreak_Related,
                // 10 - 14 : Reporting_PHU_ID, Reporting_PHU, Reporting_PHU_Address, Reporting_PHU_City, Reporting_PHU_Postal_Code,
                // 15 - 17 : Reporting_PHU_Website, Reporting_PHU_Latitude, Reporting_PHU_Longitude
                
                
                int i = -1;
                while (!reader.EndOfStream)
                {
                    i++;
                    var line = reader.ReadLine();
                    line = line.ToString().Replace("\"HALIBURTON, KAWARTHA, PINE RIDGE\"", "HALIBURTON KAWARTHA PINE RIDGE");
                    line = line.ToString().Replace("\"KINGSTON, FRONTENAC, LENNOX & ADDINGTON\"", "KINGSTON FRONTENAC LENNOX AND ADDINGTON");
                    line = line.ToString().Replace("\"LEEDS, GRENVILLE AND LANARK DISTRICT\"", "LEEDS GRENVILLE AND LANARK DISTRICT");

                    var values = line.Split(',');

                    if (i != 0)
                    {
                        Ontario eachCase = new Ontario();
                        eachCase.ControlID = Guid.NewGuid();
                        eachCase.ReportedDate = Convert.ToDateTime(values[0]);

                        try { eachCase.PHU_NAME = values[1]; } catch { eachCase.PHU_NAME = "unidentified"; }
                        try { eachCase.PHU_NUM = Convert.ToInt32(values[2]); } catch { eachCase.PHU_NUM = 0; }
                        
                        eachCase.ACTIVE_CASES = Convert.ToInt32(values[3]);
                        eachCase.RESOLVED_CASES = Convert.ToInt32(values[4]);
                        eachCase.DEATHS = Convert.ToInt32(values[5]);

                        UploadData(eachCase);

                    }

                }
               
            }

        }

        static void UploadData(Ontario eachCase)
        {
            // upload SQL
            string ConString = "please update";
            using (SqlConnection cn = new SqlConnection(ConString))
            {
                cn.Open();
                try
                {
                    StringBuilder q = new StringBuilder();
                    q.Append("INSERT INTO [Ontario]( [ControlID] ,[ReportedDate] ,[PHU_NAME] ,[PHU_NUM] ,[ACTIVE_CASES] ,[RESOLVED_CASES] ,[DEATHS] ) VALUES (");
                    q.Append("'" + eachCase.ControlID + "',");
                    q.Append("'" + eachCase.ReportedDate + "',");
                    q.Append("'" + eachCase.PHU_NAME + "',");
                    q.Append(eachCase.PHU_NUM + ",");
                    q.Append(eachCase.ACTIVE_CASES + ",");
                    q.Append(eachCase.RESOLVED_CASES + ",");
                    q.Append(eachCase.DEATHS + ")");
                    string query = q.ToString();
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Inner Exception: " + ex.Message);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


        }

        static void DeleteData()
        {
            // upload SQL
            string ConString = "please update";
            using (SqlConnection cn = new SqlConnection(ConString))
            {
                cn.Open();
                try
                {
                    StringBuilder q = new StringBuilder();
                    q.Append("DELETE FROM [COVID].[dbo].[Ontario]");
                    string query = q.ToString();
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Inner Exception: " + ex.Message);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

    }
}
