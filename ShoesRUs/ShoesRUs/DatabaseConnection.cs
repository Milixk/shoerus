using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShoesRUs
{
    class DatabaseConnection
    {
        //Local path of the Shoes R Us Database
        public static readonly string dbconnect = "Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source = " + Application.StartupPath + @"\ShoesRUsDB.accdb";      
    }
}
