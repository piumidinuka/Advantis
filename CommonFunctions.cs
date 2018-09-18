using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace Advantis
{
    class CommonFunctions
    {

        public static String sha256_hash(String value)
        {
            /*  StringBuilder Sb = new StringBuilder();

              using (SHA256 hash = SHA256Managed.Create())
              {
                  Encoding enc = Encoding.UTF8;
                  Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                  foreach (Byte b in result)
                      Sb.Append(b.ToString("x2"));
              }

              return Sb.ToString();
          }*/
            StringBuilder sb = new StringBuilder();

            using (MD5 md5 = MD5.Create())
            {

                //create an array of ascii encoding of password

                byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(value);

                // create hash of created array element
                byte[] hash = md5.ComputeHash(bytes);
                // when hash is computed then create a string of encrypted password using string builder class and append the values created to make a full string encrypted message

                //StringBuilder sb = new StringBuilder();
                for (int b = 0; b < hash.Length; b++)
                {
                    // created hex encrypted md5 password in string format
                    sb.Append(hash[b].ToString("X2"));

                }
                return sb.ToString();

            }
        }
    }
}
