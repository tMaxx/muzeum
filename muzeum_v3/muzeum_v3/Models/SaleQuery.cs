using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Media;
using muzeum_v3.Models;
using muzeum_v3.ViewModels.Sale;
using muzeum_v3.ViewModels.Ticket;

namespace muzeum_v3.Models
{
    public class SaleQuery
    {
        public bool hasError = false;
        public string errorMessage;

        public MyObservableCollection<Ticket> GetTickets()
        {
            hasError = false;
            MyObservableCollection<Ticket> Tickets = new MyObservableCollection<Ticket>();
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("GetTickets", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    SqlTicket sqlTicket = new SqlTicket(
                        (int)reader["id_biletu"],
                        (decimal)reader["cena_biletu"],
                        (string)reader["nazwa_biletu"]);
                    Tickets.Add(sqlTicket.SqlTicket2Ticket());
                }
            }
            catch (SqlException ex)
            {
                errorMessage = "GetTickets SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "GetTickets error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                DataBaseManager.Instance.closeConnetion();
            }
            return Tickets;
        }
       
        public bool AddSale(Sale displayP)
        {
            SqlSale p = new SqlSale(displayP);
            hasError = false;
            try
            {
                DataBaseManager.Instance.openConnetion();
                SqlCommand cmd = new SqlCommand("AddSale", DataBaseManager.Instance.Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nazwa_biletu", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_biletu"].Value = p.NameOfTicket;
                cmd.Parameters.Add("@cena_biletu", SqlDbType.Decimal, 8);
                cmd.Parameters["@cena_biletu"].Value = p.PriceOfTicket;
                cmd.Parameters.Add("@nazwa_ekspozycji", SqlDbType.VarChar, 50);
                cmd.Parameters["@nazwa_ekspozycji"].Value = p.ExpositionName;
                cmd.Parameters.Add("@id_sprzedazy", SqlDbType.Int, 4);
                cmd.Parameters["@id_sprzedazy"].Value = p.SaleId;
                cmd.Parameters["@id_sprzedazy"].Direction = ParameterDirection.Output;
                int rows = cmd.ExecuteNonQuery();                       //Tworzy nowy eksponat w DB
                p.SaleId = (int)cmd.Parameters["@id_sprzedazy"].Value;  //zwraca ustalone id nowego eksponatu do nowego obiektul
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
