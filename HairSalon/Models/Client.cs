using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Models
{
  public class Client
  {
    private string _clientName;
    private int _id;

    public Client (string ClientName, int Id = 0)
    {
      _clientName = ClientName;
      _id = Id;
    }
    public override bool Equals(System.Object otherClient)
    {
      if (!(otherClient is Client))
      {
        return false;
      }
      else
      {
//change?
        Client newClient = (Client) otherClient;
        bool idEquality = (this.GetId() == newClient.GetId());
        bool clientNameEquality = (this.GetClientName() == newClient.GetClientName());
        return (idEquality && clientNameEquality);
      }
    }
    public override int GetHashCode()
    {
         return this.GetClientName().GetHashCode();
    }
    public string GetClientName()
    {
      return _clientName;
    }
    public void SetClientName(string newClientName)
    {
      _clientName = newClientName;
    }
    public int GetId()
    {
      return _id;
    }
    public static List<Client> GetAll()
    {
      List<Client> allClients = new List<Client> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM clients;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int clientId = rdr.GetInt32(0);
        string clientName = rdr.GetString(1);

        Client newClient = new Client(clientName, clientId);
        allClients.Add(newClient);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allClients;
    }
    public static Client Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `clients` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int clientId = 0;
      string clientName = "";

      while (rdr.Read())
      {
          clientId = rdr.GetInt32(0);
          clientName = rdr.GetString(1);
      }

      Client foundClient= new Client(clientName, clientId);

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return foundClient;
    }
    public static void DeleteAll()
   {
     MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"DELETE FROM clients;";

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
    cmd.CommandText = @"DELETE FROM clients WHERE id = @thisId;DELETE FROM clients_stylists WHERE client_id = @thisId;";

    MySqlParameter deleteId = new MySqlParameter();
    deleteId.ParameterName = "@thisId";
    deleteId.Value = this.GetId();
    cmd.Parameters.Add(deleteId);

    cmd.ExecuteNonQuery();

    conn.Close();
    if (conn != null)
    {
        conn.Dispose();
    }
  }
   public void Save()
    {
      MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"INSERT INTO `clients` (`client_name`) VALUES (@Client_Name);";

     MySqlParameter clientName = new MySqlParameter();
     clientName.ParameterName = "@Client_Name";
     clientName.Value = this._clientName;
     cmd.Parameters.Add(clientName);

     cmd.ExecuteNonQuery();
     _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public void Edit(string newClientName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE clients SET client_name = @NewClientName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter clientName = new MySqlParameter();
      clientName.ParameterName = "@newClientName";
      clientName.Value = newClientName;
      cmd.Parameters.Add(clientName);

      cmd.ExecuteNonQuery();
      _clientName = newClientName;

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
      cmd.CommandText = @"SELECT stylist_id FROM clients_stylists WHERE client_id = @clientId;";

      MySqlParameter clientIdParameter = new MySqlParameter();
      clientIdParameter.ParameterName = "@clientId";
      clientIdParameter.Value = _id;
      cmd.Parameters.Add(clientIdParameter);

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
      cmd.CommandText = @"INSERT INTO clients_stylists (stylist_id, client_id) VALUES (@StylistId, @PatientId);";

      MySqlParameter stylist_id = new MySqlParameter();
      stylist_id.ParameterName = "@StylistId";
      stylist_id.Value = newStylist.GetId();
      cmd.Parameters.Add(stylist_id);

      MySqlParameter client_id = new MySqlParameter();
      client_id.ParameterName = "@PatientId";
      client_id.Value = _id;
      cmd.Parameters.Add(client_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
