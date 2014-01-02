using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Media;
using muzeum_v3.Models;
using muzeum_v3.ViewModels.Exhibit;
using System.Xml.Linq;
using System.Data.Linq;
using System.Data.Linq.SqlClient;


namespace muzeum_v3.Models
{
    class SimpleXML
    {
        static string fileName = "eksponaty.xml";
        
        static public void CreateXmlFile()
        {
            LinqDataContext muzeum = new LinqDataContext();
           
            XDocument xml = new XDocument(
                new XComment("Data from the muzeum database"),
                new XElement("Eksponaty",
                    from exhibit in muzeum.Eksponats
                    select
                        new XElement("Eksponat",
                            new XElement("id_eksponatu", exhibit.id_eksponatu),
                            new XElement("nazwa_eksponatu", exhibit.nazwa_eksponatu),
                            new XElement("autor", exhibit.Autor.nazwa_autora),
                            new XElement("wlasciciel", exhibit.Wlasciciel.nazwa_wlasciciela),
                            new XElement("opis", exhibit.opis_eksponatu))));

            StreamWriter writer = File.CreateText(fileName);
            writer.Write(xml.ToString());
            writer.Close();
        }

        static public void AddExhibitToXml(Exhibit e)
        {
            XDocument xml = XDocument.Load(fileName);
            
            XElement toDeleteElement =
                xml.Element("Eksponaty").Elements("Eksponat").Where(
                    exhibit => exhibit.Element("nazwa_eksponatu").Value.Equals(e.ExhibitName))
                .FirstOrDefault();
            if (toDeleteElement != null)
            {
                toDeleteElement.Remove();
            }
            xml.Element("Eksponaty").Add(
                        new XElement("Eksponat",
                            new XElement("id_eksponatu", e.ExhibitId),
                            new XElement("nazwa_eksponatu", e.ExhibitName),
                            new XElement("autor", e.Author),
                            new XElement("wlasciciel", e.Owner),
                            new XElement("opis", e.Description)));
                
           
            xml.Save(fileName);
        }

        static public void DeleteExhibitToXml(int eID)
        {
            XDocument xml = XDocument.Load(fileName);

            XElement toDeleteElement =
                xml.Element("Eksponaty")
                .Elements("Eksponat")
                .First(exhibit => (int)exhibit.Element("id_eksponatu") == eID);
                
            if (toDeleteElement != null)
            {
                toDeleteElement.Remove();
            }
            xml.Save(fileName);
        }

        static public void UpdateExhibitToXml(Exhibit e)
        {
            XDocument xml = XDocument.Load(fileName);

            XElement exhibitDetails =
                xml.Element("Eksponaty")
                .Elements("Eksponat")
                .First(exhibit => (int)exhibit.Element("id_eksponatu") == e.ExhibitId);
            
            if (exhibitDetails != null)
            {
                exhibitDetails.Element("nazwa_eksponatu").Value = e.ExhibitName;
                exhibitDetails.Element("autor").Value = e.Author;
                exhibitDetails.Element("wlasciciel").Value = e.Owner;
                exhibitDetails.Element("opis").Value = e.Description;
            }
            xml.Save(fileName);
        }
    }
}
