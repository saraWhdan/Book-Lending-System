

namespace BookLendingSystem.Domain.Consts
{
    public static class Regex
    {
        public const string ISBN = @"^\d{3}-\d{10}$";
        public const string MobileNumber = "^01[0,1,2,5]{1}[0-9]{8}$";
        public const string AllowedExtensions = @"\.png$|\.jpeg$|\.jpg";

    }
}
