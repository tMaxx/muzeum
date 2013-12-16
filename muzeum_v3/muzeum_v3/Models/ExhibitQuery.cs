using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Media;
using muzeum_v3.Models;
using muzeum_v3.ViewModels.Exhibit;

namespace muzeum_v3.Models
{
    public class ExhibitQuery
    {
        public bool hasError = false;
        public string errorMessage;

        public MyObservableCollection<Exhibit> GetExhibits()
        {
            hasError = false;
            MyObservableCollection<Exhibit> exhibits = new MyObservableCollection<Exhibit>();
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("GetExhibits", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    SqlExhibit sqlExhibit = new SqlExhibit(
                        (int)reader["id_eksponatu"],
                        (string)reader["nazwa_eksponatu"],
                        (string)reader["opis_eksponatu"],
                        (string)reader["nazwa_autora"],
                        (string)reader["nazwa_wlasciciela"]);
                    exhibits.Add(sqlExhibit.SqlExhibit2Exhibit());
                } 
            }
            catch (SqlException ex)
            {
                errorMessage = "GetExhibits SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "GetExhibits error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return exhibits;
        }

        public MyObservableCollection<Exhibit> GetExhibitsForExposition(int idExp)
        {
            hasError = false;
            MyObservableCollection<Exhibit> Exhibits = new MyObservableCollection<Exhibit>();
            try
            {
                string queryString =
                "SELECT E.id_eksponatu,E.nazwa_eksponatu,E.opis_eksponatu,(Select nazwa_autora from Autor where id_autora = E.id_autora) as nazwa_autora," +
                "(Select nazwa_wlasciciela from Wlasciciel where id_wlasciciela = E.id_wlasciciela) as nazwa_wlasciciela from dbo.Eksponat E " +
                "where  (Select id_przypisania from Przypisanie where id_eksponatu = E.id_eksponatu) IN (Select id_przypisania from Przypisanie where id_ekspozycji = @idExp)";

                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand(queryString, DataBaseManager.Instance.Connection);
                cmd.Parameters.AddWithValue("@idExp", idExp);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    SqlExhibit sqlExhibit = new SqlExhibit(
                         (int)reader["id_eksponatu"],
                        (string)reader["nazwa_eksponatu"],
                        (string)reader["opis_eksponatu"],
                        (string)reader["nazwa_autora"],
                        (string)reader["nazwa_wlasciciela"]);
                    Exhibits.Add(sqlExhibit.SqlExhibit2Exhibit());
                }
            }
            catch (SqlException ex)
            {
                errorMessage = "GetExhibitsInLocation SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "GetExhibitsInLocation error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return Exhibits;
        }
       
        public bool UpdateExhibit(Exhibit displayP)
        {
            SqlExhibit p = new SqlExhibit(displayP);
            hasError = false;
            try
            {
            DataBaseManager.Instance.openConnetion();
            SqlCommand cmd = new SqlCommand("UpdateExhibit", DataBaseManager.Instance.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@id_eksponatu", SqlDbType.Int, 4);
            cmd.Parameters["@id_eksponatu"].Value = p.ExhibitId;
            cmd.Parameters.Add("@nazwa_eksponatu", SqlDbType.VarChar, 50);
            cmd.Parameters["@nazwa_eksponatu"].Value = p.ExhibitName;
            cmd.Parameters.Add("@opis_eksponatu", SqlDbType.VarChar, 150);
            cmd.Parameters["@opis_eksponatu"].Value = p.Description;
            cmd.Parameters.Add("@autor_eksponatu", SqlDbType.VarChar, 50);
            cmd.Parameters["@autor_eksponatu"].Value = p.Author;
            cmd.Parameters.Add("@wlasciciel_eksponatu", SqlDbType.VarChar, 50);
            cmd.Parameters["@wlasciciel_eksponatu"].Value = p.Owner;
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

        public bool DeleteExhibit(int exhibitId)
        {
            hasError = false;
            try
            {
            DataBaseManager.Instance.openConnetion();
            SqlCommand cmd = new SqlCommand("DeleteExhibit",DataBaseManager.Instance.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@id_eksponatu", SqlDbType.Int, 4);
            cmd.Parameters["@id_eksponatu"].Value = exhibitId;
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
    
        public bool AddExhibit(Exhibit displayP)
        {
            SqlExhibit p = new SqlExhibit(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("AddExhibit", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nazwa_eksponatu", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_eksponatu"].Value = p.ExhibitName;
                cmd.Parameters.Add("@opis_eksponatu", SqlDbType.VarChar, 150);
                cmd.Parameters["@opis_eksponatu"].Value = p.Description;
                cmd.Parameters.Add("@autor_eksponatu", SqlDbType.VarChar, 50);
                cmd.Parameters["@autor_eksponatu"].Value = p.Author;
                cmd.Parameters.Add("@wlasciciel_eksponatu", SqlDbType.VarChar, 50);
                cmd.Parameters["@wlasciciel_eksponatu"].Value = p.Owner;
                cmd.Parameters.Add("@id_eksponatu", SqlDbType.Int, 4);
                cmd.Parameters["@id_eksponatu"].Value = p.ExhibitId;
                cmd.Parameters["@id_eksponatu"].Direction = ParameterDirection.Output;
                int rows = cmd.ExecuteNonQuery();                       //Tworzy nowy eksponat w DB
                p.ExhibitId = (int)cmd.Parameters["@id_eksponatu"].Value;  //zwraca ustalone id nowego eksponatu do nowego obiektul
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
