using S7.Net;
//using S7.Net.Types;
//using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        #region CONSTRUCTOR
        public XPlc(PLCSettings plcSettings, int dB = 0, int startByteAdr = 0)
        {
            PLC = new Plc(plcSettings.CPU, plcSettings.IP, plcSettings.Port, plcSettings.Rack, plcSettings.Slot);
            DB = dB;
            StartByteAdress = startByteAdr;
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
                PLC.Open();
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
                    ushort ValueToWrite1 = (ushort)Convert.ToInt16(value);
                    byte[] byteArray1 = BitConverter.GetBytes(ValueToWrite1);
                    Array.Reverse(byteArray1, 0, ByteSize);
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
        public async Task<bool> ReadBoolAsync(string variable)
        {
            return (bool)(await PLC.ReadAsync(variable));//value;
        }
        public async Task<bool> ReadBoolAsync(DataType dataType, int db, int startByteAdr, int bitNumber)
        {
            int oneByte = 1;
            byte value = (await PLC.ReadBytesAsync(dataType, db, startByteAdr, oneByte))[0];
            return (value & (1 << bitNumber)) != 0;
            //Convert.ToBoolean(await PLC.ReadBytesAsync(dataType, db, startByteAdr, 1)[0]);
        }
        [Obsolete]
        public double ReadDWordReal(string variable, int decimalNumbers)
        {
            return Math.Round(((uint)PLC.Read(variable)).ConvertToDouble(), decimalNumbers);
        }
        public double ReadDWordReal(int db, int startByteAdr, int decimalNumbers)
        {
            byte[] valuInBytes = PLC.ReadBytes(DataType.DataBlock, db, startByteAdr, 4);
            ReverseIfIsLittleIndian(valuInBytes);
            double value = BitConverter.ToSingle(valuInBytes, 0);

            return Math.Round(value, decimalNumbers);
        }
        [Obsolete]
        public async Task<double> ReadDWordRealAsync(string variable, int decimalNumbers)
        {
            var value = await PLC.ReadAsync(variable);
            return Math.Round(((uint)value).ConvertToDouble(), decimalNumbers);
        }
        public async Task<double> ReadDWordRealAsync(int db, int startByteAdr, int decimalNumbers)
        {
            byte[] valuInBytes = await PLC.ReadBytesAsync(DataType.DataBlock, db, startByteAdr, 4);
            ReverseIfIsLittleIndian(valuInBytes);
            double value = BitConverter.ToSingle(valuInBytes, 0);

            return Math.Round(value, decimalNumbers);
        }
        public int ReadDWordInteger(int db, int startByteAdr)
        {
            byte[] valuInBytes = PLC.ReadBytes(DataType.DataBlock, db, startByteAdr, 4);

            return (int)S7.Net.Types.DWord.FromByteArray(valuInBytes);
        }

        public int ReadDWordInteger(string variable)
        {
            UInt32 value = (UInt32)PLC.Read(variable);
            return (int)value;
        }
        public async Task<int> ReadDWordIntegerAsync(int db, int startByteAdr)
        {
            byte[] valuInBytes = await PLC.ReadBytesAsync(DataType.DataBlock, db, startByteAdr, 4);

            return (int)S7.Net.Types.DWord.FromByteArray(valuInBytes);
        }
        public async Task<int> ReadDWordIntegerAsync(string variable)
        {
            UInt32 value = (UInt32)(await PLC.ReadAsync(variable));
            return (int)value;
        }

        public int ReadWordInt(string variable)
        {
            return (ushort)PLC.Read(variable);

        }
        public int ReadWordInt(int db, int startByteAdr)
        {
            byte[] valuInBytes = PLC.ReadBytes(DataType.DataBlock, db, startByteAdr, 2);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(valuInBytes);

            ushort num = BitConverter.ToUInt16(valuInBytes, 0);

            return BitConverter.ToUInt16(valuInBytes, 0); //(UInt16)S7.Net.Types.DInt.FromByteArray(valuInBytes);
        }
        public async Task<int> ReadWordIntAsync(string variable)
        {
            return (ushort)(await PLC.ReadAsync(variable));

        }
        public async Task<int> ReadWordIntAsync(int db, int startByteAdr)
        {
            byte[] valuInBytes = await PLC.ReadBytesAsync(DataType.DataBlock, db, startByteAdr, 2);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(valuInBytes);

            ushort num = BitConverter.ToUInt16(valuInBytes, 0);

            return BitConverter.ToUInt16(valuInBytes, 0); //(UInt16)S7.Net.Types.DInt.FromByteArray(valuInBytes);
        }
        public int ReadDIntDelete(string variable)
        {
            var value = PLC.Read(variable);
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
        public async Task<int> ReadBytesAsync(DataType dataType, int db, int startByteAdr, int count)
        {
            byte[] test = await PLC.ReadBytesAsync(dataType, db, startByteAdr, count);

            //DWord.FromByteArray(PLC.ReadBytes(dataType, db, startByteAdr, count));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(test);

            int i = BitConverter.ToInt32(test, 0);
            return i;
        }
        public int ReadClass(object sourceClass, int db, int startByteAdr = 0)
        {
            return PLC.ReadClass(sourceClass, db, startByteAdr);
        }
        public async Task<Tuple<int, object>> ReadClassAsync(object sourceClass, int db, int startByteAdr = 0)
        {
            return await PLC.ReadClassAsync(sourceClass, db, startByteAdr);
        }
        public T ReadClass<T>(int db, int startByteAdr = 0) where T : class
        {
            return PLC.ReadClass<T>(db, startByteAdr);
        }
        public async Task<T> ReadClassAsync<T>(int db, int startByteAdr = 0) where T : class
        {
            return await PLC.ReadClassAsync<T>(db, startByteAdr);
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
