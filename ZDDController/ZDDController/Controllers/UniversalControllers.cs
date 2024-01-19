using System;
using System.Data;
using MySqlConnector;

namespace ZDDController.Controllers
{
	public class UniversalControllers
	{
        string connectionString = "Server=localhost;Database=testingdb;Uid=root;Pwd=m123;";

        public List<String> getParts()
        {
            List<String> partNums = new List<string>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                String query = "select pNum from parts";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                partNums.Add(reader.GetString("pNum"));
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
            return partNums;
        }

        public List<String> manCheckParts()
        {
            List<String> fos = new List<string>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                String query = "select FO from sorts where checkManager is null and finished = true";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fos.Add(reader.GetString("FO"));
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
            return fos;
        }

        public List<String> partsToEnd()
        {
            List<String> fos = new List<string>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                String query = "select FO from sorts " +
                    "natural join parts " +
                    "where finished = false " +
                    "and special = false " +
                    "and sID in (select sID from work)";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fos.Add(reader.GetString("FO"));
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
            return fos;
        }

        public UniversalControllers()
		{
		}
	}
}

