using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MvvmFoundation.Wpf;
using muzeum_v3.Models ;
namespace muzeum_v3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static HallQuery hallQuery = new HallQuery();
        public static HallQuery HallQuery { get { return hallQuery; } }

        private static SaleQuery saleQuery = new SaleQuery();
        public static SaleQuery SaleQuery { get { return saleQuery; } }

        private static OrgQuery orgQuery = new OrgQuery();
        public static OrgQuery OrgQuery { get { return orgQuery; } }

        private static ExpositionQuery expositionQuery = new ExpositionQuery();
        public static ExpositionQuery ExpositionQuery { get { return expositionQuery; } }

        private static OwnerQuery ownerQuery = new OwnerQuery();
        public static OwnerQuery OwnerQuery { get { return ownerQuery; } }

        private static ExhibitQuery exhibitQuery = new ExhibitQuery();
        public static ExhibitQuery ExhibitQuery { get { return exhibitQuery; } }

        private static AuthorQuery authorQuery = new AuthorQuery();
        public static AuthorQuery AuthorQuery { get { return authorQuery; } }

        private static LocationQuery locationQuery = new LocationQuery();
        public static LocationQuery LocationQuery { get { return locationQuery; } }

        private static LinqPresentation linqPresentation = new LinqPresentation();
        public static LinqPresentation LinqPresentation { get { return linqPresentation; } }

        internal static Messenger Messenger
        {
            get { return _messenger; }
        }

        readonly static Messenger _messenger = new Messenger();
    }
}
