using System.Numerics;

namespace WebApiService.Enums
{    
    public class ServiceRequestStatus
    {
        public const int None = 0;
        public const int Submited = 1;
        public const int Accepted = 2;
        public const int Ongoing = 4;
        public const int Rejected = 8;
        public const int Closed = 16;
        public const int ClosedInvoicing = 32;
        public const int ClosedInvoice = 64;
        public const int Archived = 128;
        
        //pamietaj aby wprowadzic zmiany w DataContext OnModelCreating

        public static string GetText(int id)
        {
            switch (id)
            {
                case None:
                    return string.Empty;
                case Submited:
                    return "Zgłoszone";
                case Accepted:
                    return "Przyjęte";
                case Ongoing:
                    return "W realizacji";
                case Rejected:
                    return "Odrzucone";
                case Closed:
                    return "Zamknięte";
                case ClosedInvoicing:
                    return "Zamknięte do fakturowania";
                case ClosedInvoice:
                    return "Zamknięte faktura";
                case Archived:
                    return "Zarchiwizowane";
            }

            return string.Empty;
        }

        public static int GetSorting(int id)
        {
            switch (id)
            {
                case None:
                    return 0;
                case Rejected: //Odrzucone
                    return 1;
                case Accepted: //Przyjęte
                    return 2;
                case Ongoing: //W realizacji
                    return 3;                
                case Closed: //Zamknięte
                    return 4;
                case ClosedInvoicing: //Zamknięte do fakturowania
                    return 5;
                case ClosedInvoice: //Zamknięte faktura
                    return 6;
                case Archived: //Zarchiwizowane
                    return 7;
                case Submited: //Zgłoszone
                    return 8;
            }

            return 0;
        }
    }
}
