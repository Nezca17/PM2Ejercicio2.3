using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace PM2Ejercicio2.3.Models
{
    public class Audio
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string date { get; set; }
    }
}
