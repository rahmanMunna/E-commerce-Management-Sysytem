using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EcommerceWeb.Helpers
{
    public static class HashHelper
    {
        public static string GetMd5Hash(string input)
        {
            // Create MD5 instance
            using (MD5 md5 = MD5.Create())
            {
                // Convert input string to byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Compute the MD5 hash
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("x2"));  // x2 = lowercase hex format

                return sb.ToString();
            }
        }
    }
}