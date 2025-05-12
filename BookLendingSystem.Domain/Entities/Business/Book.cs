using BookLendingSystem.Domain.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Domain.Entities.Business
{
    public class Book
    {

        public int Id { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        [StringLength(100)]
        public string Author { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        [RegularExpression(Regex.ISBN, ErrorMessage = Errors.FormatISBN)]
        public string ISBN { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        [DataType(DataType.Date)]

        public DateTime PublishedDate { get; set; }


        public int Quantity { get; set; }

        // Navigation Property
        public ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}

