using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Media;
using muzeum_v3.ViewModels.Location;
using System.Data.Linq.SqlClient;

namespace muzeum_v3.Models
{
    public class LocationQuery
    {
        public bool hasError = false;
        public string errorMessage;

        public MyObservableCollection<Location> SuperQuery(string locationName)
        {
            hasError = false;
            MyObservableCollection<Location> locations_ObservableCollection = new MyObservableCollection<Location>();
            List<SqlLocation> locations_List = new List<SqlLocation>();

            LinqDataContext connection = new LinqDataContext();
            connection.Connection.Open();

            try
            {
                locations_List = (from e in connection.Lokalizacjas
                                 where SqlMethods.Like(e.nazwa_lokalizacji, "%" + locationName + "%")
                                  select new SqlLocation(
                                       e.id_lokalizacji,
                                       e.nazwa_lokalizacji,
                                       e.opis_lokalizacji))
                                       .ToList();
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

            foreach (SqlLocation e in locations_List)
            {
                locations_ObservableCollection.Add(e.SqlLocation2Location());
            }

            return locations_ObservableCollection;
        }

        public MyObservableCollection<Location> GetLocations()
        {
            hasError = false;
            MyObservableCollection<Location> locations = new MyObservableCollection<Location>();
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("GetLocations", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    SqlLocation sqlLocation = new SqlLocation(
                        (int)reader["id_lokalizacji"],
                        (string)reader["nazwa_lokalizacji"],
                        (string)reader["opis_lokalizacji"]);
                    locations.Add(sqlLocation.SqlLocation2Location());
                }
            }
            catch (SqlException ex)
            {
                errorMessage = "GetLocations SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "GetLocations error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return locations;
        }

        public bool UpdateLocation(Location displayP)
        {
            SqlLocation p = new SqlLocation(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("UpdateLocation", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id_lokalizacji", SqlDbType.Int, 4);
                cmd.Parameters["@id_lokalizacji"].Value = p.LocationId;
                cmd.Parameters.Add("@nazwa_lokalizacji", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_lokalizacji"].Value = p.LocationName;
                cmd.Parameters.Add("@opis_lokalizacji", SqlDbType.VarChar, 150);
                cmd.Parameters["@opis_lokalizacji"].Value = p.Description;
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

        public bool AddLocation(Location displayP)
        {
            SqlLocation p = new SqlLocation(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("AddLocation", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nazwa_lokalizacji", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_lokalizacji"].Value = p.LocationName;
                cmd.Parameters.Add("@opis_lokalizacji", SqlDbType.VarChar, 150);
                cmd.Parameters["@opis_lokalizacji"].Value = p.Description;
                cmd.Parameters.Add("@id_lokalizacji", SqlDbType.Int, 4);
                cmd.Parameters["@id_lokalizacji"].Value = p.LocationId;
                cmd.Parameters["@id_lokalizacji"].Direction = ParameterDirection.Output;
                int rows = cmd.ExecuteNonQuery();                       //Tworzy nowy eksponat w DB
                p.LocationId = (int)cmd.Parameters["@id_lokalizacji"].Value;  //zwraca ustalone id nowego eksponatu do nowego obiektul
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
