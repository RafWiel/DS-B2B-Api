using System.Numerics;

namespace WebApiService.Enums
{
    
    public class ServiceRequestStatus
    {
        public const int None = 0;
        public const int Submited = 1;
        public const int Accepted = 2;
        public const int Ongoing = 3;
        public const int Rejected = 4;
        public const int Closed = 5;
        public const int ClosingInvoice = 6;
        public const int ClosedInvoice = 7;
        public const int Archived = 8;
        
        //pamietaj aby wprowadzic zmiany w DataContext

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
                case ClosingInvoice:
                    return "Zamknięte do fakturowania";
                case ClosedInvoice:
                    return "Zamknięte faktura";
                case Archived:
                    return "Zarchiwizowane";
            }

            return string.Empty;
        }
    }
}
