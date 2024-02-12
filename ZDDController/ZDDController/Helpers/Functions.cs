using System;
using System.Security.Cryptography;
using System.Text;
namespace ZDDController.Helpers
{
	public class Functions
	{
        public string computeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // Convert each byte to a hexadecimal string
                }
                return builder.ToString();
            }
        }
    }
}

