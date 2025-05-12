using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Application.Dtos
{
    public class CreateBookDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]

        public string ISBN { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }


        public int Quantity { get; set; }
    }
}
