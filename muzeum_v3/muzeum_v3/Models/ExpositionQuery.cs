using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Media;
using muzeum_v3.Models;
using muzeum_v3.ViewModels.Exposition;

namespace muzeum_v3.Models
{
    public class ExpositionQuery
    {
        public bool hasError = false;
        public string errorMessage;

        public MyObservableCollection<Exposition> GetExpositions()
        {
            hasError = false;
            MyObservableCollection<Exposition> expositions = new MyObservableCollection<Exposition>();
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("GetExpositions", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    SqlExposition sqlExposition = new SqlExposition(
                        (int)reader["id_ekspozycji"],
                        (string)reader["nazwa_ekspozycji"],
                        (string)reader["nazwa_organizatora"],
                        (string)reader["nazwa_lokalizacji"],
                        (string)reader["opis_ekspozycji"],
                        (int)reader["sprzedane_bilety"],
                        (decimal)reader["zysk"]);
                    expositions.Add(sqlExposition.SqlExposition2Exposition());
                }
            }
            catch (SqlException ex)
            {
                errorMessage = "GetExpositions SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "GetExpositions error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return expositions;
        }


        public bool UpdateExposition(Exposition displayP)
        {
            SqlExposition p = new SqlExposition(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("UpdateExposition", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id_ekspozycji", SqlDbType.Int, 4);
                cmd.Parameters["@id_ekspozycji"].Value = p.ExpositionId;
                cmd.Parameters.Add("@nazwa_ekspozycji", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_ekspozycji"].Value = p.ExpositionName;
                cmd.Parameters.Add("@nazwa_organizatora", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_organizatora"].Value = p.OrganizerName;
                cmd.Parameters.Add("@nazwa_lokalizacji", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_lokalizacji"].Value = p.LocationName;
                cmd.Parameters.Add("@opis_ekspozycji", SqlDbType.VarChar, 150);
                cmd.Parameters["@opis_ekspozycji"].Value = p.Description;
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

        public bool DeleteExposition(int expositionId)
        {
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("DeleteExposition", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id_ekspozycji", SqlDbType.Int, 4);
                cmd.Parameters["@id_ekspozycji"].Value = expositionId;
                int rows = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                errorMessage = "DELETE SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "DELETE error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return !hasError;
        }

        public bool AddExposition(Exposition displayP)
        {
            SqlExposition p = new SqlExposition(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("AddExposition", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nazwa_ekspozycji", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_ekspozycji"].Value = p.ExpositionName;
                cmd.Parameters.Add("@nazwa_organizatora", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_organizatora"].Value = p.OrganizerName;
                cmd.Parameters.Add("@nazwa_lokalizacji", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_lokalizacji"].Value = p.LocationName;
                cmd.Parameters.Add("@opis_ekspozycji", SqlDbType.VarChar, 150);
                cmd.Parameters["@opis_ekspozycji"].Value = p.Description;
                cmd.Parameters.Add("@id_ekspozycji", SqlDbType.Int, 4);
                cmd.Parameters["@id_ekspozycji"].Value = p.ExpositionId;
                cmd.Parameters["@id_ekspozycji"].Direction = ParameterDirection.Output;
                int rows = cmd.ExecuteNonQuery();                       //Tworzy nowy eksponat w DB
                p.ExpositionId = (int)cmd.Parameters["@id_ekspozycji"].Value;  //zwraca ustalone id nowego eksponatu do nowego obiektul
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
