using S7.Net;
//using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace S7NetPlusConsoleCoreApp
{
    public class XPlc
    {
        private Plc PLC { get; }
        private Single ValueToWriteDWord { get; set; }
        private ushort ValueToWriteWord { get; set; }
        private bool BitValueToWrite { get; set; }
        private int DB { get; set; }
        private int StartByteAdress { get; set; }
        private int ByteSize { get; set; }
        private byte[] ValueToWriteInBytes
        {
            get
            {
                switch (ByteSize)
                {
                    case PlcDataType.Word:
                        byte[] byteArrayWord = BitConverter.GetBytes(ValueToWriteWord);
                        Array.Reverse(byteArrayWord, 0, ByteSize);
                        return byteArrayWord;
                    case PlcDataType.DWord:
                        byte[] byteArrayDWord = BitConverter.GetBytes(ValueToWriteDWord);
                        Array.Reverse(byteArrayDWord, 0, ByteSize);
                        return byteArrayDWord;
                }
                return new byte[0];
            }
        }

        #region Contructor
        public XPlc(Plc plc, int dB = 0, int startByteAdr = 0)
        {
            PLC = plc;
            DB = dB;
            this.StartByteAdress = startByteAdr;

        }
        #endregion

        #region PLC Configuration
        public int GetStartByteAdr()
        {
            return StartByteAdress;
        }
        public void SetStartByteAdr(int value)
        {
            StartByteAdress = value;
        }
        public int GetDataBlock()
        {
            return DB;
        }

        public void SetDataBlock(int value)
        {
            DB = value;
        }
        public void SetDataBlockAndStartByteAdr(int db, int startByteAdr)
        {
            DB = db;
            StartByteAdress = startByteAdr;
        }
        #endregion

        #region PLC Status
        public bool IsConnected()
        {
            return PLC.IsConnected;

        }
        public bool IsAvailable()
        {
            return PLC.IsAvailable;

        }
        #endregion

        #region Manage Connection
        public bool Connect()
        {
            try
            {
                this.PLC.Open();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public bool CloseConnection()
        {
            try
            {
                PLC.Close();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool RestartConnection()
        {
            try
            {
                PLC.Close();
                PLC.Open();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        #endregion

        #region Writing Operations
        public void WriteValue(double value, int byteSize)
        {
            ByteSize = byteSize;
            switch (byteSize)
            {
                case PlcDataType.Word:
                    //ushort intValueToWrite = (ushort)Convert.ToInt16(value);
                    //PLC.Write($"DB{DB}.DBW{StartByteAdress}", intValueToWrite);

                    ushort ValueToWrite1 = (ushort)Convert.ToInt16(value);
                    byte[] byteArray1 = BitConverter.GetBytes(ValueToWrite1);
                    Array.Reverse(byteArray1, 0, ByteSize);
                    // PLC.WriteBytes(DataType.DataBlock, DB, StartByteAdress, byteArray1);

                    ValueToWriteWord = (ushort)(value);
                    PLC.WriteBytes(DataType.DataBlock, DB, StartByteAdress, ValueToWriteInBytes);
                    break;

                case PlcDataType.DWord:
                    ValueToWriteDWord = Convert.ToSingle(value);
                    PLC.WriteBytes(DataType.DataBlock, DB, StartByteAdress, ValueToWriteInBytes);
                    break;
            }
        }

        public void WriteBit(int bitAdr, bool value)
        {
            ByteSize = 1;
            BitValueToWrite = value;
            PLC.WriteBit(DataType.DataBlock, DB, StartByteAdress, bitAdr, BitValueToWrite);
        }
        #endregion

        #region Reading Operations
        public bool ReadBool(string variable)
        {
            return (bool)(this.PLC.Read(variable));//value;
        }
        public bool ReadBool(DataType dataType, int db, int startByteAdr)
        {
            return Convert.ToBoolean(PLC.ReadBytes(dataType, db, startByteAdr, 1)[0]);
        }

        [Obsolete]
        public double ReadDWord(string variable, int decimalNumbers)
        {
            return Math.Round(((uint)PLC.Read(variable)).ConvertToDouble(), decimalNumbers);
        }
        public double ReadDWord(int db, int startByteAdr, int decimalNumbers)
        {
            byte[] valuInBytes = PLC.ReadBytes(DataType.DataBlock, db, startByteAdr, 4);

            ReverseIfIsLittleIndian(valuInBytes);
            double myValue = BitConverter.ToSingle(valuInBytes, 0);

            return Math.Round(myValue, decimalNumbers);
        }
        public int ReadInt(string variable)
        {
            //int test = Int.FromByteArray((byte[])PLC.Read(variable));
            PLC.Open();
            var value = PLC.Read(variable);
            PLC.Close();
            if (Convert.ToInt32(value) == 0)
            {
                return 0;
            }
            return (ushort)value;
        }

        public int ReadBytes(DataType dataType, int db, int startByteAdr, int count)
        {
            byte[] test = PLC.ReadBytes(dataType, db, startByteAdr, count);

            //DWord.FromByteArray(PLC.ReadBytes(dataType, db, startByteAdr, count));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(test);

            int i = BitConverter.ToInt32(test, 0);
            return i;
        }

        public int ReadDInt(string variable)
        {
            return (ushort)PLC.Read(variable);
        }

        public T ReadClass<T>(int db, int startByteAdr = 0) where T : class
        {
            return PLC.ReadClass<T>(db, startByteAdr);
        }
        #endregion

        #region Utilities
        public static string FromDTLToDate(byte[] DTLBytes)
        {
            byte[] yearBytes = new byte[] { DTLBytes[0], DTLBytes[1] };
            byte[] monthBytes = new byte[] { DTLBytes[2] };
            byte[] dayBytes = new byte[] { DTLBytes[3] };
            byte[] dayOfWeekByte = new byte[] { DTLBytes[4] };
            byte[] hourBytes = new byte[] { DTLBytes[5] };
            byte[] minBytes = new byte[] { DTLBytes[6] };
            byte[] secBytes = new byte[] { DTLBytes[7] };
            byte[] nanoSecBytes = new byte[] { DTLBytes[8], DTLBytes[9], DTLBytes[10], DTLBytes[11] };


            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(yearBytes);
                Array.Reverse(monthBytes);
                Array.Reverse(dayBytes);
                Array.Reverse(dayOfWeekByte);
                Array.Reverse(hourBytes);
                Array.Reverse(minBytes);
                Array.Reverse(secBytes);
                Array.Reverse(nanoSecBytes);
            }

            int year = BitConverter.ToInt16(yearBytes, 0);
            uint month = (uint)monthBytes[0];
            uint day = (uint)dayBytes[0];
            uint dayOfWeek = (uint)dayOfWeekByte[0];
            uint hour = (uint)hourBytes[0];
            uint min = (uint)minBytes[0];
            uint sec = (uint)secBytes[0];
            int nanoSec = BitConverter.ToInt32(nanoSecBytes, 0);
            return $"{year}-{month}-{day}-{hour}:{min}:{sec}:{nanoSec}";
        }

        public void ReverseIfIsLittleIndian(byte[] value)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
        }
        #endregion
    }
}
