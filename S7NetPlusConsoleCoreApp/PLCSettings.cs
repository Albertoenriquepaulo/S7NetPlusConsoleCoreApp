﻿using System;
using System.Collections.Generic;
using System.Text;
using S7.Net;

namespace S7NetPlusConsoleCoreApp
{
    static class PLCSettings
    {
        //static public int WriteTimeout { get; set; }
        //static public int ReadTimeout { get; set; }
        //static public short MaxPDUSize { get; set; }
        static public short Slot { get; set; } = 1;
        static public short Rack { get; set; } = 0;
        static public CpuType CPU { get; set; } = CpuType.S71200;
        static public int Port { get; set; } = 4000;
        static public string IP { get; set; } = "192.168.100.65";
        //static public bool IsAvailable { get; }
        //static public bool IsConnected { get; }
    }
}
