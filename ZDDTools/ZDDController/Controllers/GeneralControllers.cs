using System;
using MySqlConnector;

namespace ZDDController.Controllers
{
	public class GeneralControllers
	{
        string connectionString = "Server=localhost;Database=testingdb;Uid=root;Pwd=m123;";

        public bool startSort(int eID, System.DateTime startTime, int fo, String pNum)
		{
			bool status = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "insert into work (eID, sID, startTime) " +
                    "select @eID, sID, @startTime from sorts where " +
                    "fo = @fo and " +
                    "pID = (select pID from parts where pNum = @pNum)";
                try
                {

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@eID", eID);
                        command.Parameters.AddWithValue("@startTime", startTime);
                        command.Parameters.AddWithValue("@fo", fo);
                        command.Parameters.AddWithValue("@pNum", pNum);

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

        //-------------------------------

        public int getSid(int fo)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "select sID from sorts where fo = @fo";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fo", fo);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return reader.GetInt32(0);
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
            return -1;
        }

        //-------------------------------

        public bool stopWorkingOnSort(int sid, DateTime combined, int eid)
        {
            Console.WriteLine("in stopWorkingOnSort");
            bool status = false;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string updateWorkCommand = "update work set endTime = @combined where sID = @sid and eID = @eid and endTime is null";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(updateWorkCommand, connection))
                    {
                        command.Parameters.AddWithValue("@combined", combined);
                        command.Parameters.AddWithValue("@sid", sid);
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

        //-------------------------------

        //public bool finishSort(int sid, string comments, DateTime combined)
        //{
        //    Console.WriteLine("in finishSort");
        //    bool status = false;
        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string updateSortCommand = "update sorts set dateFinished = @combined, " +
        //            "finished = true, " +
        //            "comments = @comments " +
        //            "where sID = @sid";

        //        using (MySqlCommand command = new MySqlCommand(updateSortCommand, connection))
        //        {
        //            command.Parameters.AddWithValue("@combined", combined);
        //            command.Parameters.AddWithValue("@sid", sid);
        //            command.Parameters.AddWithValue("@comments", comments);

        //            command.ExecuteNonQuery();
        //            status = true;
        //        }
        //        connection.Close();
        //    }
        //    return status;
        //}

        //-------------------------------

        //public bool addSortDefects(int sid, List<int> dQtys, List<string> dNames)
        //{
        //    Console.WriteLine("in addSortDefects");
        //    bool status = false;
        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string updateDefectsCommand = "insert into sortDefects (dID, sID, dQuantity) " +
        //            "select dID, @sid, @dQtys from defects where dName = @dName";

        //        try
        //        {
        //            for (int i = 0; i < dQtys.Count; i++)
        //            {
        //                using (MySqlCommand command = new MySqlCommand(updateDefectsCommand, connection))
        //                {
        //                    command.Parameters.AddWithValue("@dQtys", dQtys[i]);
        //                    command.Parameters.AddWithValue("@sid", sid);
        //                    command.Parameters.AddWithValue("@dName", dNames[i]);

        //                    command.ExecuteNonQuery();
        //                }
        //            }
        //            status = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"An error occurred: {ex.Message}");
        //        }
        //        finally
        //        {
        //            connection.Close();
        //        }

        //        return status;
        //    }
        //}

        //-------------------------------

        public bool addDefects(List<string> dNames)
        {
            if (dNames is null || dNames.Count == 0)
            {
                return true;
            }

            Console.WriteLine("in addDefects");
            bool status = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                Console.WriteLine("opening connection");
                connection.Open();
                Console.WriteLine("connection open");

                string insertDefectsCommand = "insert ignore into defects (dName) VALUES (@dName)";

                Console.WriteLine("starting loop");
                try
                {
                    foreach (string dName in dNames)
                    {
                        using (MySqlCommand command = new MySqlCommand(insertDefectsCommand, connection))
                        {
                            Console.WriteLine("adding defect " + dName);
                            command.Parameters.AddWithValue("@dName", dName);
                            command.ExecuteNonQuery();
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

        //-------------------------------

        //Makes sure that all tables involved with completing the sort are changed together
        public bool completeSort(List<string> dNames, List<int> dQtys, int sid, string comments, DateTime combined, int eid)
        {
            bool status = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string insertDefectsQuery = "insert ignore into defects (dName) VALUES (@dName)";

                    Console.WriteLine("starting loop");

                    foreach (string dName in dNames)
                    {
                        using (MySqlCommand insertDefectsCommand = new MySqlCommand(insertDefectsQuery, connection, transaction))
                        {
                            Console.WriteLine("adding defect " + dName);
                            insertDefectsCommand.Parameters.AddWithValue("@dName", dName);
                            insertDefectsCommand.ExecuteNonQuery();
                        }
                    }

                    string updateWorkQuery = "update work set endTime = @combined " +
                        "where sID = @sid " +
                        "and eID = @eid " +
                        "and endTime is null";

                    using (MySqlCommand updateWorkCommand = new MySqlCommand(updateWorkQuery, connection, transaction))
                    {
                        updateWorkCommand.Parameters.AddWithValue("@combined", combined);
                        updateWorkCommand.Parameters.AddWithValue("@sid", sid);
                        updateWorkCommand.Parameters.AddWithValue("@eid", eid);

                        updateWorkCommand.ExecuteNonQuery();
                    }

                    string updateSortQuery = "update sorts set dateFinished = @combined, " +
                    "finished = true, " +
                    "comments = @comments " +
                    "where sID = @sid";

                    using (MySqlCommand updateSortCommand = new MySqlCommand(updateSortQuery, connection, transaction))
                    {
                        updateSortCommand.Parameters.AddWithValue("@combined", combined);
                        updateSortCommand.Parameters.AddWithValue("@sid", sid);
                        updateSortCommand.Parameters.AddWithValue("@comments", comments);

                        updateSortCommand.ExecuteNonQuery();
                    }

                    string insertWorkDefectsQuery = "insert into sortDefects (dID, sID, dQuantity) " +
                    "select dID, @sid, @dQtys from defects where dName = @dName";

                    for (int i = 0; i < dQtys.Count; i++)
                    {
                        using (MySqlCommand insertWorkDefectsCommand = new MySqlCommand(insertWorkDefectsQuery, connection, transaction))
                        {
                            insertWorkDefectsCommand.Parameters.AddWithValue("@dQtys", dQtys[i]);
                            insertWorkDefectsCommand.Parameters.AddWithValue("@sid", sid);
                            insertWorkDefectsCommand.Parameters.AddWithValue("@dName", dNames[i]);

                            insertWorkDefectsCommand.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    status = true;
                }
                catch (Exception ex)
                {
                    // An error occurred, so roll back the transaction
                    Console.WriteLine($"Error: {ex.Message}");
                    transaction.Rollback();
                }
                finally
                {
                    connection.Close();
                }

                return status;
            }
        }

        //-------------------------------

        public bool stopWorkingOnSpecialSort(int sid, DateTime startCombined, DateTime endCombined, int eid)
        {
            Console.WriteLine("in stopWorkingOnSpecialSort");
            bool status = false;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                
                try
                {
                    string insertWorkCommand = "insert into work (eID, sID, startTime, endTime)" +
                    "values (@eid, @sid, @startCombined, @endCombined)";

                    using (MySqlCommand command = new MySqlCommand(insertWorkCommand, connection))
                    {
                        command.Parameters.AddWithValue("@startCombined", startCombined);
                        command.Parameters.AddWithValue("@endCombined", endCombined);
                        command.Parameters.AddWithValue("@sid", sid);
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

        //-------------------------------

        //Makes sure that all tables involved with completing the sort are changed together
        public bool completeSpecialSort(List<string> dNames, List<int> dQtys, int sid, string comments, DateTime startCombined, DateTime endCombined, int eid)
        {
            bool status = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string insertDefectsQuery = "insert ignore into defects (dName) VALUES (@dName)";

                    Console.WriteLine("starting loop");

                    foreach (string dName in dNames)
                    {
                        using (MySqlCommand insertDefectsCommand = new MySqlCommand(insertDefectsQuery, connection, transaction))
                        {
                            Console.WriteLine("adding defect " + dName);
                            insertDefectsCommand.Parameters.AddWithValue("@dName", dName);
                            insertDefectsCommand.ExecuteNonQuery();
                        }
                    }

                    string insertWorkQuery = "insert into work (eID, sID, startTime, endTime)" +
                    "values (@eid, @sid, @startCombined, @endCombined)";

                    using (MySqlCommand insertWorkCommand = new MySqlCommand(insertWorkQuery, connection, transaction))
                    {
                        insertWorkCommand.Parameters.AddWithValue("@startCombined", startCombined);
                        insertWorkCommand.Parameters.AddWithValue("@endCombined", endCombined);
                        insertWorkCommand.Parameters.AddWithValue("@sid", sid);
                        insertWorkCommand.Parameters.AddWithValue("@eid", eid);

                        insertWorkCommand.ExecuteNonQuery();
                    }

                    string updateSortQuery = "update sorts set dateFinished = @endCombined, " +
                    "finished = true, " +
                    "comments = @comments, " +
                    "checkManager = 1 " +
                    "where sID = @sid";

                    using (MySqlCommand updateSortCommand = new MySqlCommand(updateSortQuery, connection, transaction))
                    {
                        updateSortCommand.Parameters.AddWithValue("@endCombined", endCombined);
                        updateSortCommand.Parameters.AddWithValue("@sid", sid);
                        updateSortCommand.Parameters.AddWithValue("@comments", comments);

                        updateSortCommand.ExecuteNonQuery();
                    }

                    string insertWorkDefectsQuery = "insert into sortDefects (dID, sID, dQuantity) " +
                    "select dID, @sid, @dQtys from defects where dName = @dName";

                    for (int i = 0; i < dQtys.Count; i++)
                    {
                        using (MySqlCommand insertWorkDefectsCommand = new MySqlCommand(insertWorkDefectsQuery, connection, transaction))
                        {
                            insertWorkDefectsCommand.Parameters.AddWithValue("@dQtys", dQtys[i]);
                            insertWorkDefectsCommand.Parameters.AddWithValue("@sid", sid);
                            insertWorkDefectsCommand.Parameters.AddWithValue("@dName", dNames[i]);

                            insertWorkDefectsCommand.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    status = true;
                }
                catch (Exception ex)
                {
                    // An error occurred, so roll back the transaction
                    Console.WriteLine($"Error: {ex.Message}");
                    transaction.Rollback();
                }
                finally
                {
                    connection.Close();
                }

                return status;
            }
        }

        //-------------------------------

        public int getSpecialSid(string pNum, int qty)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "select sID from sorts natural join parts where pNum = @pNum and finished = 0 and quantity = @qty limit 1";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@pNum", pNum);
                        command.Parameters.AddWithValue("@qty", qty);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return reader.GetInt32(0);
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
            return -1;
        }

        //-------------------------------

        public GeneralControllers()
		{
		}
	}
}

