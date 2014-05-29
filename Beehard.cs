using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace MarkerAddFromAd
{
    public class Beehard
    {
        public DataTable GetMarkers()
        {
            DataTable _usr = new DataTable();
            string select = "SELECT * FROM markers";

            DbDataAdapter adap = FastCreator.CreateConAdapter(select);
            adap.Fill(_usr);

            return _usr;
        }


        public string ToShortMarker(string marker)
        {
            string newmarker = "";
            foreach (char c in marker)
            {
                if (c.Equals('@') || c.Equals(';'))
                    return newmarker;

                newmarker += c;
            }
            return newmarker;
        }

        public void CreateMarker(string name)
        {
           SqlCommand create = (SqlCommand)FastCreator.CreateConCommand("createmarker");
            create.CommandType = CommandType.StoredProcedure;
            create.Parameters.AddWithValue("@name", name);
            create.Connection.Open();
            create.ExecuteNonQuery();
            create.Connection.Close();
        }
    }
}
