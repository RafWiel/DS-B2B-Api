namespace WebApiService.Enums
{
    public class CustomerType
    {
        public const int None = 0;        
        public const int Supervisor = 2;
        public const int Employee = 3;

        //pamietaj aby wprowadzic zmiany w DataContext OnModelCreating

        public static string GetText(int id)
        {
            switch (id)
            {
                case None:
                    return string.Empty;
                case Supervisor:
                    return "Kierownik";
                case Employee:
                    return "Pracownik";
            }

            return string.Empty;
        }

        public static int GetSorting(int id)
        {
            switch (id)
            {
                case None:
                    return 0;
                case Supervisor: //Kierownik
                    return 2;
                case Employee: //Pracownik
                    return 3;
            }

            return 0;
        }
    }   
}
