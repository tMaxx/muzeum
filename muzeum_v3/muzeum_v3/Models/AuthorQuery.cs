using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Media;
using muzeum_v3.Models;
using muzeum_v3.ViewModels.Author;

namespace muzeum_v3.Models
{
    public class AuthorQuery
    {
        public bool hasError = false;
        public string errorMessage;

        public MyObservableCollection<Author> GetAuthors()
        {
            hasError = false;
            MyObservableCollection<Author> authors = new MyObservableCollection<Author>();
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("GetAuthors", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    SqlAuthor sqlAuthor = new SqlAuthor(
                        (int)reader["id_autora"],
                        (string)reader["nazwa_autora"],
                        (DateTime)reader["data_urodzenia"],
                        (DateTime)reader["data_smierci"],
                        (string)reader["opis_autora"]);
                    authors.Add(sqlAuthor.SqlAuthor2Author());
                }
            }
            catch (SqlException ex)
            {
                errorMessage = "GetAuthors SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "GetAuthors error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return authors;
        }

        public bool UpdateAuthor(Author displayP)
        {
            SqlAuthor p = new SqlAuthor(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("UpdateAuthor", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id_autora", SqlDbType.Int, 4);
                cmd.Parameters["@id_autora"].Value = p.AuthorId;
                cmd.Parameters.Add("@nazwa_autora", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_autora"].Value = p.AuthorName;
                cmd.Parameters.Add("@opis_autora", SqlDbType.VarChar, 150);
                cmd.Parameters["@opis_autora"].Value = p.Description;
                cmd.Parameters.Add("@data_urodzenia", SqlDbType.DateTime, 50);
                cmd.Parameters["@data_urodzenia"].Value = p.DateOfBirth;
                cmd.Parameters.Add("@data_smierci", SqlDbType.DateTime, 50);
                cmd.Parameters["@data_smierci"].Value = p.DateOfDeath;
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
        public bool AddAuthor(Author displayP)
        {
            SqlAuthor p = new SqlAuthor(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("AddAuthor", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nazwa_autora", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_autora"].Value = p.AuthorName;
                cmd.Parameters.Add("@opis_autora", SqlDbType.VarChar, 150);
                cmd.Parameters["@opis_autora"].Value = p.Description;
                cmd.Parameters.Add("@data_urodzenia", SqlDbType.DateTime, 50);
                cmd.Parameters["@data_urodzenia"].Value = p.DateOfBirth;
                cmd.Parameters.Add("@data_smierci", SqlDbType.DateTime, 50);
                cmd.Parameters["@data_smierci"].Value = p.DateOfDeath;
                cmd.Parameters.Add("@id_autora", SqlDbType.Int, 4);
                cmd.Parameters["@id_autora"].Value = p.AuthorId;
                cmd.Parameters["@id_autora"].Direction = ParameterDirection.Output;
                int rows = cmd.ExecuteNonQuery();                       //Tworzy nowy eksponat w DB
                p.AuthorId = (int)cmd.Parameters["@id_autora"].Value;  //zwraca ustalone id nowego eksponatu do nowego obiektul
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
