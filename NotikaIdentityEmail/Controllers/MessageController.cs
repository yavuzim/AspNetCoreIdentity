using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models;
using NuGet.Protocol.Plugins;

namespace NotikaIdentityEmail.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Inbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var values = (from m in _context.Messages
                          join u in _context.Users
                          on m.SenderEmail equals u.Email into userGroup
                          from sender in userGroup.DefaultIfEmpty()

                          join c in _context.Categories
                          on m.CategoryId equals c.CategoryId into categoryGroup
                          from category in categoryGroup.DefaultIfEmpty()

                          where m.ReceiverEmail == user.Email
                          select new MessageWithSenderInfoViewModel
                          {
                              MessageId = m.MessageId,
                              MessageDetail = m.MessageDetail,
                              Subject = m.Subject,
                              SendDate = m.SendDate,
                              SenderEmail = m.SenderEmail,
                              SenderName = sender != null ? sender.Name : "Unknown",
                              SenderSurname = sender != null ? sender.Surname : "User",
                              CategoryName = category != null ? category.CategoryName : "Unknown Category",
                              IsRead = m.IsRead,
                          }).ToList();
            return View(values);
        }
        public async Task<IActionResult> Sendbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var values = (from m in _context.Messages
                          join u in _context.Users
                          on m.ReceiverEmail equals u.Email into userGroup
                          from receiver in userGroup.DefaultIfEmpty()

                          join c in _context.Categories
                          on m.CategoryId equals c.CategoryId into categoryGroup
                          from category in categoryGroup.DefaultIfEmpty()

                          where m.SenderEmail == user.Email
                          select new MessageWithReceiverInfoViewModel
                          {
                              MessageId = m.MessageId,
                              MessageDetail = m.MessageDetail,
                              Subject = m.Subject,
                              SendDate = m.SendDate,
                              ReceiverEmail = m.ReceiverEmail,
                              ReceiverName = receiver != null ? receiver.Name : "Unknown",
                              ReceiverSurname = receiver != null ? receiver.Surname : "User",
                              CategoryName = category != null ? category.CategoryName : "Unknown Category",
                              IsRead = m.IsRead,
                          }).ToList();
            return View(values);
        }
        public async Task<IActionResult> MessageDetail(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var values = await (from m in _context.Messages
                                join u in _context.Users
                                on m.SenderEmail equals u.Email into userGroup
                                from sender in userGroup.DefaultIfEmpty()

                                join c in _context.Categories
                                on m.CategoryId equals c.CategoryId into categoryGroup
                                from category in categoryGroup.DefaultIfEmpty()

                                where m.MessageId == id
                                && m.ReceiverEmail == user.Email

                                select new MessageWithSenderInfoViewModel
                                {
                                    MessageId = m.MessageId,
                                    MessageDetail = m.MessageDetail,
                                    Subject = m.Subject,
                                    SendDate = m.SendDate,
                                    SenderEmail = m.SenderEmail,
                                    SenderName = sender != null ? sender.Name : "Unknown",
                                    SenderSurname = sender != null ? sender.Surname : "User",
                                    CategoryName = category != null ? category.CategoryName : "Unknown Category"
                                }).FirstOrDefaultAsync();

            if (values == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(x => x.MessageId == id);

            if (message != null && !message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return View(values);
        }

        [HttpGet]
        public IActionResult ComposeMessage()
        {
            return View();
        }


    }
}
