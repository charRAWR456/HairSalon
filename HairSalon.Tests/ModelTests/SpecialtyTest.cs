using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Models;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Tests
{
  [TestClass]
  public class SpecialtyTest: IDisposable
  {
    public SpecialtyTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=hair_salon_test;";
    }
    public void Dispose()
    {
      Stylist.DeleteAll();
      Specialty.DeleteAll();
    }
    [TestMethod]
    public void Equals_TrueForSameDescription_Specialty()
    {
      //Arrange, Act
      Specialty firstSpecialty = new Specialty("Color");
      Specialty secondSpecialty = new Specialty("Color");

      //Assert
      Assert.AreEqual(firstSpecialty, secondSpecialty);
    }
    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Specialty.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }
    [TestMethod]
    public void Save_SpecialtySavesToDatabase_SpecialtyList()
    {
      //Arrange
      Specialty testSpecialty = new Specialty("Color");
      testSpecialty.Save();

      //Act
      List<Specialty> result = Specialty.GetAll();
      List<Specialty> testList = new List<Specialty>{testSpecialty};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      Specialty testSpecialty = new Specialty("Color");
      testSpecialty.Save();

      //Act
      Specialty savedSpecialty = Specialty.GetAll()[0];

      int result = savedSpecialty.GetId();
      int testId = testSpecialty.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Delete_DeletesSpecialtyAssociationsFromDatabase_SpecialtyList()
    {
      //Arrange
      Stylist testStylist = new Stylist("Jim");
      testStylist.Save();

      string testDescription = "color";
      Specialty testSpecialty = new Specialty(testDescription);
      testSpecialty.Save();

      //Act
      testSpecialty.AddStylist(testStylist);
      testSpecialty.Delete();

      List<Specialty> resultStylistSpecialtys = testStylist.GetSpecialtys();
      List<Specialty> testStylistSpecialtys = new List<Specialty> {};

      //Assert
      CollectionAssert.AreEqual(testStylistSpecialtys, resultStylistSpecialtys);
    }
    [TestMethod]
    public void Find_FindsSpecialtyInDatabase_Specialty()
    {
      //Arrange
      Specialty testSpecialty = new Specialty("color");
      testSpecialty.Save();

      //Act
      Specialty result = Specialty.Find(testSpecialty.GetId());

      //Assert
      Assert.AreEqual(testSpecialty, result);
    }
    [TestMethod]
    public void AddStylist_AddsStylistToSpecialty_StylistList()
    {
      //Arrange
      Specialty testSpecialty = new Specialty("stylist");
      testSpecialty.Save();

      Stylist testStylist = new Stylist("Joe");
      testStylist.Save();

      //Act
      testSpecialty.AddStylist(testStylist);

      List<Stylist> result = testSpecialty.GetStylists();
      List<Stylist> testList = new List<Stylist>{testStylist};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void GetStylists_ReturnsAllSpecialtyStylists_StylistList()
    {
      //Arrange
      Specialty testSpecialty = new Specialty("color");
      testSpecialty.Save();

      Stylist testStylist1 = new Stylist("Sam");
      testStylist1.Save();

      Stylist testStylist2 = new Stylist("Taylor");
      testStylist2.Save();

      //Act
      testSpecialty.AddStylist(testStylist1);
      List<Stylist> result = testSpecialty.GetStylists();
      List<Stylist> testList = new List<Stylist> {testStylist1};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
  }
}
