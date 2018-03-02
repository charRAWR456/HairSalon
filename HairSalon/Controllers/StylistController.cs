using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System.Collections.Generic;
using System;

namespace HairSalon.Controllers
{
  public class StylistController : Controller
  {

    [HttpGet("/stylists")]
public ActionResult Index()
{
  List<Stylist> allStylists = Stylist.GetAll();
  return View(allStylists);
}

[HttpGet("/stylists/new/{specialty}")]
public ActionResult CreateForm(int specialty)
{
  return View(specialty);
}

[HttpPost("/stylists")]
public ActionResult Create()
{
  Stylist newStylist = new Stylist(Request.Form["stylist-name"]);
  newStylist.Save();
  return RedirectToAction("Success", "Home");
}

[HttpGet("/stylists/{id}")]
public ActionResult Details(int id)
{
  Dictionary<string, object> model = new Dictionary<string, object>();
  Stylist selectedStylist = Stylist.Find(id);
  List<Client> stylistClients = selectedStylist.GetClients();
  List<Client> allClients = Client.GetAll();
  model.Add("stylist", selectedStylist);
  model.Add("stylistClients", stylistClients);
  model.Add("allClients", allClients);
  return View(model);
}

[HttpPost("/stylists/{specialist}")]
public ActionResult CreateWithSpecialist(int specialist)
{
  Stylist newStylist = new Stylist(Request.Form["stylist-name"]);
  newStylist.Save();
  Specialty mySpecialty = Specialty.Find(specialist);
  mySpecialty.AddStylist(newStylist);
  return RedirectToAction("Success", "Home");
}

[HttpGet("/stylists/{id}/clients/new")]
public ActionResult CreateClientForm()
{
  return View("~/Views/Clients/CreateClientForm.cshtml");
}

[HttpPost("/stylists/{stylistId}/clients/new")]
public ActionResult AddClient(int stylistId)
{
  Stylist stylist = Stylist.Find(stylistId);
  Client client = Client.Find(Int32.Parse(Request.Form["client-id"]));
  stylist.AddClient(client);
  return RedirectToAction("Success", "Home");
}

[HttpGet("/stylists/{stylistId}/update")]
public ActionResult UpdateForm(int stylistId)
{
  Stylist thisStylist = Stylist.Find(stylistId);
  return View("update", thisStylist);
}

[HttpPost("/stylists/{stylistId}/update")]
public ActionResult Update(int stylistId)
{
  Stylist thisStylist = Stylist.Find(stylistId);
  thisStylist.Edit(Request.Form["newname"]);
  return RedirectToAction("Index");
}

[HttpGet("/stylists/{stylistid}/delete")]
public ActionResult DeleteOne(int stylistId)
{
  Stylist thisStylist = Stylist.Find(stylistId);
  thisStylist.Delete();
  return RedirectToAction("index");
}
}
}
