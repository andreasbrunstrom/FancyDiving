using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace ReadExcelFile
{
    class ReadFile
    {
        public void read()
        {
            var fileName = string.Format("{0}Hoppkoder.xlsx", Directory.GetCurrentDirectory());
            var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", fileName);
            var adapter = new OleDbDataAdapter("SELECT * FROM [Blad1$]", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds, "Koder");
            var data = ds.Tables["Koder"].AsEnumerable();


        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ReadFile rf = new ReadFile();
            rf.read();
            Console.ReadKey();
        }
    }
}
