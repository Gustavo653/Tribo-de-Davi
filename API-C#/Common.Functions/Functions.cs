using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Functions
{
    public static class Functions
    {
        public static int CalculateAge(DateTime birthDate)
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - birthDate.Year;

            if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
                age--;

            return age;
        }

        public static string GetUrl(string url)
        {
            if (url.Contains("tribo-davi-hom")) return url;
            else return url.Replace("tribo-davi", Environment.GetEnvironmentVariable("GCS_BUCKET_NAME"));
        }
    }
}
