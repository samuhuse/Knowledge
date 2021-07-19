using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMvcConsumer.Util
{
    public static class StaticDetails
    {
        public static class SimpleApi
        {
            public static string BaseUrl => "https://localhost:5001/";
            public static string ContactApiUrl => BaseUrl + "api/contacts/";
            public static string CompanyApiUrl => BaseUrl + "api/company/";
        }

        public static class JwtBearerApi
        {
            public static string BaseUrl => "https://localhost:44328/";
            public static string LogInApiUrl => BaseUrl + "api/user/";
            public static string RegisterApiUrl => BaseUrl + "api/user/register/";
            public static string DrinkApiUrl => BaseUrl + "api/drink/";
        }

    }
}
