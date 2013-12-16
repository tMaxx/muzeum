using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muzeum_v3.Models
{
    class DataBaseManager
    {
        private static readonly DataBaseManager instance = new DataBaseManager("muzeum_v4");
        public System.Data.SqlClient.SqlConnection Connection { get; set; }
        public static DataBaseManager Instance
        {
            get
            {
                return instance;
            }
        }
        DataBaseManager(string name)
        {
            setConnetion(name);
        }
        public void setConnetion(string name)
        {
            Connection = new System.Data.SqlClient.SqlConnection(
            "Data Source=(local);Initial Catalog=" + name + ";Integrated Security=SSPI");
        }
        public void openConnetion()
        {
            if (Connection != null)
                Connection.Open();
        }
        public void closeConnetion()
        {
            Connection.Close();
        }



    }
}
