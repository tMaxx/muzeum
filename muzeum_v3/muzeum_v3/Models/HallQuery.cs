using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Media;
using muzeum_v3.ViewModels.Hall ;
using System.Data.Linq.SqlClient;

namespace muzeum_v3.Models
{
    public class HallQuery
    {
        public bool hasError = false;
        public string errorMessage;


        public MyObservableCollection<Hall> SuperQuery(string HallName, string locationName)
        {
            hasError = false;
            MyObservableCollection<Hall> halls_ObservableCollection = new MyObservableCollection<Hall>();
            List<SqlHall> halls_List = new List<SqlHall>();

            LinqDataContext connection = new LinqDataContext();
            connection.Connection.Open();

            try
            {
                halls_List = (from e in connection.Salas
                                 where SqlMethods.Like(e.nazwa_sali, "%" + HallName + "%")
                                 && SqlMethods.Like(e.Lokalizacja.nazwa_lokalizacji, "%" + locationName + "%")
                              select new SqlHall(
                                       e.id_sali,
                                       e.Lokalizacja.nazwa_lokalizacji,
                                       e.nazwa_sali,
                                       e.opis_sali)).ToList();
            }
            catch (SqlException ex)
            {
                errorMessage = "SuperQuery SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "SuperQuery error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                connection.Connection.Close();
            }

            foreach (SqlHall e in halls_List)
            {
                halls_ObservableCollection.Add(e.SqlHall2Hall());
            }

            return halls_ObservableCollection;
        }

        public MyObservableCollection<Hall> GetHalls()
        {
            hasError = false;
            MyObservableCollection<Hall> halls = new MyObservableCollection<Hall>();
            try
            {

                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("GetHalls", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SqlHall sqlHall = new SqlHall(
                        (int)reader["id_sali"],
                        (string)reader["nazwa_lokalizacji"],
                        (string)reader["nazwa_sali"],
                        (string)reader["opis_sali"]);
                    halls.Add(sqlHall.SqlHall2Hall());
                }
            }
            catch (SqlException ex)
            {
                errorMessage = "GetHalls SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "GetHalls error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return halls;
        }

        public MyObservableCollection<Hall> GetHallsInLocation(string locationName)
        {
            hasError = false;
            MyObservableCollection<Hall> halls = new MyObservableCollection<Hall>();
            try
            {
                string queryString =
                "SELECT * from dbo.Sala " + 
                "Where id_lokalizacji = " +
                "(Select L.id_lokalizacji from Lokalizacja L Where L.nazwa_lokalizacji = @locationName)";
             
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand(queryString,  DataBaseManager.Instance.Connection);
                cmd.Parameters.AddWithValue("@locationName", locationName);
           
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    SqlHall sqlHall = new SqlHall(
                        (int)reader["id_sali"],
                        locationName,
                        (string)reader["nazwa_sali"],
                        (string)reader["opis_sali"]);
                    halls.Add(sqlHall.SqlHall2Hall());
                }
            }
            catch (SqlException ex)
            {
                errorMessage = "GetHallsInLocation SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "GetHallsInLocation error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return halls;
        }

        public bool UpdateHall(Hall displayP)
        {
            SqlHall p = new SqlHall(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("UpdateHall", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id_sali", SqlDbType.Int, 4);
                cmd.Parameters["@id_sali"].Value = p.HallId;
                cmd.Parameters.Add("@nazwa_lokalizacji", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_lokalizacji"].Value = p.LocationName;
                cmd.Parameters.Add("@nazwa_sali", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_sali"].Value = p.HallName;
                cmd.Parameters.Add("@opis_sali", SqlDbType.VarChar, 150);
                cmd.Parameters["@opis_sali"].Value = p.Description;
                int rows = 0;
                rows = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                errorMessage = "Update SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "Update error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return (!hasError);
        }

        public bool AddHall(Hall displayP)
        {
            SqlHall p = new SqlHall(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("AddHall", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nazwa_lokalizacji", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_lokalizacji"].Value = p.LocationName;
                cmd.Parameters.Add("@nazwa_sali", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_sali"].Value = p.HallName;
                cmd.Parameters.Add("@opis_sali", SqlDbType.VarChar, 150);
                cmd.Parameters["@opis_sali"].Value = p.Description;
                cmd.Parameters.Add("@id_sali", SqlDbType.Int, 4);
                cmd.Parameters["@id_sali"].Value = p.HallId;
                cmd.Parameters["@id_sali"].Direction = ParameterDirection.Output;
                int rows = cmd.ExecuteNonQuery();                       //Tworzy nowy eksponat w DB
                p.HallId = (int)cmd.Parameters["@id_sali"].Value;  //zwraca ustalone id nowego eksponatu do nowego obiektul
                displayP.toDataBase(p);                            //upadatuje odpowiedni Eksponat
            }
            catch (SqlException ex)
            {
                errorMessage = "Add SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "ADD error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return !hasError;
        } 

    }
}
