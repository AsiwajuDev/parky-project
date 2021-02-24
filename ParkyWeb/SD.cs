using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:44377/";
        public static string NationalParkAPIPath = APIBaseUrl + "api/NationalParks";
        public static string TrailAPIPath = APIBaseUrl + "api/trails";
    }
}
