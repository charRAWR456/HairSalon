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
    public void Delete_DeletesStylistAssociationsFromDatabase_StylistList()
    {
      //Arrange
      Client testClient = new Client("Jim");
      testClient.Save();

      string testName = "Tony";
      Stylist testStylist = new Stylist(testName);
      testStylist.Save();

      //Act
      testStylist.AddClient(testClient);
      testStylist.Delete();

      List<Stylist> resultClientStylists = testClient.GetStylists();
      List<Stylist> testClientStylists = new List<Stylist> {};

      //Assert
      CollectionAssert.AreEqual(testClientStylists, resultClientStylists);
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
    [TestMethod]
    public void GetClients_ReturnsAllStylistClients_ClientList()
  {
    //Arrange
    Stylist testStylist = new Stylist("Joe");
    testStylist.Save();

    Client testClient1 = new Client("Sally");
    testClient1.Save();

    Client testClient2 = new Client("Molly");
    testClient2.Save();

    //Act
    testStylist.AddClient(testClient1);
    List<Client> savedClients = testStylist.GetClients();
    List<Client> testList = new List<Client> {testClient1};

    //Assert
    CollectionAssert.AreEqual(testList, savedClients);
  }
  [TestMethod]
    public void Test_AddClient_AddsClientToStylist()
    {
      //Arrange
      Stylist testStylist = new Stylist("Carl");
      testStylist.Save();

      Client testClient = new Client("Howard");
      testClient.Save();

      Client testClient2 = new Client("Phil");
      testClient2.Save();

      //Act
      testStylist.AddClient(testClient);
      testStylist.AddClient(testClient2);

      List<Client> result = testStylist.GetClients();
      List<Client> testList = new List<Client>{testClient, testClient2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
  }
}
