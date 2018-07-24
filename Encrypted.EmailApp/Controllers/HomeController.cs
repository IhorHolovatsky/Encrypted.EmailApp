using System.Security.Cryptography;
using System.Threading.Tasks;
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
        [Route("messages/send")]
        public async Task<ActionResult> SendMessage(EmailMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SendMessageIndex", model);
            }

            var savedMessage = await _emailMessageService.SendMessageAsync(new EmailMessage
            {
                Message = model.Message,
                Subject = model.Subject
            }, model.EncryptionKey);

            return await ViewMessage(savedMessage.MessageId);
        }


        [Route("messages")]
        public async Task<ActionResult> ViewMessagesIndex()
        {
            var model = new ViewMessagesIndexModel(_emailMessageService);
            await model.BuildAsync();

            return View(model);
        }

        [Route("messages/{messageId:int:min(0)}")]
        public async Task<ActionResult> ViewMessage(int? messageId)
        {
            if (!messageId.HasValue)
                return RedirectToAction("ViewMessagesIndex");

            var model = new ViewMessageIndexModel(_emailMessageService);
            await model.BuildAsync(messageId.Value);

            return View("ViewMessage", model);
        }

        [HttpPost]
        [Route("messages/decrypt")]
        public async Task<ActionResult> DecryptMessage(EmailMessageViewModel message)
        {
            if (!message.MessageId.HasValue)
                return HttpNotFound();

            var model = new ViewMessageIndexModel(_emailMessageService);
            await model.BuildAsync(message.MessageId.Value);

            if (!string.IsNullOrEmpty(message.EncryptionKey))
            {
                try
                {
                    model.Message.Message = await _emailMessageService.DecryptMessageAsync(
                        model.Message.Message,
                        message.EncryptionKey);
                }
                catch (CryptographicException)
                {
                    ModelState.AddModelError($"{nameof(message)}.{nameof(message.EncryptionKey)}", "Invalid encryption key!");
                }
            }

            return View("ViewMessage", model);
        }
    }
}