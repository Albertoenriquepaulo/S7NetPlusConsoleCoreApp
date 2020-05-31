using System;
using System.Collections.Generic;
using System.Text;

namespace S7NetPlusConsoleCoreApp
{
    class Test
    {
        //public byte BlindPercentLRoom3 { get; set; }
        public byte BlindPercentLRoomO { get; set; } //SINT en el PLC. Este y el de abajo corresponden a SINT, se deben leer en pareja cuando se lee desde la clase
        public byte BlindPercentLRoomE { get; set; } //SINT en el PLC. DB42.DBB9
        public float TempRoom1SP { get; set; } //DWORD en el PLC
        public float TempRoom2SP { get; set; } //DWORD en el PLC
        public float TempRoom3SP { get; set; } //DWORD en el PLC
        public float TempRoom4SP { get; set; } //DWORD en el PLC
        public byte[] TempPasilloSP { get; set; } = new byte[4]; //DWORD en el PLC
        public byte[] TempArribaSP { get; set; } = new byte[4]; //DWORD en el PLC
        public bool ActivarCaldera { get; set; } //BOOL en el PLC.
        public bool HabilitarHMI { get; set; } //BOOL en el PLC.
        public bool AlbertoRecibir1 { get; set; }  //BOOL en el PLC. DB42.DBX34.2
        public ushort AlbertoRecibir2 { get; set; } //INT en el PLC -> DB42.DBW36 
        public byte[] AlbertoRecibir3 { get; set; } = new byte[2]; //Date en el PLC
        public byte[] AlbertoRecibir4Bytes { get; set; } = new byte[12]; //DTL en el PLC
        public byte[] AlbertoRecibir5 { get; set; } = new byte[256]; // STRING en el PLC
        public byte[] AlbertoRecibir6 { get; set; } = new byte[4]; //TIME en el PLC
        public byte[] AlbertoRecibir7 { get; set; } = new byte[4]; //Time_Of_Day en el PLC.
        public ushort AlbertoRecibir8 { get; set; } //INT en el PLC. DB42.DBW316
        public float AlbertoRecibir9 { get; set; } //REAL en el PLC. DB42.DBW318 , puede ser double, pero doublecompleta con muchos decimales

        public byte AlbertoRecibir10 { get; set; }
        public byte AlbertoRecibir11 { get; set; }

        public char CharAlbertoRecibir11
        {
            get
            {
                return Convert.ToChar(AlbertoRecibir11);
            }
        }

        public string AlbertoRecibir4
        {
            get
            {
                return XPlc.FromDTLToDate(AlbertoRecibir4Bytes);
            }
        }
    }
}
