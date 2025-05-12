using BookLendingSystem.Application.Interfaces.IServices;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BookLendingSystem.Infrastructure.Context;
using BookLendingSystem.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace BookLendingSystem.Persistence.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting emailSetting;
        private readonly ApplicationDbContext bookContext;
        public EmailService(IOptions<EmailSetting> _emailSetting, ApplicationDbContext _bookContext)
        {
            emailSetting = _emailSetting.Value;
            bookContext = _bookContext;
        }


        public async Task<IEnumerable<SendingEmail>> GetMemberReturnAsyn()
        {
            var day = DateTime.Today;
            List<SendingEmail> result = new List<SendingEmail>();
            var value = await bookContext.BorrowedBooks.Where(x => !x.IsReturned && x.ActionDate < day).Select(x => new SendingEmail
            {
                MemberName = x.Member.UserName,
                BookName = x.Book.Title,
                MemberEmail = x.Member.Email
            }).ToListAsync();
            if (value != null)
            {
                result.AddRange(value);
                return result;
            }
            return null;


        }

        public async Task SendEmailReturnAsync()
        {
            var sendingEmails = await GetMemberReturnAsyn();
            var smtpClient = new SmtpClient(emailSetting.Host)
            {
                Port = int.Parse(emailSetting.Port),
                Credentials = new NetworkCredential(emailSetting.Username, emailSetting.Password),
                EnableSsl = true,
            };
            foreach (SendingEmail sendingEmail in sendingEmails)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailSetting.From),
                    Subject = "Not return book",

                    Body = $"please {sendingEmail.MemberName} hurry up bring back the books{sendingEmail.BookName} period available has expired ",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(sendingEmail.MemberEmail);

                await smtpClient.SendMailAsync(mailMessage);

            }

        }
    }
}
