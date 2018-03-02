using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;

namespace HairSalon.Controllers
{
  public class ClientController : Controller
  {

    [HttpGet("/clients")]
    public ActionResult Index()
    {
      List<Client> allClients = Client.GetAll();
      return View(allClients);
    }

    [HttpGet("/clients/new")]
    public ActionResult CreateForm()
    {
      return View();
    }

    [HttpPost("/clients")]
    public ActionResult Create()
    {
      Client newClient = new Client(Request.Form["client-name"]);
      newClient.Save();
      return RedirectToAction("Success", "Home");
    }

    [HttpGet("/clients/{id}/delete")]
    public ActionResult DeleteOne(int id)
    {
      Client thisClient = Client.Find(id);
      thisClient.Delete();
      return RedirectToAction("index");
    }

    [HttpGet("/clients/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Client selectedClient = Client.Find(id);
      List<Stylist> clientStylists = selectedClient.GetStylists();
      List<Stylist> allStylists = Stylist.GetAll();
      model.Add("client", selectedClient);
      model.Add("clientStylists", clientStylists);
      model.Add("allStylists", allStylists);
      return View( model);
    }

    [HttpGet("/clients/{id}/update")]
    public ActionResult UpdateForm(int id)
    {
      Client thisClient = Client.Find(id);
      return View("update", thisClient);
    }

    [HttpPost("/clients/{id}/update")]
    public ActionResult Update(int id)
    {
      Client thisClient = Client.Find(id);
      thisClient.Edit(Request.Form["newname"]);
      return RedirectToAction("Index");
    }

    [HttpPost("/clients/{clientId}/stylists/new")]
    public ActionResult AddStylist(int clientId)
    {
      Client client = Client.Find(clientId);
      Stylist stylist = Stylist.Find(Int32.Parse(Request.Form["stylist-id"]));
      client.AddStylist(stylist);
      return RedirectToAction("Success", "Home");
    }
  }
}
