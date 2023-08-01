namespace HouseRentingSystem.Data
{
    public class DataConstants
    {
        public class House
        {
            public const int TitleMaxLength = 50;
            public const int TitleMinLength = 10;

            public const int AddressMaxLength = 150;
            public const int AddressMinLength = 30;

            public const int DescriptionMaxLength = 500;
            public const int DescriptionMinLength = 50;

            public const int MaxPricePerMonth = 20000;

        }

        public class Category
        {
            public const int NameMaxLength = 50; 
        }

        public class Agent
        {
            public const int PhoneNumberMaxLength = 15;
            public const int PhoneNumberMinLength = 7;
        }

        public class User
        {
            public const int UserFirstNameMaxLenght = 12;
            public const int UserFirstNameMinLenght = 1;

            public const int UserLastNameMaxLenght = 15;
            public const int UserLastNameMinLenght = 3;
        }
    }
}