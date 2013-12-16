using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Media;
using muzeum_v3.Models;
using muzeum_v3.ViewModels.Owner ;

namespace muzeum_v3.Models
{
    public class OwnerQuery
    {
        public bool hasError = false;
        public string errorMessage;

        public MyObservableCollection<Owner> GetOwners()
        {
            hasError = false;
            MyObservableCollection<Owner> owners = new MyObservableCollection<Owner>();
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("GetOwners", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    SqlOwner sqlOwner = new SqlOwner(
                        (int)reader["id_wlasciciela"],
                        (string)reader["nazwa_wlasciciela"],
                        (string)reader["miasto_wlasciciela"],
                        (string)reader["kraj_wlasciciela"],
                        (string)reader["email_wlasciciela"],
                        (string)reader["telefon_wlasciciela"]);
                    owners.Add(sqlOwner.SqlOwner2Owner());
                }
            }
            catch (SqlException ex)
            {
                errorMessage = "GetOwners SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "GetOwners error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return owners;
        }

        public bool AddOwner(Owner displayP)
        {
            SqlOwner p = new SqlOwner(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("AddOwner", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nazwa_wlasciciela", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_wlasciciela"].Value = p.OwnerName;
                cmd.Parameters.Add("@miasto_wlasciciela", SqlDbType.VarChar, 50);
                cmd.Parameters["@miasto_wlasciciela"].Value = p.City;
                cmd.Parameters.Add("@kraj_wlasciciela", SqlDbType.VarChar, 50);
                cmd.Parameters["@kraj_wlasciciela"].Value = p.Country;
                cmd.Parameters.Add("@email_wlasciciela", SqlDbType.VarChar, 50);
                cmd.Parameters["@email_wlasciciela"].Value = p.Email;
                cmd.Parameters.Add("@telefon_wlasciciela", SqlDbType.VarChar, 50);
                cmd.Parameters["@telefon_wlasciciela"].Value = p.PhoneNumber;
                cmd.Parameters.Add("@id_wlasciciela", SqlDbType.Int, 4);
                cmd.Parameters["@id_wlasciciela"].Value = p.OwnerId;
                cmd.Parameters["@id_wlasciciela"].Direction = ParameterDirection.Output;
                int rows = cmd.ExecuteNonQuery();                       //Tworzy nowy eksponat w DB
                p.OwnerId = (int)cmd.Parameters["@id_wlasciciela"].Value;  
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

        public bool UpdateOwner(Owner displayP)
        {
            SqlOwner p = new SqlOwner(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("UpdateOwner", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id_wlasciciela", SqlDbType.Int, 4);
                cmd.Parameters["@id_wlasciciela"].Value = p.OwnerId;
                cmd.Parameters.Add("@nazwa_wlasciciela", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_wlasciciela"].Value = p.OwnerName;
                cmd.Parameters.Add("@miasto_wlasciciela", SqlDbType.VarChar, 50);
                cmd.Parameters["@miasto_wlasciciela"].Value = p.City;
                cmd.Parameters.Add("@kraj_wlasciciela", SqlDbType.VarChar, 50);
                cmd.Parameters["@kraj_wlasciciela"].Value = p.Country;
                cmd.Parameters.Add("@email_wlasciciela", SqlDbType.VarChar, 50);
                cmd.Parameters["@email_wlasciciela"].Value = p.Email;
                cmd.Parameters.Add("@telefon_wlasciciela", SqlDbType.VarChar, 50);
                cmd.Parameters["@telefon_wlasciciela"].Value = p.PhoneNumber;
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
    }
}
