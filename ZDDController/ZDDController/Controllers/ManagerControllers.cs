using System;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ZDDController.Controllers
{
	public class ManagerControllers
	{
        string connectionString = "Server=localhost;Database=testingdb;Uid=root;Pwd=m123;";

        public bool makePart(String pNum, float rate, float ppm, float ttfRate, bool bwi, bool oring, bool special)
		{
            Console.WriteLine("in makePart");
            bool status = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("connected");

                string insertQuery = "INSERT INTO parts (pNum, rate, expectedPPM, ttfRate, bwi, oring, special) " +
                    "VALUES (@pNum, @rate, @ppm, @ttfRate, @bwi, @oring, @special)";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        Console.WriteLine("adding arguments");
                        command.Parameters.AddWithValue("@pNum", pNum);
                        command.Parameters.AddWithValue("@rate", rate);
                        command.Parameters.AddWithValue("@ppm", ppm);
                        command.Parameters.AddWithValue("@ttfRate", ttfRate);
                        command.Parameters.AddWithValue("@bwi", bwi);
                        command.Parameters.AddWithValue("@oring", oring);
                        command.Parameters.AddWithValue("@special", special);

                        Console.WriteLine("executing");

                        command.ExecuteNonQuery();
                        Console.WriteLine("command executed");
                    }
                    status = true;
                    Console.WriteLine("closing connection");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return status;
		}

        //------------------------------------------------

        public bool makeEmployee(String name, int badge)
        {
            Console.WriteLine("in make employee");
            bool status = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("connected");

                string insertQuery = "INSERT INTO employees (eName, badgeNum) VALUES (@name, @badge)";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        Console.WriteLine("adding arguments");
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@badge", badge);
                        Console.WriteLine("executing");

                        //freezes here
                        command.ExecuteNonQuery();
                        Console.WriteLine("command executed");
                    }
                    status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return status;
        }

        //------------------------------------------------

        public int getLastEID()
        {
            int id = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                String query = "select eID from employees order by eID desc limit 1";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id = reader.GetInt32(0);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return id;
        }

        //------------------------------------------------

        public bool editPart(String pNum, float ppm, float rate, float ttfRate)
        {
            //TODO make a version of this method with rate or float or ppm being null
            bool status = false;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string alterQuery = "update parts set rate = @rate, expectedPPM = @ppm, ttfRate = @ttfRate where pNum = @pNum";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(alterQuery, connection))
                    {
                        command.Parameters.AddWithValue("@pNum", pNum);
                        command.Parameters.AddWithValue("@rate", rate);
                        command.Parameters.AddWithValue("@ppm", ppm);
                        command.Parameters.AddWithValue("@ttfRate", ttfRate);

                        command.ExecuteNonQuery();
                    }
                    status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return status;
        }

        //------------------------------------------------

        public bool addNormalInventroy(int fo, string pNum, int qty, System.DateTime date, string bNum)
        {
            bool status = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "insert into sorts" +
                    "(FO, pID, TTF, TTFDateRecieved, quantity, dateReceived, dateFinished, finished, leadTime, comments, PPM, sortTime, checkManager, batchNum) values" +
                    "(@fo, (select pID from parts where pNum = @pNum), false, null, @qty, @date, null, false, null, null, null, null, null, @bNum)";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@fo", fo);
                        command.Parameters.AddWithValue("@pNum", pNum);
                        command.Parameters.AddWithValue("@qty", qty);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@bNum", bNum);

                        command.ExecuteNonQuery();
                    }
                    status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return status;
        }

        //------------------------------------------------

        public bool addTTFInventory(int fo, string pNum, int qty, System.DateTime date, string bNum, System.DateTime ttfDate)
        {
            bool status = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                //string insertQuery = "insert into sorts" +
                //    "(FO, pID, TTF, TTFDateRecieved, quantity, dateReceived, dateFinished, finished, leadTime, comments, PPM, sortTime, checkManager, batchNum) values" +
                //    "(@fo, (select pID from parts where pNum = @pNum), false, null, @qty, @date, null, false, null, null, null, null, null, @bNum)";

                string ttfEditQuery = "update sorts set TTF = true, TTFDateRecieved = @ttfDate " +
                    "where sID in (select sID from(select sID from sorts where " +
                    "FO = @fo " +
                    "and pID = (select pID from parts where pNum = @pNum) " +
                    "and quantity = @qty " +
                    "and dateReceived = @date " +
                    "and batchNum = @bNum) " +
                    "as subquery)";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(ttfEditQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ttfDate", ttfDate);
                        command.Parameters.AddWithValue("@fo", fo);
                        command.Parameters.AddWithValue("@pNum", pNum);
                        command.Parameters.AddWithValue("@qty", qty);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@bNum", bNum);

                        command.ExecuteNonQuery();
                    }
                    status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return status;
        }

        //------------------------------------------------

        public bool passSort(int eid, int fo)
        {
            bool status = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string passQuery = "update sorts set checkManager = @eid where FO = @fo";

                    using (MySqlCommand command = new MySqlCommand(passQuery, connection))
                    {
                        command.Parameters.AddWithValue("@eid", eid);
                        command.Parameters.AddWithValue("@fo", fo);

                        command.ExecuteNonQuery();
                    }
                    status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return status;
        }

        //------------------------------------------------

        public bool failSort(int fo)
        {
            bool status = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string failQuery = "update sorts set finished = false, dateFinished = null, comments = null where FO = @fo";

                    using (MySqlCommand command = new MySqlCommand(failQuery, connection))
                    {
                        command.Parameters.AddWithValue("@fo", fo);

                        command.ExecuteNonQuery();
                    }
                    status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return status;
        }

        //------------------------------------------------

        public bool editEmployee(String eName, int eid, int bNum)
        {
            bool status = false;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string alterQuery = "update employees set badgeNum = @bNum where eName = @eName and eID = @eid";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(alterQuery, connection))
                    {
                        command.Parameters.AddWithValue("@bNum", bNum);
                        command.Parameters.AddWithValue("@eName", eName);
                        command.Parameters.AddWithValue("@eid", eid);

                        command.ExecuteNonQuery();
                    }
                    status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return status;
        }

        //------------------------------------------------

        public bool generatePayrollReport(DateTime beginDate, DateTime endDate)
        {
            bool status = false;
            List<DataClasses.Billing> billingInfo = new List<DataClasses.Billing>();
            List<DataClasses.Billing> ttfBillingInfo = new List<DataClasses.Billing>();
            List<DataClasses.Payroll> payrollInfo = new List<DataClasses.Payroll>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {

                    //get the information for employee work and store it in a list
                    string payrollQuery = "select eID, eName, startTime, endTime, " +
                    "TIMEDIFF(endTime, startTime) as time from work natural join employees where " +
                    "startTime between @beginDate and @endDate and " +
                    "endTime is not null " +
                    "order by eID";

                    using (MySqlCommand payrollCommand = new MySqlCommand(payrollQuery, connection, transaction))
                    {
                        payrollCommand.Parameters.AddWithValue("@beginDate", beginDate);
                        payrollCommand.Parameters.AddWithValue("@endDate", endDate);
                        using (MySqlDataReader reader = payrollCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataClasses.Payroll info = new DataClasses.Payroll
                                {
                                    eID = Convert.ToInt32(reader["eID"]),
                                    eName = reader["eName"].ToString(),
                                    startTime = Convert.ToDateTime(reader["startTime"]),
                                    endTime = Convert.ToDateTime(reader["endTime"]),
                                    time = TimeSpan.Parse(reader["time"].ToString())
                                };
                                payrollInfo.Add(info);
                            }
                        }
                    }

                    //get the information for standard sorts and store it in a list
                    string billingQuery = "select sID, pNum, rate, TTF, ttfRate, quantity from " +
                        "sorts natural join parts where " +
                        "finished = true and " +
                        "checkManager is not null and " +
                        "dateFinished between @beginDate and @endDate and " +
                        "TTF = 0 " +
                        "order by pNum";

                    using (MySqlCommand billingCommand = new MySqlCommand(billingQuery, connection, transaction))
                    {
                        billingCommand.Parameters.AddWithValue("@beginDate", beginDate);
                        billingCommand.Parameters.AddWithValue("@endDate", endDate);
                        using (MySqlDataReader reader = billingCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataClasses.Billing info = new DataClasses.Billing
                                {
                                    sID = Convert.ToInt32(reader["sID"]),
                                    pNum = Convert.ToString(reader["pNum"]),
                                    rate = Convert.ToDecimal(reader["rate"]),
                                    ttf = Convert.ToInt32(reader["TTF"]),
                                    ttfRate = Convert.ToDecimal(reader["ttfRate"]),
                                    qty = Convert.ToInt32(reader["quantity"])
                                };
                                billingInfo.Add(info);
                            }
                        }
                    }

                    //get the information for ttf sorts and store it in a list
                    string ttfBillingQuery = "select sID, pNum, rate, TTF, ttfRate, quantity from " +
                        "sorts natural join parts where " +
                        "finished = true and " +
                        "checkManager is not null and " +
                        "dateFinished between @beginDate and @endDate and " +
                        "TTF = 1 " +
                        "order by pNum";

                    using (MySqlCommand ttfBillingCommand = new MySqlCommand(ttfBillingQuery, connection, transaction))
                    {
                        ttfBillingCommand.Parameters.AddWithValue("@beginDate", beginDate);
                        ttfBillingCommand.Parameters.AddWithValue("@endDate", endDate);
                        using (MySqlDataReader reader = ttfBillingCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataClasses.Billing info = new DataClasses.Billing
                                {
                                    sID = Convert.ToInt32(reader["sID"]),
                                    pNum = Convert.ToString(reader["pNum"]),
                                    rate = Convert.ToDecimal(reader["rate"]),
                                    ttf = Convert.ToInt32(reader["TTF"]),
                                    ttfRate = Convert.ToDecimal(reader["ttfRate"]),
                                    qty = Convert.ToInt32(reader["quantity"])
                                };
                                ttfBillingInfo.Add(info);
                            }
                        }
                    }
                    transaction.Commit();

                    //payroll report
                    //set up initial varaibles
                    string payrollReportString = "Employee ID, Name, Time Worked \n";
                    if (payrollInfo.Count > 0)
                    {
                        int lastEID = payrollInfo[0].eID;
                        string lastName = payrollInfo[0].eName;
                        TimeSpan runningTotal = TimeSpan.Zero;

                        foreach (DataClasses.Payroll row in payrollInfo)
                        {
                            if (row.eID == lastEID)
                            {
                                runningTotal += row.time;
                            }
                            else
                            {
                                payrollReportString += lastEID + ", " + lastName + ", " + runningTotal + "\n";
                                runningTotal = row.time;
                                lastEID = row.eID;
                                lastName = row.eName;
                            }
                        }
                        //get the final row information
                        payrollReportString += payrollInfo[payrollInfo.Count - 1].eID + ", " +
                            payrollInfo[payrollInfo.Count - 1].eName + ", " +
                            runningTotal + "\n";
                    }
                    string payrollDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(payrollDocPath, "payroll.csv")))
                    {
                        outputFile.WriteLine(payrollReportString);
                    }

                    //billing report
                    //set up initial string for normal sorts
                    string billingReportString = "Part Number, Quantity, Rate \n";
                    
                    //get the string for the standard sorts file
                    if (billingInfo.Count > 0)
                    {
                        //initialized necessary variables
                        string lastPNum = billingInfo[0].pNum;
                        decimal rate = billingInfo[0].rate;
                        int runningQty = 0;

                        foreach (DataClasses.Billing row in billingInfo)
                        {
                            if (lastPNum == row.pNum)
                            {
                                runningQty += row.qty;
                            }
                            else
                            {
                                billingReportString += lastPNum + ", " + runningQty + ", " + rate + "\n";
                                runningQty = row.qty;
                                rate = row.rate;
                                lastPNum = row.pNum;
                            }
                        }
                        //get the final row information
                        billingReportString += billingInfo[billingInfo.Count - 1].pNum + ", " +
                            runningQty + ", " +
                            billingInfo[billingInfo.Count - 1].rate + "\n";

                    }

                    //set up the initial string for ttf sorts
                    string ttfBillingReportString = "TTF Part Number, Quantity, TTF Rate \n";

                    //get the string for the ttf sorts file
                    if (ttfBillingInfo.Count > 0)
                    {
                        //initialized necessary variables
                        string lastTTF = ttfBillingInfo[0].pNum;
                        decimal ttfRate = ttfBillingInfo[0].ttfRate;
                        int runningTTFQty = 0;

                        foreach (DataClasses.Billing row in ttfBillingInfo)
                        {
                            if (lastTTF == row.pNum)
                            {
                                runningTTFQty += row.qty;
                            }
                            else
                            {
                                ttfBillingReportString += lastTTF + ", " + runningTTFQty + ", " + ttfRate + "\n";
                                runningTTFQty = row.qty;
                                ttfRate = row.rate;
                                lastTTF = row.pNum;
                            }
                        }
                        //get the final row information
                        ttfBillingReportString += ttfBillingInfo[ttfBillingInfo.Count - 1].pNum + ", " +
                            runningTTFQty + ", " +
                            ttfBillingInfo[ttfBillingInfo.Count - 1].ttfRate + "\n";
                    }

                    //create the .csv files for the ttf and normal sorts
                    string ttfDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(ttfDocPath, "ttfBilling.csv")))
                    {
                        outputFile.WriteLine(ttfBillingReportString);
                    }

                    string billingDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(billingDocPath, "normalBilling.csv")))
                    {
                        outputFile.WriteLine(billingReportString);
                    }

                    status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return status;
        }

        //------------------------------------------------

        public bool generateDailyReports(DateTime date)
        {
            bool status = false;
            List<DataClasses.Sort> incomingParts = new List<DataClasses.Sort>();
            List<DataClasses.Sort> outGoingParts = new List<DataClasses.Sort>();
            List<DataClasses.Sort> wipParts = new List<DataClasses.Sort>();
            List<DataClasses.Sort> wipFinishedParts = new List<DataClasses.Sort>();


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {

                    //get the information for new sorts and store it in a list
                    //used in daily summary
                    string newHoursQuery = "select sID, dateReceived, TTF, quantity, expectedPPM, bwi, oring, special " +
                        "from sorts natural join parts " +
                        "where dateReceived = @date";

                    using (MySqlCommand newHoursCommand = new MySqlCommand(newHoursQuery, connection, transaction))
                    {
                        newHoursCommand.Parameters.AddWithValue("@date", date);
                        using (MySqlDataReader reader = newHoursCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataClasses.Sort info = new DataClasses.Sort
                                {
                                    recDate = Convert.ToDateTime(reader["dateReceived"]),
                                    ttf = Convert.ToBoolean(reader["TTF"]),
                                    qty = Convert.ToInt32(reader["quantity"]),
                                    ppm = Convert.ToDouble(reader["expectedPPM"]),
                                    bwi = Convert.ToBoolean(reader["bwi"]),
                                    oring = Convert.ToBoolean(reader["oring"]),
                                    special = Convert.ToBoolean(reader["special"]),
                                    sID = Convert.ToInt32(reader["sID"])
                                };
                                incomingParts.Add(info);
                            }
                        }
                    }

                    //get the information for outgoing sorts and store it in a list
                    //used in daily summary
                    //used in wip
                    string partsOutputQuery = "select sID, dateReceived, dateFinished, TTF, quantity, expectedPPM, bwi, oring, special " +
                        "from sorts " +
                        "natural join parts " +
                        "where dateFinished = @date " +
                        "and checkManager is not null;";

                    using (MySqlCommand partsOutputCommand = new MySqlCommand(partsOutputQuery, connection, transaction))
                    {
                        partsOutputCommand.Parameters.AddWithValue("@date", date);
                        using (MySqlDataReader reader = partsOutputCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("making outGoingParts");
                                DataClasses.Sort info = new DataClasses.Sort
                                {
                                    recDate = Convert.ToDateTime(reader["dateReceived"]),
                                    finDate = Convert.ToDateTime(reader["dateFinished"]),
                                    ttf = Convert.ToBoolean(reader["TTF"]),
                                    qty = Convert.ToInt32(reader["quantity"]),
                                    ppm = Convert.ToDouble(reader["expectedPPM"]),
                                    bwi = Convert.ToBoolean(reader["bwi"]),
                                    oring = Convert.ToBoolean(reader["oring"]),
                                    special = Convert.ToBoolean(reader["special"]),
                                    sID = Convert.ToInt32(reader["sID"])
                                };
                                outGoingParts.Add(info);
                            }
                            Console.WriteLine("outGoingParts made");
                        }
                    }

                    //get information for current inventory
                    //used in wip
                    string wipInventoryQuery = "select pNum, quantity, dateReceived, TTF from sorts natural join parts " +
                        "where checkManager is null " +
                        "order by pNum, TTF, dateReceived";
                    using (MySqlCommand wipInventoryCommand = new MySqlCommand(wipInventoryQuery, connection, transaction))
                    {
                        wipInventoryCommand.Parameters.AddWithValue("@date", date);
                        using (MySqlDataReader reader = wipInventoryCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataClasses.Sort info = new DataClasses.Sort
                                {
                                    recDate = Convert.ToDateTime(reader["dateReceived"]),
                                    ttf = Convert.ToBoolean(reader["TTF"]),
                                    qty = Convert.ToInt32(reader["quantity"]),
                                    pNum = Convert.ToString(reader["pNum"])
                                };
                                wipParts.Add(info);
                            }
                        }
                    }

                    //get the information for inventory finished for today
                    //used in the wip
                    string wipFinishedQuery = "select sID, pNum, TTF, quantity, dQuantity, endTime, startTime, dateReceived, dateFinished " +
                        "from parts " +
                        "natural join sorts " +
                        "natural join work " +
                        "natural join sortDefects " +
                        "where checkManager is not null " +
                        "and dateFinished = @date " +
                        "order by pNum, TTF";

                    string wipFinishedNoDefectsQuery = "select sID, pNum, TTF, quantity, endTime, startTime, dateReceived, dateFinished " +
                        "from parts " +
                        "natural join sorts " +
                        "natural join work " +
                        "where checkManager is not null " +
                        "and dateFinished = @date " +
                        "and sID not in (select sID from sortDefects) " +
                        "order by pNum, TTF";

                    using (MySqlCommand wipFinishedCommand = new MySqlCommand(wipFinishedQuery, connection, transaction))
                    {
                        wipFinishedCommand.Parameters.AddWithValue("@date", date);
                        using (MySqlDataReader reader = wipFinishedCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataClasses.Sort info = new DataClasses.Sort
                                {
                                    sID = Convert.ToInt32(reader["sID"]),
                                    ttf = Convert.ToBoolean(reader["TTF"]),
                                    qty = Convert.ToInt32(reader["quantity"]),
                                    pNum = Convert.ToString(reader["pNum"]),
                                    dQty = Convert.ToInt32(reader["dQuantity"]),
                                    workStart = Convert.ToDateTime(reader["startTime"]),
                                    workEnd = Convert.ToDateTime(reader["endTime"]),
                                    recDate = Convert.ToDateTime(reader["dateReceived"]),
                                    finDate = Convert.ToDateTime(reader["dateFinished"])
                                };
                                wipFinishedParts.Add(info);
                            }
                        }
                    }

                    using (MySqlCommand wipFinishedNoDefectsCommand = new MySqlCommand(wipFinishedNoDefectsQuery, connection, transaction))
                    {
                        wipFinishedNoDefectsCommand.Parameters.AddWithValue("@date", date);
                        using (MySqlDataReader reader = wipFinishedNoDefectsCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataClasses.Sort info = new DataClasses.Sort
                                {
                                    sID = Convert.ToInt32(reader["sID"]),
                                    ttf = Convert.ToBoolean(reader["TTF"]),
                                    qty = Convert.ToInt32(reader["quantity"]),
                                    pNum = Convert.ToString(reader["pNum"]),
                                    dQty = 0,
                                    workStart = Convert.ToDateTime(reader["startTime"]),
                                    workEnd = Convert.ToDateTime(reader["endTime"]),
                                    recDate = Convert.ToDateTime(reader["dateReceived"]),
                                    finDate = Convert.ToDateTime(reader["dateFinished"])
                                };
                                wipFinishedParts.Add(info);
                            }
                        }
                    }

                    transaction.Commit();

                    //Daily summary report
                    //incoming parts
                    int runningBWISortTotal = 0;
                    int runningBWIPartTotal = 0;
                    int runningOringSortTotal = 0;
                    int runningOringPartTotal = 0;
                    int runningSpecialSortTotal = 0;
                    int runningSpecialPartTotal = 0;

                    foreach (DataClasses.Sort row in incomingParts)
                    {
                        if (row.bwi == true)
                        {
                            runningBWIPartTotal += row.qty;
                            runningBWISortTotal++;
                        }
                        else if (row.oring == true)
                        {
                            runningOringPartTotal += row.qty;
                            runningOringSortTotal++;
                        }
                        else if (row.special == true)
                        {
                            runningSpecialPartTotal += row.qty;
                            runningSpecialSortTotal++;
                        }
                        else
                        {
                            Console.WriteLine("Sort not given a type " + row.sID);
                            connection.Close();
                            return false;
                        }
                    }

                    //output parts
                    int runningBWISortOutput = 0;
                    int runningBWIPartOutput = 0;
                    int runningOringSortOutput = 0;
                    int runningOringPartOutput = 0;
                    int runningSpecialSortOutput = 0;
                    int runningSpecialPartOutput = 0;

                    foreach (DataClasses.Sort row in outGoingParts)
                    {
                        if (row.bwi == true)
                        {
                            runningBWIPartOutput += row.qty;
                            runningBWISortOutput++;
                        }
                        else if (row.oring == true)
                        {
                            runningOringPartOutput += row.qty;
                            runningOringSortOutput++;
                        }
                        else if (row.special == true)
                        {
                            runningSpecialPartOutput += row.qty;
                            runningSpecialSortOutput++;
                        }
                        else
                        {
                            Console.WriteLine("Sort not given a type " + row.sID);
                            connection.Close();
                            return false;
                        }
                    }

                    //add totals to the daily summary string
                    string dailySummaryString = "Date, BWI FO's Received, BWI Part Quantities Received, BWI FO Output, BWI Part Output, " +
                        "Oring FO's Received, Oring Part Quantities Received, Oring FO Output, Oring Part Output, " +
                        "Special FO's Received, Special Part Quantities Received, Special FO Output, Special Part Output, " +
                        "Total FO's Received, Total Parts Received, Total FO Output, Total Part Output \n";
                    int totalFosReceived = runningBWISortTotal + runningOringSortTotal + runningSpecialSortTotal;
                    int totalPartsReceived = runningBWIPartTotal + runningOringPartTotal + runningSpecialPartTotal;
                    int totalFoOutput = runningBWISortOutput + runningOringSortOutput + runningSpecialSortOutput;
                    int totalPartOutput = runningBWIPartOutput + runningOringPartOutput + runningSpecialPartOutput;
                    dailySummaryString += date.ToShortDateString() + ", " +
                        runningBWISortTotal + ", " +
                        runningBWIPartTotal + ", " +
                        runningBWISortOutput + ", " +
                        runningBWIPartOutput + ", " +
                        runningOringSortTotal + ", " +
                        runningOringPartTotal + ", " +
                        runningOringSortOutput + ", " +
                        runningOringPartOutput + ", " +
                        runningSpecialSortTotal + ", " +
                        runningSpecialPartTotal + ", " +
                        runningSpecialSortOutput + ", " +
                        runningSpecialPartOutput + ", " +
                        totalFosReceived + ", " +
                        totalPartsReceived + ", " +
                        totalFoOutput + ", " +
                        totalPartOutput + "\n";

                    //Create the Daily Summary file
                    string DailySummaryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(DailySummaryPath, "DailySummary.csv")))
                    {
                        outputFile.WriteLine(dailySummaryString);
                    }

                    //WIP report
                    //current inventory
                    List<List<string>> inv = new List<List<string>>();
                    List<string> header = new List<string>();

                    //Build the header row
                    header.Add("Part Numbers");
                    Console.WriteLine("first bit written");

                    List<string> datesSeen = new List<string>();
                    foreach (DataClasses.Sort row in wipParts)
                    {
                        if (!datesSeen.Contains(row.recDate.ToShortDateString()))
                        {
                            datesSeen.Add(row.recDate.ToShortDateString());
                        }
                    }
                    foreach (string value in datesSeen)
                    {
                        header.Add(value);
                        Console.WriteLine("date added");

                    }
                    header.Add("Average Parts Per FO");
                    header.Add("Total Left");
                    //header.Add("Total Finished Today");
                    //header.Add("Average Time in Hours");
                    //header.Add("Average Lead Time");
                    //header.Add("Average Parts Per Minute");
                    //header.Add("Average Fail Rate");
                    inv.Add(header);

                    //create the sub rows
                    List<string> pNumsSeen = new List<string>();
                    foreach (DataClasses.Sort row in wipParts)
                    {
                        if (!pNumsSeen.Contains(row.pNum))
                        {
                            Console.WriteLine("adding " + row.pNum.ToString());
                            pNumsSeen.Add(row.pNum);
                            Console.WriteLine("adding " + row.pNum + " TTF");
                            pNumsSeen.Add(row.pNum + " TTF");
                        }
                    }
                    foreach(string pNum in pNumsSeen)
                    {
                        Console.WriteLine("looking at " + pNum);
                        List<string> info = new List<string>();
                        int averageParts = 0;
                        int totalParts = 0;
                        int totalFOs = 0;
                        info.Add(pNum);

                        foreach (string columnDate in datesSeen)
                        {
                            Console.WriteLine("looking at " + columnDate);
                            int columnFOs = 0;
                            foreach(DataClasses.Sort row in wipParts)
                            {
                                if (row.pNum == pNum && row.recDate.ToShortDateString() == columnDate && row.ttf == false)
                                {
                                    columnFOs++;
                                    totalFOs++;
                                    totalParts += row.qty;
                                }
                                else if ((row.pNum + " TTF") == pNum && row.recDate.ToShortDateString() == columnDate && row.ttf == true)
                                {
                                    columnFOs++;
                                    totalFOs++;
                                    totalParts += row.qty;
                                }
                            }
                            info.Add(columnFOs.ToString());
                        }
                        if (totalFOs != 0)
                        {
                            averageParts = totalParts / totalFOs;
                        }
                        info.Add(averageParts.ToString());
                        info.Add(totalFOs.ToString());
                        inv.Add(info);
                        foreach(string value in info)
                        {
                            Console.WriteLine(value);
                        }
                    }

                    //create wip file
                    string wipPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(wipPath, "wip.csv")))
                    {
                        foreach(List<string> row in inv)
                        {
                            outputFile.WriteLine(string.Join(",", row));
                        }
                    }

                    //wip output file
                    //create header
                    List<List<string>> outputWip = new List<List<string>>();
                    List<string> outputHeader = new List<string>();
                    outputHeader.Add("Part Numbers");
                    outputHeader.Add("Total Finished Today");
                    outputHeader.Add("Average Time in Hours");
                    outputHeader.Add("Average Lead Time");
                    outputHeader.Add("Average Parts Per Minute");
                    outputHeader.Add("Average Fail Rate");
                    outputWip.Add(outputHeader);
                    List<string> outputPNumsSeen = new List<string>();
                    foreach (DataClasses.Sort row in wipFinishedParts)
                    {
                        Console.WriteLine("checking " + row.sID.ToString());
                        if (!outputPNumsSeen.Contains(row.pNum))
                        {
                            Console.WriteLine("adding " + row.pNum.ToString());
                            outputPNumsSeen.Add(row.pNum);
                            Console.WriteLine("adding " + row.pNum + " TTF");
                            outputPNumsSeen.Add(row.pNum + " TTF");
                        }
                    }

                    //create sub-rows
                    foreach(string value in outputPNumsSeen)
                    {
                        int FOs = 0;
                        int parts = 0;
                        int defects = 0;
                        int workSecs = 0;
                        int runningLeadTime = 0;
                        int leadTime = 0;
                        double ppm = 0;
                        double failPercentage = 0;
                        double workHours = 0;
                        List<int> sIDsSeen = new List<int>();
                        List<string> outputInfo = new List<string>();
                        foreach(DataClasses.Sort row in wipFinishedParts)
                        {
                            if (row.pNum == value && !sIDsSeen.Contains(row.sID))
                            {
                                sIDsSeen.Add(row.sID);
                                FOs++;
                                parts += row.qty;
                                defects += row.dQty;
                                workSecs += (int)(row.workEnd - row.workStart).TotalSeconds;
                                runningLeadTime += (int)(row.finDate - row.recDate).TotalDays;
                            }
                            else if (row.pNum == value && sIDsSeen.Contains(row.sID))
                            {
                                workSecs += (int)(row.workEnd - row.workStart).TotalSeconds;
                            }
                        }
                        //convert seconds to hours
                        if (FOs > 0)
                        {
                            workHours = ((double)workSecs / 3600) / FOs;
                        }
                        if (FOs > 0)
                        {
                            leadTime = runningLeadTime / FOs;
                        }
                        //convert parts per second to parts per minute
                        if (workSecs > 0)
                        {
                            ppm = ((double)parts / workSecs) * 60;
                        }
                        //get the percentage of parts failed
                        if (parts > 0)
                        {
                            failPercentage = ((double)defects / parts);
                        }
                        outputInfo.Add(value);
                        outputInfo.Add(FOs.ToString());
                        outputInfo.Add(workHours.ToString("0.00"));
                        outputInfo.Add(leadTime.ToString());
                        outputInfo.Add(ppm.ToString("0.00"));
                        outputInfo.Add(failPercentage.ToString("0.00"));
                        outputWip.Add(outputInfo);
                    }

                    //create wip output file
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(wipPath, "wip.csv"), true))
                    {
                        outputFile.WriteLine("\n");
                        outputFile.WriteLine("\n");
                        foreach (List<string> row in outputWip)
                        {
                            outputFile.WriteLine(string.Join(",", row));
                        }
                    }

                    status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return status;
        }

        public DataClasses.Part populateEditPart(string pNum)
        {
            DataClasses.Part ret = new DataClasses.Part();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                String query = "select * from parts where pNum = @pNum limit 1";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@pNum", pNum);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ret.pNum = Convert.ToString(reader["pNum"]);
                                ret.rate = Convert.ToDouble(reader["rate"]);
                                ret.ttfRate = Convert.ToDouble(reader["ttfRate"]);
                                ret.ppm = Convert.ToDouble(reader["expectedPPM"]);
                                ret.bwi = Convert.ToBoolean(reader["bwi"]);
                                ret.oring = Convert.ToBoolean(reader["oring"]);
                                ret.special = Convert.ToBoolean(reader["special"]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return ret;
        }

        public string getPinHash(int eID)
        {
            string ret = "";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                String query = "select pinHash from employees where eID = @eID limit 1";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@eID", eID);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ret = reader[0].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    ret = "failure";
                }
                finally
                {
                    connection.Close();
                }
            }
            return ret;
        }

        public ManagerControllers()
		{
		}
	}
}

