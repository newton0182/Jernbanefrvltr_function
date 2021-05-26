using System;
using System.Collections.Generic;
using System.Text;

namespace Jernbanefrvltr_function
{
    public class Faultreport
    {

        public int ID { get; set; }
        public int TraintrackID { get; set; }
        public int EquipmentID { get; set; }
        public DateTime Datoogtid { get; set; }

        public string Banenummer { get; set; }
        public string Lokation { get; set; }
        public string Banetype { get; set; }
        public string Modelnummer { get; set; }
        public string Udstyrstype { get; set; }
        public string Producent { get; set; }
        public string Medarbejder { get; set; }


    }
}
