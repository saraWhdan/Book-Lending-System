using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingSystem.Application.Interfaces.IServices
{
    public interface IEmailService
    {

        Task SendEmailReturnAsync();
    }
}

