using System;
using JetBrains.Annotations;

namespace HelperServices
{
    public static class Utility
    {
        public static string GenerateId()
        {
            string id = "";
            string guid = Guid.NewGuid().ToString();
            var sections = guid.Split('-');
            string dateId = DateTime.UtcNow.Ticks.ToString();
            for(int i = 2; i < sections.Length; ++i)
            {
                id += sections[i];
            }

            dateId = dateId + "-" + id;
            return dateId;
        }

        public static String GetCreatedDate(string id)
        {
            var sections = id.Split("-");
            var tick = Int64.Parse(sections[0]);

            var dateTime = new DateTime(tick);

            String date = dateTime.ToString("dd-MM-yyyy");

            return date;
        }

        public static Type CheckNotNull<Type>(Type value, [InvokerParameterName][NotNull] string parameterName)
        {
            if(value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }
    }
}
