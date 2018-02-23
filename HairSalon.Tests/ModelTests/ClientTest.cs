using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Models;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Tests
{
  [TestClass]
  public class ClientTest: IDisposable
  {
    public ClientTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=jamie_pittak_test;";
    }
    public void Dispose()
    {
      Client.DeleteAll();
      Stylist.DeleteAll();
    }
    [TestMethod]
    public void GetClientName_ReturnsClientName_String()
    {
      //Arrange
      string clientName = "Jamie Pittak";
      Client newClient = new Client(clientName);

      //Act
      string result = newClient.GetClientName();

      //Assert
      Assert.AreEqual(clientName, result);
    }

    [TestMethod]
    public void GetAll_ReturnsClients_ClientList()
    {
      //Arrange
      string clientName1 = "Jamie Pittak";
      string clientName2 = "Nick Pittak";
      Client newClient1 = new Client(clientName1);
      newClient1.Save();
      Client newClient2 = new Client(clientName2);
      newClient2.Save();
      List<Client> newList = new List<Client> { newClient1, newClient2 };

      //Act
      List<Client> result = Client.GetAll();

      //Assert
      CollectionAssert.AreEqual(newList, result);
    }
    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Client.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }
    [TestMethod]
    public void Equals_ReturnsTrueIfClientNamesAreTheSame_Client()
    {
      // Arrange, Act
      Client firstClient = new Client("Jamie Pittak",1);
      Client secondClient = new Client("Jamie Pittak",1);

      // Assert
      Assert.AreEqual(firstClient, secondClient);
    }
    [TestMethod]
    public void Save_SavesToDatabase_ClientList()
    {
      //Arrange
      Client testClient = new Client("Jamie Pittak",1);

      //Act
      testClient.Save();
      List<Client> result = Client.GetAll();
      List<Client> testList = new List<Client>{testClient};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Client testClient = new Client("Jamie Pittak",1);

      //Act
      testClient.Save();
      Client savedClient = Client.GetAll()[0];

      int result = savedClient.GetId();
      int testId = testClient.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindsClientInDatabase_Client()
    {
      //Arrange
      Client testClient = new Client("Jamie Pittak",1);
      testClient.Save();

      //Act
      Client foundClient = Client.Find(testClient.GetId());

      //Assert
      Assert.AreEqual(testClient, foundClient);
    }
    [TestMethod]
    public void Edit_UpdatesClientInDatabase_String()
    {
      //Arrange
      Client testClient = new Client ("Jamie Pittak", 1,1);
      testClient.Save();
      string secondClientName = "Nick Pittak";

      //Act
      testClient.Edit(secondClientName);

      string result = Client.Find(testClient.GetId()).GetClientName();

      //Assert
      Assert.AreEqual(secondClientName , result);
    }
    [TestMethod]
    public void Delete_DeletesClientInDatabase_Void()
    {
      //Arrange
      string firstClientName = "Jamie Pittak";
      Client testClient = new Client(firstClientName,1,2);
      testClient.Save();
      string secondClientName = "Nick Pittak";
      Client testClient2 = new Client(secondClientName,1,3);
      testClient2.Save();
      //Act
      testClient.Delete();
      List<Client> expected = new List<Client> {testClient2};
      List<Client> result = Client.GetAll();

      //Assert
      CollectionAssert.AreEqual(expected, result);
    }
  }
}
