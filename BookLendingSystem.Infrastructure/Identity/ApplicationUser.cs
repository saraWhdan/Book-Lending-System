using BookLendingSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<BorrowedBook> BorrowedBooks { get; set; }
        public bool CanBorrow { get; set; }
    }
}
