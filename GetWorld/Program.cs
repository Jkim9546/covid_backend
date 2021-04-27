using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Data.SqlClient;

namespace GetWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            DeleteData();
            string url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
            string worldPath = @"D:\Github2020\covid19-api\Data\world_confirm.csv";
            System.Net.WebClient client = new WebClient();
            client.DownloadFile(url, worldPath);
            ReadData(worldPath, "Confirmed");

            url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_recovered_global.csv";
            worldPath = @"D:\Github2020\covid19-api\Data\world_recovered.csv";
            client = new WebClient();
            client.DownloadFile(url, worldPath);
            ReadData(worldPath, "Recovered");

            url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_deaths_global.csv";
            worldPath = @"D:\Github2020\covid19-api\Data\world_deaths.csv";
            client = new WebClient();
            client.DownloadFile(url, worldPath);
            ReadData(worldPath, "Deaths");
        }


        static void ReadData(string path, string dataType)
        {
            using (var reader = new StreamReader(path))
            {
                int country = -1;
                DateTime reportDate = Convert.ToDateTime("2020/1/18");
                while (!reader.EndOfStream)
                {
                    country++;
                    var line = reader.ReadLine();

                    if (country != 0)
                    {
                        line = line.ToString().Replace("\"Korea, South\"", "Korea South");
                        line = line.ToString().Replace("\"Saint Helena, Ascension and Tristan da Cunha\"", "Saint Helena Ascension and Tristan da Cunha");
                        line = line.ToString().Replace("\"Bonaire, Sint Eustatius and Saba\"", "Bonaire Sint Eustatius and Saba");
                        line = line.ToString().Replace("Cote d'Ivoire", "Cote d Ivoire");

                        var values = line.Split(',');
                        //Console.WriteLine(values[1]);
                        for (int dateNumber = 4; dateNumber < values.Length; dateNumber++)
                        {

                            World eachCase = new World();
                            eachCase.ControlID = Guid.NewGuid();
                            eachCase.Province_State = values[0];
                            eachCase.Country_Region = values[1];
                            eachCase.ReportedDate = reportDate.AddDays(dateNumber);
                            eachCase.DataType = dataType;
                            eachCase.DataValue = Convert.ToInt32(values[dateNumber]);

                            UploadData(eachCase);
                        }

                    }
                }
            }
        }

        static void UploadData(World eachCase)
        {
            // upload SQL
            string ConString = "please update";
            using (SqlConnection cn = new SqlConnection(ConString))
            {
                cn.Open();
                try
                {
                    StringBuilder q = new StringBuilder();
                    q.Append("INSERT INTO [World]([ControlID],[Province_State],[Country_Region],[ReportedDate],[DataType],[DataValue]) values (");
                    q.Append("'" + eachCase.ControlID + "',");
                    q.Append("'" + eachCase.Province_State + "',");
                    q.Append("'" + eachCase.Country_Region + "',");
                    q.Append("'" + eachCase.ReportedDate + "',");
                    q.Append("'" + eachCase.DataType + "',");
                    q.Append(eachCase.DataValue);
                    q.Append(")");
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
                    q.Append("DELETE FROM [COVID].[dbo].[World]");
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
