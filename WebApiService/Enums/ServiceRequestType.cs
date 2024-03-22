namespace WebApiService.Enums
{
    public class ServiceRequestType
    {
        public const int None = 0;
        public const int Software = 1;
        public const int Hardware = 2;
        public const int Other = 3;

        //pamietaj aby wprowadzic zmiany w DataContext

        public static string GetText(int id)
        {
            switch (id)
            {
                case None:
                    return string.Empty;
                case Software:
                    return "Programowe";
                case Hardware:
                    return "Sprzętowe";
                case Other:
                    return "Inne";
            }

            return string.Empty;
        }
    }
}
