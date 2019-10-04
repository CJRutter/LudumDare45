using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CamiFramwork
{
    public static class StringHelper
    {
        public static string ToBase64String(string value)
        {
            byte[] data = Encoding.UTF8.GetBytes(value);
            return System.Convert.ToBase64String(data);
        }

        public static string FromBase64String(string value)
        {
            byte[] data = System.Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(data);
        }

        public static int Tokenise(string source, string delim, ICollection<string> tokens)
        {
            if (string.IsNullOrEmpty(source))
                return 0;

            string token;
            int count = 0;
            int current = 0;
            int next = source.IndexOf(delim);
            while (next >= 0)
            {
                token = source.Substring(current, next - current);
                tokens.Add(token);
                count++;

                current = next + delim.Length;
                next = source.IndexOf(delim, current);
            }
            
            token = source.Substring(current, source.Length - current);
            tokens.Add(token);
            count++;
            
            return count;
        }
    }
}
