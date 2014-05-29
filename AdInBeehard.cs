using System;
using System.DirectoryServices.AccountManagement;
using System.Data;
using System.IO;

namespace MarkerAddFromAd
{
    public class AdInBeehard
    {
        public string _domainName;
        public DataTable _tblMarkers;
        public Beehard _beehard;
        StreamWriter _logAdd;

        public AdInBeehard()
        {
            _beehard = new Beehard();
            _tblMarkers = _beehard.GetMarkers();
            Console.WriteLine("Markers loaded");
            _domainName = "kar-tel.local";
            _logAdd = new StreamWriter("logAdd.txt");
            

        }

        public void PrintNonStandartMarkers()
        {
            foreach (DataRow row in _tblMarkers.Rows)
            {
                string mrk = row[1].ToString();

                int sym1 = mrk.IndexOf('@');
                int sym2 = mrk.IndexOf(';');
                if (sym1 == -1)
                    Console.WriteLine(mrk);
            }
        }
        public void AddMarkersFromAd()
        {
            PrincipalContext users = new PrincipalContext(ContextType.Domain, "kar-tel.local");
            PrincipalSearcher usr = new PrincipalSearcher(new UserPrincipal(users));
            PrincipalSearchResult<Principal> psr = usr.FindAll();
            Console.WriteLine("UserPrincipal loaded");
            foreach (UserPrincipal p in psr)
            {
                Start(p.DisplayName, p.SamAccountName, p.Description);
            }
            _logAdd.Close();
        }


        private string CreateMarkerName(string DisplayName, string SamAccountName, string Description)
        {
            string name = "";

            name = SamAccountName + "@kar-tel.local;" + Description + ";" + DisplayName;
            if (name.Length < 100)
                return name;

            name = SamAccountName + "@kar-tel.local;" + DisplayName;
                return name;
        }

        public bool IsContain(string SamAccountName)
        {
            if (SamAccountName.Contains("_"))
                return true;

            DataRow[] rContMarkers = _tblMarkers.Select("name like '" + SamAccountName + "@%'");
            if (rContMarkers.Length > 0)
                return true;

            rContMarkers = _tblMarkers.Select("name like '" + SamAccountName + ";%'");
            if (rContMarkers.Length > 0)
                return true;
            
            Console.WriteLine(SamAccountName);
            return false;

        }
        public void Start(string DisplayName, string SamAccountName, string Description)
        {

            if (SamAccountName == null || SamAccountName.Trim() == "")
                return;

            if (IsContain(SamAccountName) == true)
                return;

            string name = CreateMarkerName(DisplayName, SamAccountName, Description);
            _beehard.CreateMarker(name);
            _logAdd.WriteLine(name);

        }
    }
}
