using BookLendingSystem.Domain.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BookLendingSystem.Infrastructure.Identity;

namespace BookLendingSystem.Domain.Entities.Business
{
    public class BorrowedBook
    {

        public int Id { get; set; } //PK
        [Required]
        public int? BookId { get; set; } //FK
        [Required(ErrorMessage = Errors.RequiredField)]
        public string MemberId { get; set; } //FK to identity


        public DateTime BorrowDate { get; set; }

        public DateTime? ReturnDate { get; set; }
        public DateTime? ActionDate { get; set; }

        public bool IsReturned { get; set; }

        public Book Book { get; set; }
        // need navigation property for user
        public ApplicationUser Member { get; set; }

    }
}

