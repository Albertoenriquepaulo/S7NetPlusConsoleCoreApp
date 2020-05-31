using S7.Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace S7NetPlusConsoleCoreApp
{
    class Program
    {

        [Obsolete]
        static async Task Main(string[] args)
        {
            PLCSettings MyPLCSettings = new PLCSettings(CpuType.S71200, "192.168.100.65", 0, 1);
            PLCSettings MyPLCSettingsPLCSim = new PLCSettings(CpuType.S71200, "127.0.0.1", 0, 1);
            XPlc myPLCSim = new XPlc(MyPLCSettingsPLCSim);
            XPlc myPLC = new XPlc(MyPLCSettings);
            const int START_BY_ADR = 8;

            try
            {
                myPLC.RestartConnection();
                myPLCSim.RestartConnection();
                Test testing = new Test();
                await myPLC.ReadClassAsync(testing, 42, START_BY_ADR);
                ///
                await myPLC.WriteValueAsync(0, PlcDataType.DWord, 42, 10);


                await myPLC.ReadClassAsync(testing, 42, START_BY_ADR);

                //myPLCSim.SetDataBlockAndStartByteAdr(1, 12);
                myPLCSim.WriteValue(0X222111, PlcDataType.DInt, 1, 12);
                myPLCSim.SetDataBlockAndStartByteAdr(1, 8);
                myPLCSim.WriteValue(1010, PlcDataType.DInt);

                await myPLCSim.WriteValueAsync(0X555444, PlcDataType.DInt, 1, 12);
                myPLCSim.SetDataBlockAndStartByteAdr(1, 8);
                await myPLCSim.WriteValueAsync(2020, PlcDataType.DInt);





                //REAL and WORD (4bytes)
                ////double DB40_DBD4a = await myPLC.ReadDWordRealAsync("DB40.DBD4", 2);

                double DB1_DBD4 = await myPLCSim.ReadDWordRealAsync(1, 4, 2);
                double DB1_DBD4a = await myPLCSim.ReadDWordRealAsync("DB1.DBD4", 2);

                double DB1_DBD34 = await myPLCSim.ReadDWordRealAsync(1, 34, 2);
                double DB1_DBD34a = await myPLCSim.ReadDWordRealAsync("DB1.DBD34", 2);


                //INTEGER (4bytes)
                int DB1_DBD12 = await myPLCSim.ReadDWordIntegerAsync(1, 12);
                int DB1_DBD12a = await myPLCSim.ReadDWordIntegerAsync("DB1.DBD12");

                int DB1_DBD8 = await myPLCSim.ReadDWordIntegerAsync(1, 8);
                int DB1_DBD8a = await myPLCSim.ReadDWordIntegerAsync("DB1.DBD8");

                //INTEGER (2bytes)
                int DB1_DBW16 = await myPLCSim.ReadWordIntegerAsync("DB1.DBW16");
                int DB1_DBW16a = await myPLCSim.ReadWordIntegerAsync(1, 16);

                int DB1_DBW2 = await myPLCSim.ReadWordIntegerAsync("DB1.DBW2");
                int DB1_DBW2a = await myPLCSim.ReadWordIntegerAsync(1, 2);

                //BOOL
                bool DB1_DBX0_0 = await myPLCSim.ReadBoolAsync("DB1.DBX0.0");
                bool DB1_DBX0_0a = await myPLCSim.ReadBoolAsync(DataType.DataBlock, 1, 0, 0);

                bool DB1_DBX0_1 = await myPLCSim.ReadBoolAsync("DB1.DBX0.1");
                bool DB1_DBX0_1a = await myPLCSim.ReadBoolAsync(DataType.DataBlock, 1, 0, 1);


                ////char vOut = Convert.ToChar(testing.AlbertoRecibir11);
                //////int sintValue = Convert.ToInt32(plc.Read("DB42.DBB9"));
                ////Test testing1 = await myPLC.ReadClassAsync<Test>(42, START_BY_ADR);

                ////var dwordValue = await myPLC.ReadDWordRealAsync(42, 10, 2);
                ////var dwordValue1 = await myPLC.ReadDWordRealAsync("DB42.DBD10", 2);
                ////var drealValue = await myPLC.ReadDWordRealAsync(42, 318, 2);

                //if (BitConverter.IsLittleEndian)
                //    Array.Reverse(testing.AlbertoRecibir4);

                ////testing.AlbertoRecibir1 = !testing.AlbertoRecibir1;
                ////testing.AlbertoRecibir2++;
                ////testing.AlbertoRecibir9++;
                ////testing.AlbertoRecibir11++;
                ////testing.BlindPercentLRoomE = 0;

                myPLCSim.SetDataBlockAndStartByteAdr(1, 4);
                myPLCSim.WriteValue(225.54, PlcDataType.DWord);

                myPLCSim.SetDataBlockAndStartByteAdr(1, 12);
                myPLCSim.WriteValue(160, PlcDataType.DWord);

                double mivalor = (await myPLCSim.ReadDWordRealAsync(1, 34, 2)) - 100;
                myPLCSim.SetDataBlockAndStartByteAdr(1, 34);
                myPLCSim.WriteValue(mivalor, PlcDataType.DWord);

                myPLCSim.SetStartByteAdr(2);
                myPLCSim.WriteValue(0, PlcDataType.Word);
                Thread.Sleep(2000);
                for (int i = 0; i < 20; i++)
                {
                    int valor = await myPLCSim.ReadWordIntegerAsync(1, 2);
                    valor++;
                    myPLCSim.WriteValue(valor, PlcDataType.Word);
                    if (valor == 10)
                    {
                        Thread.Sleep(5000);
                    }
                }

                //while (true)
                //{
                //    DB40_DBD4a = await myPLC.ReadDWordRealAsync("DB40.DBD4", 5);
                //    Console.Clear();
                //    Console.WriteLine($"\n\n\t\tRoom 1 Temperature: {DB40_DBD4a}");
                //    Thread.Sleep(50);
                //}

                myPLCSim.SetStartByteAdr(0);
                myPLCSim.WriteBit(0, true);
                myPLCSim.WriteBit(1, false);


                ////if (!myPLC.IsAvailable())
                ////{
                ////    Console.WriteLine("PLC is not Available");
                ////    Console.ReadLine();
                ////    return;
                ////}

                ////if (!myPLC.IsConnected())
                ////{
                ////    Console.WriteLine("PLC is Available but can not connect to it");
                ////    Console.ReadLine();
                ////    return;
                ////}
                ////myPLC.RestartConnection();
                ////Console.WriteLine("Connected");
                ////myPLC.SetDataBlockAndStartByteAdr(4, START_BY_ADR);

                ////myPLC.SetDataBlockAndStartByteAdr(42, START_BY_ADR);

                ////myPLC.SetStartByteAdr(36);

                ////bool heaterRoom1 = await myPLC.ReadBoolAsync("A9.2");
                ////double tempRoom1 = await myPLC.ReadDWordRealAsync("MD104", 1);
                ////double tempSPRoom1 = await myPLC.ReadDWordRealAsync("MD200", 1);  //OJO
                ////double humidityRoom1 = await myPLC.ReadDWordRealAsync("MD168", 1);
                ////double DB42_DBD10 = await myPLC.ReadDWordRealAsync("DB42.DBD10", 1);
                ////bool DB42_DBX34_0 = await myPLC.ReadBoolAsync(DataType.DataBlock, 42, 34, 0);

                ////int DB42_DBW36 = await myPLC.ReadWordIntegerAsync("DB42.DBW36");

                ////myPLC.SetStartByteAdr(318);

                ////double DB42_DBD318 = await myPLC.ReadDWordRealAsync(42, 318, 2);

                ////Console.WriteLine($"Temperatura en habitación 1         =   {tempRoom1}");
                ////Console.WriteLine($"Temperatura deseada en habitación 1 =   {tempSPRoom1}");
                ////Console.WriteLine($"Humedad Relativa en habitacion 1    =   {humidityRoom1}");
                ////Console.WriteLine($"Radiador en habitacion 1            =   {heaterRoom1}");
                ////Console.WriteLine($"DB42                                =   {DB42_DBD10}");
                ////Console.WriteLine($"DB42.DBX34.0                        =   {DB42_DBX34_0}");
                ////Console.WriteLine($"------------------------------------------------------");
                ////Console.WriteLine($"DB42.DBW36                          =   {DB42_DBW36}");
                ////Console.WriteLine($"DB42.DBD318                         =   {DB42_DBD318}");
                ////////myPLC.WriteValue(15.66, 4);
                //////DB42_DBD318 = myPLC.ReadDouble("DB42.DBD318", 1);
                ////Console.WriteLine($"DB42.DBD318                         =   {DB42_DBD318}");
                ////Console.ReadLine();

                ////myPLC.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("PLC is not available | {0}", ex.Message);
                Console.ReadLine();
            }
            //}
        }
    }
}
