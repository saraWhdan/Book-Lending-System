using Microsoft.AspNetCore.Identity;

namespace BookLendingSystem.Domain.Entities.Business
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<BorrowedBook> borrowedBooks { get; set; }

    }
}