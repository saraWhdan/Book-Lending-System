using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Application.Dtos
{
    public class BookDto
    {
        public string Title { get; set; }

       
        public string Author { get; set; }

   
        public string ISBN { get; set; }

      
        public DateTime PublishedDate { get; set; }


        public int Quantity { get; set; }

    }
}
