using System;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using HairSalon;

namespace HairSalon.Models
{
    public class DB
    {
        public static MySqlConnection Connection()
        {
            MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
            return conn;
        }
    }
}
