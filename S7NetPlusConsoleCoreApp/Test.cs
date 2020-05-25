using System;
using System.Collections.Generic;
using System.Text;

namespace S7NetPlusConsoleCoreApp
{
    class Test
    {
        public UInt32 SPTempArriba { get; set; }
        public bool ActivarCaldera { get; set; }
        public bool HabilitarHMI { get; set; }
        public bool AlbertoRecibir1 { get; set; }  //DB42.DBX34.2
        public ushort AlbertoRecibir2 { get; set; } //DB42.DBW36
        public byte[] AlbertoRecibir3 { get; set; } = new byte[2];
        public byte[] AlbertoRecibir4 { get; set; } = new byte[12];
        public byte[] AlbertoRecibir5 { get; set; } = new byte[256];
        public byte[] AlbertoRecibir6 { get; set; } = new byte[4];
        public byte[] AlbertoRecibir7 { get; set; } = new byte[4];
        public ushort AlbertoRecibir8 { get; set; } //DB42.DBW316

        //public byte[] AlbertoRecibir9 { get; set; } = new byte[4]; //DB42.DBW318
        public float AlbertoRecibir9 { get; set; } //DB42.DBW318 , puede ser double, pero doublecompleta con muchos decimales

        public byte AlbertoRecibir10 { get; set; }
        public byte AlbertoRecibir11 { get; set; }

        public char CharAlbertoRecibir11
        {
            get
            {
                return Convert.ToChar(AlbertoRecibir11);
            }
        }




        //public string AlbertoRecibir5 { get; set; }
        //public DateTime AlbertoRecibir6 { get; set; }
        //public DateTime AlbertoRecibir7 { get; set; }
        //public int AlbertoRecibir8 { get; set; }
        //public double AlbertoRecibir9 { get; set; }
        //public byte[] AlbertoRecibir10 { get; set; }
        //public char AlbertoRecibir11 { get; set; }
    }
}
