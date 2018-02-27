using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Models
{
  public class Client
  {
    private string _clientName;
    private int _stylistId;
    private int _id;

    public Client (string ClientName, int StylistId = 0, int Id = 0)
    {
      _clientName = ClientName;
      _stylistId = StylistId;
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
        Client newClient = (Client) otherClient;
        bool idEquality = (this.GetId() == newClient.GetId());
        bool clientNameEquality = (this.GetClientName() == newClient.GetClientName());
        bool stylistEquality = this.GetStylistId() == newClient.GetStylistId();
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
    public int GetStylistId()
    {
        return _stylistId;
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
        int clientStylistId = rdr.GetInt32(2);

        Client newClient = new Client(clientName, clientStylistId, clientId);
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
      int clientStylistId = 0;

      while (rdr.Read())
      {
          clientId = rdr.GetInt32(0);
          clientName = rdr.GetString(1);
          clientStylistId = rdr.GetInt32(2);
      }

      Client foundClient= new Client(clientName, clientStylistId, clientId);

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
    cmd.CommandText = @"DELETE FROM clients WHERE id = @thisId;";

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
     cmd.CommandText = @"INSERT INTO `clients` (`client_name`,`stylist_id`) VALUES (@Client_Name, @Stylist_Id);";

     MySqlParameter clientName = new MySqlParameter();
     clientName.ParameterName = "@Client_Name";
     clientName.Value = this._clientName;
     cmd.Parameters.Add(clientName);

     MySqlParameter stylistId = new MySqlParameter();
     stylistId.ParameterName = "@Stylist_Id";
     stylistId.Value = this._stylistId;
     cmd.Parameters.Add(stylistId);

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
  }
}
