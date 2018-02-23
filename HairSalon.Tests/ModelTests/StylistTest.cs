using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Models;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Tests
{
  [TestClass]
  public class StylistTest: IDisposable
  {
    public StylistTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=jamie_pittak_test;";
    }
    public void Dispose()
    {
      Client.DeleteAll();
      Stylist.DeleteAll();
    }
    [TestMethod]
    public void GetClients_RetrievesAllClientsWithStylist_ClientList()
    {
      Stylist testStylist = new Stylist("Gina Allen");
      testStylist.Save();

      Client firstClient = new Client("Jamie Pittak", testStylist.GetId());
      firstClient.Save();
      Client secondClient = new Client("Nick Pittak", testStylist.GetId());
      secondClient.Save();


      List<Client> testClientList = new List<Client> {firstClient, secondClient};
      List<Client> resultClientList = testStylist.GetClients();

      CollectionAssert.AreEqual(testClientList, resultClientList);
    }
    [TestMethod]
       public void GetAll_StylistsEmptyAtFirst_0()
       {
         //Arrange, Act
         int result = Stylist.GetAll().Count;

         //Assert
         Assert.AreEqual(0, result);
       }

      [TestMethod]
      public void Equals_ReturnsTrueForSameName_Stylist()
      {
        //Arrange, Act
        Stylist firstStylist = new Stylist("Gina Allen");
        Stylist secondStylist = new Stylist("Gina Allen");

        //Assert
        Assert.AreEqual(firstStylist, secondStylist);
      }

      [TestMethod]
      public void Save_SavesStylistToDatabase_StylistList()
      {
        //Arrange
        Stylist testStylist = new Stylist("Gina Allen");
        testStylist.Save();

        //Act
        List<Stylist> result = Stylist.GetAll();
        List<Stylist> testList = new List<Stylist>{testStylist};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }


     [TestMethod]
     public void Save_DatabaseAssignsIdToStylist_Id()
     {
       //Arrange
       Stylist testStylist = new Stylist("Gina Allen");
       testStylist.Save();

       //Act
       Stylist savedStylist = Stylist.GetAll()[0];

       int result = savedStylist.GetId();
       int testId = testStylist.GetId();

       //Assert
       Assert.AreEqual(testId, result);
    }


    [TestMethod]
    public void Find_FindsStylistInDatabase_Stylist()
    {
      //Arrange
      Stylist testStylist = new Stylist("Gina Allen");
      testStylist.Save();

      //Act
      Stylist foundStylist = Stylist.Find(testStylist.GetId());

      //Assert
      Assert.AreEqual(testStylist, foundStylist);
    }
  }
}
