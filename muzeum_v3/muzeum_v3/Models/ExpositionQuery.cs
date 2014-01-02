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
using System.Data.Linq.SqlClient;


namespace muzeum_v3.Models
{
    public class ExpositionQuery
    {
        public bool hasError = false;
        public string errorMessage;

        public MyObservableCollection<Exposition> SuperQuery(string expositionName, string org, string location,
            int numberOfTicketsFROM, int numberOfTicketsTO, decimal profitFROM, decimal profitTO)
        {
            hasError = false;
            MyObservableCollection<Exposition> expositions_ObservableCollection = new MyObservableCollection<Exposition>();
            List<SqlExposition> expositions_List = new List<SqlExposition>();

            LinqDataContext connection = new LinqDataContext();
            connection.Connection.Open();
            List<int> sale = new List<int>();
            try
            {
                sale = (from s in connection.Sprzedazs
                        group s by s.id_ekspozycji into g
                        where
                        (g.Count() >= numberOfTicketsFROM && g.Count() <= numberOfTicketsTO)
                        && (g.Sum(x => x.Bilet.cena_biletu) >= profitFROM && (g.Sum(x => x.Bilet.cena_biletu) <= profitTO))
                        select new
                        {
                            idExp = g.Key,
                        }.idExp).ToList();


                expositions_List =
                    (from e in connection.Ekspozycjas
                     where SqlMethods.Like(e.nazwa_ekspozycji, "%" + expositionName + "%")
                        && SqlMethods.Like(e.Organizator.nazwa_organizatora, "%" + org + "%")
                        && SqlMethods.Like(e.Lokalizacja.nazwa_lokalizacji, "%" + location + "%")
                     select new SqlExposition(
                     e.id_ekspozycji,
                     e.nazwa_ekspozycji,
                     e.Organizator.nazwa_organizatora,
                     e.Lokalizacja.nazwa_lokalizacji,
                     e.opis_ekspozycji,
                     (from S in connection.Sprzedazs
                      where
                        e.id_ekspozycji == S.id_ekspozycji
                      select new
                      {
                          S
                      }).Count(),

                       (from S in connection.Sprzedazs
                        where
                          S.id_ekspozycji == e.id_ekspozycji
                        select new
                        {
                            cena_biletu = (decimal)S.Bilet.cena_biletu
                        }).Sum(p => p.cena_biletu))
                         ).ToList();
                
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

            foreach (SqlExposition e in expositions_List)
            {
                foreach (int d in sale)
                {
                    if (d == e.ExpositionId)
                    {
                        expositions_ObservableCollection.Add(e.SqlExposition2Exposition());
                        break;
                    }
                }
            }

            return expositions_ObservableCollection;
        }

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

        internal MyObservableCollection<Exposition> GetExpositionsForOrg(int p)
        {
            hasError = false;
            MyObservableCollection<Exposition> expositions = new MyObservableCollection<Exposition>();
            try
            {
                string queryString =
                "SELECT " +
                "E.id_ekspozycji, " +
                "E.nazwa_ekspozycji, " +
                "O.nazwa_organizatora, " +
                "L.nazwa_lokalizacji, " +
                "E.opis_ekspozycji, " +
                "(SELECT COUNT(*) FROM Sprzedaz S WHERE E.id_ekspozycji= id_ekspozycji) as sprzedane_bilety, " +
                "(SELECT ISNULL(SUM(B.cena_biletu),0) FROM Sprzedaz S " +
                "Join dbo.Bilet As B ON S.id_biletu = B.id_biletu " +
                "Where S.id_ekspozycji = E.id_ekspozycji) AS zysk " +
                "From dbo.Ekspozycja AS E " +
                "JOIN dbo.Lokalizacja AS L " +
                    "ON E.id_lokalizacji = L.id_lokalizacji " +
                "JOIN dbo.Organizator AS O " +
                    "ON E.id_organizatora = O.id_organizatora where O.id_organizatora = @p";
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand(queryString, DataBaseManager.Instance.Connection);
                cmd.Parameters.AddWithValue("@p", p);

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

        internal MyObservableCollection<Exposition> GetExpositionsForLocation(int p)
        {
            hasError = false;
            MyObservableCollection<Exposition> expositions = new MyObservableCollection<Exposition>();
            try
            {
                string queryString =
                "SELECT " +
                "E.id_ekspozycji, " +
                "E.nazwa_ekspozycji, " +
                "O.nazwa_organizatora, " +
                "L.nazwa_lokalizacji, " +
                "E.opis_ekspozycji, " +
                "(SELECT COUNT(*) FROM Sprzedaz S WHERE E.id_ekspozycji= id_ekspozycji) as sprzedane_bilety, " +
                "(SELECT ISNULL(SUM(B.cena_biletu),0) FROM Sprzedaz S " +
                "Join dbo.Bilet As B ON S.id_biletu = B.id_biletu " +
                "Where S.id_ekspozycji = E.id_ekspozycji) AS zysk " +
                "From dbo.Ekspozycja AS E " +
                "JOIN dbo.Organizator AS O " +
                    "ON E.id_organizatora = O.id_organizatora " +
                "JOIN dbo.Lokalizacja AS L " +
                    "ON E.id_lokalizacji = L.id_lokalizacji where  L.id_lokalizacji = @p";
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand(queryString, DataBaseManager.Instance.Connection);
                cmd.Parameters.AddWithValue("@p", p);

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
    }
}
