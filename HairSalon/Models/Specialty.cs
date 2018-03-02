using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Models
{
  public class Specialty
  {
    private string _description;
    private int _id;

    public Specialty(string description, int id = 0)
    {

      _id = id;
      _description = description;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM specialties;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherSpecialty)
    {
      if (!(otherSpecialty is Specialty))
      {
        return false;
      }
      else
      {
        Specialty newSpecialty = (Specialty) otherSpecialty;
        return this.GetId().Equals(newSpecialty.GetId());
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public string GetDescription()
    {
      return _description;
    }

    public int GetId()
    {
      return _id;
    }

    public static List<Specialty> GetAll()
    {
      List<Specialty> allSpecialtys = new List<Specialty>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM specialties;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int specialtyId = rdr.GetInt32(0);
        string specialtyName = rdr.GetString(1);
        Specialty newSpecialty = new Specialty(specialtyName, specialtyId);
        allSpecialtys.Add(newSpecialty);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allSpecialtys;
    }

    public static Specialty Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM specialties WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int specialtyId = 0;
      string specialtyName = "";

      while(rdr.Read())
      {
        specialtyId = rdr.GetInt32(0);
        specialtyName = rdr.GetString(1);
      }

      Specialty newSpecialty = new Specialty(specialtyName, specialtyId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return newSpecialty;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO specialties (description) VALUES (@description);";

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@description";
      description.Value = this._description;
      cmd.Parameters.Add(description);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();

      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Stylist> GetStylists()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT stylist_id FROM specialties_stylists WHERE specialty_id = @specialtyId;";

      MySqlParameter specialtyIdParameter = new MySqlParameter();
      specialtyIdParameter.ParameterName = "@specialtyId";
      specialtyIdParameter.Value = _id;
      cmd.Parameters.Add(specialtyIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<int> stylistIds = new List<int> {};
      while(rdr.Read())
      {
        int stylistId = rdr.GetInt32(0);
        stylistIds.Add(stylistId);
      }
      rdr.Dispose();

      List<Stylist> stylists = new List<Stylist> {};
      foreach (int stylistId in stylistIds)
      {
        var stylistQuery = conn.CreateCommand() as MySqlCommand;
        stylistQuery.CommandText = @"SELECT * FROM stylists WHERE id = @StylistId;";

        MySqlParameter stylistIdParameter = new MySqlParameter();
        stylistIdParameter.ParameterName = "@StylistId";
        stylistIdParameter.Value = stylistId;
        stylistQuery.Parameters.Add(stylistIdParameter);

        var stylistQueryRdr = stylistQuery.ExecuteReader() as MySqlDataReader;
        while(stylistQueryRdr.Read())
        {
          int thisStylistId = stylistQueryRdr.GetInt32(0);
          string stylistName = stylistQueryRdr.GetString(1);

          Stylist foundStylist = new Stylist(stylistName, thisStylistId);
          stylists.Add(foundStylist);
        }
        stylistQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return stylists;
    }

    public void AddStylist(Stylist newStylist)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO specialties_stylists (stylist_id, specialty_id) VALUES (@StylistId, @SpecialtyId);";

      MySqlParameter stylist_id = new MySqlParameter();
      stylist_id.ParameterName = "@StylistId";
      stylist_id.Value = newStylist.GetId();
      cmd.Parameters.Add(stylist_id);

      MySqlParameter specialty_id = new MySqlParameter();
      specialty_id.ParameterName = "@SpecialtyId";
      specialty_id.Value = _id;
      cmd.Parameters.Add(specialty_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM specialties WHERE id = @specialtyId; DELETE FROM specialties_stylists WHERE specialty_id = @specialtyId;";

      MySqlParameter specialtyIdParameter = new MySqlParameter();
      specialtyIdParameter.ParameterName = "@specialtyId";
      specialtyIdParameter.Value = this.GetId();
      cmd.Parameters.Add(specialtyIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Edit(string newDescription)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE specialties SET description = @newDescription WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@newDescription";
      description.Value = newDescription;
      cmd.Parameters.Add(description);

      cmd.ExecuteNonQuery();
      _description = newDescription;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

    }
  }
}
