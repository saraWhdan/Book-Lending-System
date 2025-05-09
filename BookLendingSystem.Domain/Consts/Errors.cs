using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Domain.Consts
{
    public static class Errors
    {
        public const string RequiredField = "Required field";
        public const string NotAllowedExtension = "image must end with .png or .jpg  or .jpeg";
        public const string NotEmpty = " cannot be empty.";
        public const string MobileNumber = "Invalid Egyptian mobile number format.";
        public const string FormatISBN = "ISBN must be in the format XXX-XXXXXXXXXX.";


    }
}
