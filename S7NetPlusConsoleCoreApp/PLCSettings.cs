using System;
using System.Collections.Generic;
using System.Text;
using S7.Net;

namespace S7NetPlusConsoleCoreApp
{
    public class PLCSettings
    {


        //static public int WriteTimeout { get; set; }
        //static public int ReadTimeout { get; set; }
        //static public short MaxPDUSize { get; set; }
        public CpuType CPU { get; set; }// = CpuType.S71200;
        public string IP { get; set; }// = "192.168.100.65";
        public int Port { get; set; }// = 4000;
        public short Slot { get; set; }// = 1;
        public short Rack { get; set; }// = 0;
        //static public bool IsAvailable { get; }
        //static public bool IsConnected { get; }

        public PLCSettings(CpuType cpu, string ip, int port, short slot, short rack)
        {
            CPU = cpu;
            Slot = slot;
            Rack = rack;
            Port = port;
            IP = ip;
        }
    }
}
