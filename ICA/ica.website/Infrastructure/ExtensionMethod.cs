using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ica.website.Infrastructure
{
    public static class ExtensionMethod
    {
        public static string Repeat(this char chatToRepeat, int repeat)
        {

            return new string(chatToRepeat, repeat);
        }
        public static string Repeat(this string stringToRepeat, int repeat)
        {
            var builder = new StringBuilder(repeat * stringToRepeat.Length);
            for (int i = 1; i < repeat; i++)
            {
                builder.Append(stringToRepeat);
            }
            return builder.ToString();
        }
    }
}
