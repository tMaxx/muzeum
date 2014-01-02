using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Media;
using muzeum_v3.Models;
using muzeum_v3.ViewModels.Org;
using System.Data.Linq.SqlClient;


namespace muzeum_v3.Models
{
    public class OrgQuery
    {
        public bool hasError = false;
        public string errorMessage;

        public MyObservableCollection<Org> SuperQuery(string orgName, string city)
        {
            hasError = false;
            MyObservableCollection<Org> orgs_ObservableCollection = new MyObservableCollection<Org>();
            List<SqlOrg> orgs_List = new List<SqlOrg>();

            LinqDataContext connection = new LinqDataContext();
            connection.Connection.Open();

            try
            {
                orgs_List = (from e in connection.Organizators
                                 where SqlMethods.Like(e.nazwa_organizatora, "%" + orgName + "%")
                                 && SqlMethods.Like(e.miasto_organizatora, "%" + city + "%")
                             select new SqlOrg(
                                       e.id_organizatora,
                                       e.nazwa_organizatora,
                                       e.miasto_organizatora,
                                       e.e_mail_organizatora,
                                       e.telefon_organizatora))
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

            foreach (SqlOrg e in orgs_List)
            {
                orgs_ObservableCollection.Add(e.SqlOrg2Org());
            }

            return orgs_ObservableCollection;
        }
        public MyObservableCollection<Org> GetOrgs()
        {
            hasError = false;
            MyObservableCollection<Org> Orgs = new MyObservableCollection<Org>();
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("GetOrgs", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    SqlOrg sqlOrg = new SqlOrg(
                        (int)reader["id_organizatora"],
                        (string)reader["nazwa_organizatora"],
                        (string)reader["miasto_organizatora"],
                        (string)reader["e_mail_organizatora"],
                        (string)reader["telefon_organizatora"]);
                    Orgs.Add(sqlOrg.SqlOrg2Org());
                }
            }
            catch (SqlException ex)
            {
                errorMessage = "GetOrgs SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "GetOrgs error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return Orgs;
        }

        public bool AddOrg(Org displayP)
        {
            SqlOrg p = new SqlOrg(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("AddOrg", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nazwa_organizatora", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_organizatora"].Value = p.OrgName;
                cmd.Parameters.Add("@miasto_organizatora", SqlDbType.VarChar, 50);
                cmd.Parameters["@miasto_organizatora"].Value = p.City;
                cmd.Parameters.Add("@e_mail_organizatora", SqlDbType.VarChar, 50);
                cmd.Parameters["@e_mail_organizatora"].Value = p.Email;
                cmd.Parameters.Add("@telefon_organizatora", SqlDbType.VarChar, 50);
                cmd.Parameters["@telefon_organizatora"].Value = p.PhoneNumber;
                cmd.Parameters.Add("@id_organizatora", SqlDbType.Int, 4);
                cmd.Parameters["@id_organizatora"].Value = p.OrgId;
                cmd.Parameters["@id_organizatora"].Direction = ParameterDirection.Output;
                int rows = cmd.ExecuteNonQuery();                       //Tworzy nowy eksponat w DB
                p.OrgId = (int)cmd.Parameters["@id_organizatora"].Value;
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

        public bool UpdateOrg(Org displayP)
        {
            SqlOrg p = new SqlOrg(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("UpdateOrg", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id_organizatora", SqlDbType.Int, 4);
                cmd.Parameters["@id_organizatora"].Value = p.OrgId;
                cmd.Parameters.Add("@nazwa_organizatora", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_organizatora"].Value = p.OrgName;
                cmd.Parameters.Add("@miasto_organizatora", SqlDbType.VarChar, 50);
                cmd.Parameters["@miasto_organizatora"].Value = p.City;
                cmd.Parameters.Add("@e_mail_organizatora", SqlDbType.VarChar, 50);
                cmd.Parameters["@e_mail_organizatora"].Value = p.Email;
                cmd.Parameters.Add("@telefon_organizatora", SqlDbType.VarChar, 50);
                cmd.Parameters["@telefon_organizatora"].Value = p.PhoneNumber;
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
