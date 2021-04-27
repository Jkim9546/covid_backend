using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;

namespace GetCanada
{
    class Program
    {
        static void Main(string[] args)
        {
            DeleteData();
            string url = "https://health-infobase.canada.ca/src/data/covidLive/covid19.csv";

            string canadaCsvPath = @"D:\Github2020\covid19-api\Data\canada.csv";
            System.Net.WebClient client = new WebClient();
            client.DownloadFile(url, canadaCsvPath);

            using (var reader = new StreamReader(canadaCsvPath))
            {
                //  0 -  4 : pruid, prname, prnameFR, date, update,
                //  5 -  9 : numconf, numprob, numdeaths, numtotal, numtested,
                // 10 - 14 : numtests, numrecover, percentrecover, ratetested, ratetests,
                // 15 - 19 : numtoday, percentoday, ratetotal, ratedeaths, numdeathstoday,
                // 20 - 24 : percentdeath, numtestedtoday, numteststoday, numrecoveredtoday, percentactive,
                // 25 - 29 : numactive, rateactive, numtotal_last14, ratetotal_last14,
                // 30 - 34 : numdeaths_last14, ratedeaths_last14, numtotal_last7, ratetotal_last7, numdeaths_last7,
                // 35 - 39 : ratedeaths_last7, avgtotal_last7, avgincidence_last7, avgdeaths_last7, avgratedeaths_last7

                Canada eachCase = new Canada();
                int i = -1;
                while (!reader.EndOfStream)
                {
                    i++;
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (i != 0)
                    {
                        eachCase.ControlID = Guid.NewGuid();
                        eachCase.PrUid = Convert.ToInt32(values[0]);
                        eachCase.PrName = values[1];
                        eachCase.Date = DateTime.Parse(values[3], CultureInfo.CreateSpecificCulture("fr-FR")); // Convert.ToDateTime(values[3]);   
                        eachCase.NumConfirmed = Convert.ToInt32(values[5]);
                        eachCase.NumProbable = Convert.ToInt32(values[6]);
                        if (values[7] == "") { eachCase.NumDeaths = 0; } else { eachCase.NumDeaths = Convert.ToInt32(values[7]); }
                        eachCase.NumTotal = Convert.ToInt32(values[8]);
                        eachCase.NumToday = Convert.ToInt32(values[15]);
                        if (values[25] == "") { eachCase.Activated = 0; } else { eachCase.Activated = Convert.ToInt32(values[25]); }
                        if (values[9] == "") { eachCase.NumTested = 0; } else { eachCase.NumTested = Convert.ToInt32(values[9]); }
                        if (values[11] == "") { eachCase.NumRecover = 0; } else { eachCase.NumRecover = Convert.ToInt32(values[11]); }

                        UploadData(eachCase);

                    }

                }
            }

        }

        static void UploadData(Canada eachCase)
        {
            // upload SQL
            string ConString = "please upate";
            using (SqlConnection cn = new SqlConnection(ConString))
            {
                cn.Open();
                try
                {
                    StringBuilder q = new StringBuilder();
                    q.Append("INSERT INTO [Canada]([ControlID],[PrUid],[PrName],[Date],[NumConfirmed],[NumProbable],[NumDeaths],[NumTotal],[NumToday],[Activated],[NumTested],[NumRecover]) values (");
                    q.Append("'" + eachCase.ControlID + "',");
                    q.Append(eachCase.PrUid + ",");
                    q.Append("'" + eachCase.PrName + "',");
                    q.Append("'" + eachCase.Date + "',");
                    q.Append(eachCase.NumConfirmed + ",");
                    q.Append(eachCase.NumProbable + ",");
                    q.Append(eachCase.NumDeaths + ",");
                    q.Append(eachCase.NumTotal + ",");
                    q.Append(eachCase.NumToday + ",");
                    q.Append(eachCase.Activated + ",");
                    q.Append(eachCase.NumTested + ",");
                    q.Append(eachCase.NumRecover);
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
            string ConString = "please upate";
            using (SqlConnection cn = new SqlConnection(ConString))
            {
                cn.Open();
                try
                {
                    StringBuilder q = new StringBuilder();
                    q.Append("DELETE FROM [COVID].[dbo].[Canada]");
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
