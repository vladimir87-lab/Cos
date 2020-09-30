using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ControlOper.Service
{
    public class Function
    {
        public static string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string ConvertStrDateTame(string str)
        {
            try
            {
                string time = str.Split('T')[1];
                string date = str.Split('T')[0];
                time = time.Split('.')[0];
                return date + " " + time;
            }
            catch { return str; }
                
        }

        public static string ReadStringConnect( string patch)
        {

            StreamReader strr = new StreamReader(patch);
            
            string str = strr.ReadLine();
            return str;
        }


        public static Dictionary<int, string> AddBalsDictin()
        {
            Dictionary<int, string> bals = new Dictionary<int, string>(6);
            bals.Add(0, "0 балов");
            bals.Add(1, "1 бал");
            bals.Add(2, "2 бала");
            bals.Add(3, "3 бала");
            bals.Add(4, "4 бала");
            bals.Add(5, "5 балов");
            return bals.OrderByDescending( k => k.Key ).ToDictionary(k => k.Key, k => k.Value);
        }
        public static Dictionary<int, string> AddOcIsObosn()
        {
            Dictionary<int, string> OcIsObosn = new Dictionary<int, string>(3);
            OcIsObosn.Add(0, "Не выбрано");
            OcIsObosn.Add(1, "Обоснована");
            OcIsObosn.Add(2, "Не обоснована");

            return OcIsObosn;
        }
        public static Dictionary<int, string> AddNizOcOperOrMeneg()
        {
            Dictionary<int, string> NizOcOperOrMeneg = new Dictionary<int, string>(3);
            NizOcOperOrMeneg.Add(0, "Не выбрано");
            NizOcOperOrMeneg.Add(1, "Оператора");
            NizOcOperOrMeneg.Add(2, "Менеджера");
            NizOcOperOrMeneg.Add(3, "Оператора и менеджера");

            return NizOcOperOrMeneg;
        }



    }
}