using BookLendingSystem.Domain.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Domain.Entities
{
    public class BorrowedBook
    {
        [Key]
        public int Id { get; set; } //PK
        [Required]
        public int? BookId { get; set; } //FK
        [Required(ErrorMessage = Errors.RequiredField)]
        public string MemberId { get; set; } //FK to identity
      
        
       
        public DateTime BorrowDate { get; set; }
      
        public DateTime? ReturnDate { get; set; }
      
      
        public Book Book { get; set; }
        // need navigation property for user
    }
}

