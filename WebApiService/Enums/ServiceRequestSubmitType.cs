﻿using System.Numerics;

namespace WebApiService.Enums
{
    public class ServiceRequestSubmitType
    {
        public const int None = 0;
        public const int WWW = 1;
        public const int Email = 2;
        public const int Phone = 3;
        public const int Internal = 4;

        //pamietaj aby wprowadzic zmiany w DataContext OnModelCreating

        public static string GetText(int id)
        {
            switch (id)
            {
                case None:
                    return string.Empty;
                case WWW:
                    return "WWW";
                case Email:
                    return "E-mail";
                case Phone:
                    return "Telefon";
                case Internal:
                    return "Wewnętrzne";
            }

            return string.Empty;
        }

        public static int GetSorting(int id)
        {
            switch (id)
            {
                case None:
                    return 0;
                case Email: //E-mail
                    return 1;                               
                case Phone: //Telefon
                    return 2;
                case Internal: //Wewnętrzne
                    return 3;
                case WWW: //WWW
                    return 4;
            }

            return 0;
        }
    }
}
