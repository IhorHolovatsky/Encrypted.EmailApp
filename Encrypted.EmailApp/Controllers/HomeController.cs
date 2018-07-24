using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using EmailApp.Domain;
using Encrypted.EmailApp.Models;
using Encrypted.EmailApp.Services.Interfaces;
using Encrypted.EmailApp.Utils;

namespace Encrypted.EmailApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailMessageService _emailMessageService;

        public HomeController(IEmailMessageService emailMessageService)
        {
            _emailMessageService = emailMessageService;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [Route("messages/send")]
        public ActionResult SendMessageIndex()
        {
            var model = new EmailMessageViewModel
            {
                ToUserName = Request.QueryString.Get("t"),
                EncryptionKey = Extensions.GetUniqueKey(8)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SendMessage(EmailMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SendMessageIndex", model);
            }

            await _emailMessageService.SendMessageAsync(new EmailMessage
            {
                Message = model.Message,
                Subject = model.Subject
            }, model.EncryptionKey);

            return await ViewMessage(0);
        }


        [Route("messages")]
        public async Task<ActionResult> ViewMessagesIndex()
        {
            var model = new ViewMessagesIndexModel(_emailMessageService);
            await model.BuildAsync();

            return View(model);
        }

        public async Task<ActionResult> ViewMessage(int? messageId)
        {
            if (!messageId.HasValue)
                return RedirectToAction("ViewMessagesIndex");

            var model = new ViewMessageIndexModel(_emailMessageService);
            await model.BuildAsync(messageId.Value);

            return View("ViewMessage", model);
        }

        [HttpPost]
        public async Task<ActionResult> ViewMessage(EmailMessageViewModel message)
        {
            if (!message.MessageId.HasValue)
                return HttpNotFound();

            var model = new ViewMessageIndexModel(_emailMessageService);
            await model.BuildAsync(message.MessageId.Value);

            if (!string.IsNullOrEmpty(message.EncryptionKey))
            {
                model.Message.Message = await _emailMessageService.DecryptMessageAsync(
                    model.Message.Message,
                    message.EncryptionKey);
            }

            return View("ViewMessage", model);
        }
    }
}