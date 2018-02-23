using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System.Collections.Generic;
using System;

namespace HairSalon.Controllers
{
    public class ClientController : Controller
    {

        [HttpGet("/stylists/{stylistId}/clients/new")]
        public ActionResult CreateForm(int stylistId)
        {
          Dictionary<string, object> model = new Dictionary<string, object>();
          Stylist stylist = Stylist.Find(stylistId);
          return View(stylist);
        }

        [HttpGet("/stylists/{stylistId}/clients/{clientId}")]
        public ActionResult Details(int stylistId, int clientId)
        {
          Client client = Client.Find(clientId);
          Dictionary<string, object> model = new Dictionary<string, object>();
          Stylist stylist = Stylist.Find(stylistId);
          model.Add("client", client);
          model.Add("stylist", stylist);
          return View("details",stylist);
        }

        // [HttpGet("/clients")]
        // public ActionResult Index()
        // {
        //     List<Client> allClients = Client.GetAll();
        //     return View(allClients);
        // }
        //
        // [HttpGet("/clients/new")]
        // public ActionResult CreateForm()
        // {
        //     return View();
        // }
        [HttpPost("/stylists/{stylistId}")]
        public ActionResult Create()
        {
          Client newClient = new Client (Request.Form["client-name"],Int32.Parse(Request.Form["stylist-id"]));
          newClient.Save();
          List<Client> allClients = Client.GetAll();
          return RedirectToAction("details");
        }
        // [HttpPost("/clients/delete")]
        // public ActionResult DeleteAll(int id)
        // {
        //   Client thisClient = Client.Find(id);
        //   thisClient.Delete();
        //   return RedirectToAction("Index");
        // }
//         [HttpGet("/clients/{id}/delete")]
//         public ActionResult DeleteOne(int id)
//         {
//           Client thisClient = Client.Find(id);
//           thisClient.Delete();
//           return RedirectToAction("details");
//         }
// //need?
//         [HttpGet("/clients/{id}")]
//         public ActionResult Details(int id)
//         {
//             Client client = Client.Find(id);
//             return View(client);
//         }
//         [HttpGet("/clients/{id}/update")]
//         public ActionResult UpdateForm(int id)
//         {
//             Client thisClient = Client.Find(id);
//             return View("update", thisClient);
//         }
//         [HttpPost("/clients/{id}/update")]
//         public ActionResult Update(int id)
//         {
//           Client thisClient = Client.Find(id);
//           thisClient.Edit(Request.Form["newname"]);
//           return RedirectToAction("Index");
//         }
    }
}
