using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Windows.Media;
using muzeum_v3.Models;
using muzeum_v3.ViewModels.Presentation;

namespace muzeum_v3.Models
{
    public class LinqPresentation
    {
        public bool hasError = false;
        public string errorMessage;

        public MyObservableCollection<Presentation> GetPresentations(int exhibitId)
        {
            hasError = false;

            MyObservableCollection<Presentation> presentations_ObservableCollection = new MyObservableCollection<Presentation>();
            List<SqlPresentation> presentations_List = new List<SqlPresentation>();

            LinqDataContext connection = new LinqDataContext();
            connection.Connection.Open();

            try
            {
                presentations_List = (from p in connection.Prezentacjes
                                      where p.id_eksponatu == exhibitId
                                      select
                                      new SqlPresentation(
                                          p.id_prezentacji,
                                          p.data_rozpoczecia,
                                          p.data_zakonczenia,
                                          p.Eksponat.nazwa_eksponatu,
                                          p.Ekspozycja.nazwa_ekspozycji,
                                          p.Sala.Lokalizacja.nazwa_lokalizacji,
                                          p.Sala.nazwa_sali))
                                          .ToList();
            }
            catch (SqlException ex)
            {
                errorMessage = "GetPresentations SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                errorMessage = "GetPresentations error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                connection.Connection.Close();
            }

            foreach (SqlPresentation p in presentations_List)
            {
                presentations_ObservableCollection.Add(p.SqlPresentation2Presentation());
            }

            return presentations_ObservableCollection;
        }

        public bool UpdatePresentation(Presentation displayP)
        {

            hasError = false;

            LinqDataContext connection = new LinqDataContext();
            connection.Connection.Open();
            System.Data.Common.DbTransaction transaction = connection.Connection.BeginTransaction();
            connection.Transaction = transaction;

            connection.Transaction = transaction;
            try
            {
                var presentation = (from p in connection.Prezentacjes
                                    where p.id_prezentacji == displayP.PresentationId
                                    select p);
                var item = presentation.Single();

                item.id_eksponatu = (from e in connection.Eksponats
                                             where e.nazwa_eksponatu == displayP.PresentedExhibit
                                             select e.id_eksponatu).SingleOrDefault();

                item.id_ekspozycji = (from e in connection.Ekspozycjas
                                              where e.nazwa_ekspozycji == displayP.Exposition
                                              select e.id_ekspozycji).SingleOrDefault();

                item.id_sali = (from e in connection.Salas
                                        where e.nazwa_sali == displayP.Hall
                                        select e.id_sali).SingleOrDefault();

                item.data_rozpoczecia = Convert.ToDateTime(displayP.DateOfBegin);
                item.data_zakonczenia = Convert.ToDateTime(displayP.DateOfEnd);


                connection.SubmitChanges();

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                errorMessage = "Update SQL error, " + ex.Message;
                hasError = true;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                errorMessage = "Update error, " + ex.Message;
                hasError = true;
            }
            finally
            {
                connection.Connection.Close();
            }
            return (!hasError);
        }
        public bool AddPresentation(Presentation displayP)
        {
            SqlPresentation p = new SqlPresentation(displayP);
            hasError = false;
            try
            {
               
                LinqDataContext connetion = new LinqDataContext();
                int? newPresentationId = 0;
                connetion.AddPresentation(p.DateOfBegin,p.DateOfEnd,p.PresentedExhibit,p.Exposition,p.Hall, ref newPresentationId);
                p.PresentationId = (int)newPresentationId;
                displayP.toDataBase(p);
            }
            catch (Exception ex)
            {
                errorMessage = "ADD error, " + ex.Message;
                hasError = true;
            }
            
            return !hasError;
        }
        public bool DeletePresentation(int id)
        {
           
            hasError = false;
            try
            {
                LinqDataContext connetion = new LinqDataContext();

                var deleteItem =
                from item in connetion.Prezentacjes
                where item.id_prezentacji == id
                select item;

            if (deleteItem != null)
            {
                connetion.Prezentacjes.DeleteOnSubmit(deleteItem.First());
            }
                connetion.SubmitChanges();

            }
            catch (Exception ex)
            {
                errorMessage = "ADD error, " + ex.Message;
                hasError = true;
            }

            return !hasError;

        }
    }
}
