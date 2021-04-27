using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;

namespace GetVaccine
{
    class Program
    {
        static void Main(string[] args)
        {
            DeleteData();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string url = "https://health-infobase.canada.ca/src/data/covidLive/vaccination-coverage-map.csv";

            string vaccineCsvPath = @"D:\Github2020\covid19-api\Data\vaccine.csv";
            System.Net.WebClient client = new WebClient();
            client.DownloadFile(url, vaccineCsvPath);

            using (var reader = new StreamReader(vaccineCsvPath))
            {

                Vaccine eachCase = new Vaccine();
                int i = -1;
                while (!reader.EndOfStream)
                {
                    i++;
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (i != 0)
                    {
                        eachCase.ControlID = Guid.NewGuid();
                        eachCase.Week_End = Convert.ToDateTime(values[0]);
                        eachCase.PrUid = Convert.ToInt32(values[1]);
                        eachCase.PrName = values[2];
                        if (values[4] == "") { eachCase.numtotal_atleast1dose = 0; } else { eachCase.numtotal_atleast1dose = Convert.ToInt32(values[4]); }
                        if (values[5] == "" || values[5].ToLower() == "na") { eachCase.numtotal_1dose = 0; } else { eachCase.numtotal_1dose = Convert.ToInt32(values[5]); }
                        if (values[6] == "" || values[6].ToLower() == "na") { eachCase.numtotal_2doses = 0; } else { eachCase.numtotal_2doses = Convert.ToInt32(values[6]); }
                        if (values[7] == "" || values[7].ToLower() == "na") { eachCase.proptotal_atleast1dose = 0.0; } else { eachCase.proptotal_atleast1dose = Convert.ToDouble(values[7]); }
                        if (values[8] == "" || values[8].ToLower() == "na") { eachCase.proptotal_1dose = 0.0; } else { eachCase.proptotal_1dose = Convert.ToDouble(values[8]); }
                        if (values[9] == "" || values[9].ToLower() == "na") { eachCase.proptotal_2doses = 0.0; } else { eachCase.proptotal_2doses = Convert.ToDouble(values[9]); }

                        UploadData(eachCase);

                    }

                }
            }

        }

        static void UploadData(Vaccine eachCase)
        {
            // upload SQL
            string ConString = "pleae update";
            using (SqlConnection cn = new SqlConnection(ConString))
            {
                cn.Open();
                try
                {
                    StringBuilder q = new StringBuilder();
                    q.Append("INSERT INTO Vaccine ([ControlID], [Week_End], [PrUid], [PrName], [numtotal_atleast1dose], [numtotal_1dose], [numtotal_2doses], [proptotal_atleast1dose], [proptotal_1dose], [proptotal_2doses]) VALUES(");
                    q.Append("'" + eachCase.ControlID + "',");
                    q.Append("'" + eachCase.Week_End + "',");
                    q.Append(eachCase.PrUid + ",");
                    q.Append("'" + eachCase.PrName + "',");
                    q.Append(eachCase.numtotal_atleast1dose + ",");
                    q.Append(eachCase.numtotal_1dose + ",");
                    q.Append(eachCase.numtotal_2doses + ",");
                    q.Append(eachCase.proptotal_atleast1dose + ",");
                    q.Append(eachCase.proptotal_1dose + ",");
                    q.Append(eachCase.proptotal_2doses);
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
            string ConString = "pleae update";
            using (SqlConnection cn = new SqlConnection(ConString))
            {
                cn.Open();
                try
                {
                    StringBuilder q = new StringBuilder();
                    q.Append("DELETE FROM [COVID].[dbo].[Vaccine]");
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
