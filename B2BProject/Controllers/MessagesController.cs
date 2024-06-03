using B2BProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2BProject.Controllers
{
    public class MessagesController : Controller
    {
        private B2BDbEntities db = new B2BDbEntities();

        // Satıcıya mesaj gönderme sayfası
        public ActionResult SendMessage()
        {
            return View();
        }

        // Satıcıya mesaj gönderme işlemi
        [HttpPost]
        public ActionResult SendMessage(Messaging message, int recipientId)
        {
        
            int senderId = Convert.ToInt32(Session["UserId"]);
            message.Sender_Id = senderId;
            message.Recipient_Id = recipientId;
            message.Date = DateTime.Now;

            db.Messaging.Add(message);
            db.SaveChanges();

           
            return RedirectToAction("Inbox");
        }

        // Satıcıya gelen mesajları görüntüleme
        public ActionResult Inbox()
        {
            // Gönderilen mesajları al
            int userId = Convert.ToInt32(Session["UserId"]);
            var receivedMessages = db.Messaging
                .Where(m => m.Recipient_Id == userId)
                .OrderByDescending(m => m.Date)
                .ToList();

            return View(receivedMessages);
        }
    }
}
