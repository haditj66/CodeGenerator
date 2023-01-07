//generated file: MyDeserializercgen.cs





using MatFileHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace HolterMonitorGui
{


    public class MyDeSerializerContainer<Ttype1>
    {

        public List<Ttype1> Values { get; set; }
        public string Name { get; private set; }
        public int Length { get; private set; }

        public MyDeSerializerContainer(string name, int length)
        {
            Name = name;
            Length = length;
            Values = new List<Ttype1>(new Ttype1[length]);
        }


    }





    public class MyDeSerializer1<
     Ttype1








    > : IMyDeSerializer
     where Ttype1 : struct








    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }

















        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


















            var actual = builder.NewFile(new[] {
            VarMat1














              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer1
            <
     Ttype1








    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer1(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 1;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
















        }



        public MyDeSerializer1(
            MyDeSerializerContainer<Ttype1> variable1














            )
        {
            NumOfVars = 1;

            Variable1 = variable1;














        }





        public List<byte> Serialize(
List<Ttype1> data1














)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



















            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length













              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);
































            return true;
        }


    }






    public class MyDeSerializer2<
     Ttype1
     , Ttype2







    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct







    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
















        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);

















            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2













              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer2
            <
     Ttype1
     , Ttype2







    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer2(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 2;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);















        }



        public MyDeSerializer2(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2













            )
        {
            NumOfVars = 2;

            Variable1 = variable1;
            Variable2 = variable2;













        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2













)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }

















            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length












              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);





























            return true;
        }


    }






    public class MyDeSerializer3<
     Ttype1
     , Ttype2
     , Ttype3






    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct






    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }















        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);
















            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3












              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer3
            <
     Ttype1
     , Ttype2
     , Ttype3






    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer3(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 3;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);














        }



        public MyDeSerializer3(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3












            )
        {
            NumOfVars = 3;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;












        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3












)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }















            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length











              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);



























            return true;
        }


    }






    public class MyDeSerializer4<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4





    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct





    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }














        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);















            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4











              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer4
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4





    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer4(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 4;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);













        }



        public MyDeSerializer4(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4











            )
        {
            NumOfVars = 4;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;











        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4











)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }













            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length










              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);

























            return true;
        }


    }






    public class MyDeSerializer5<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5




    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct
     where Ttype5 : struct




    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }
        public MyDeSerializerContainer<Ttype5> Variable5 { get; private set; }













        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);


            var Var5 = builder.NewArray<Ttype5>(1, this.Variable5.Length);
            Array.Copy(this.Variable5.Values.ToArray(), Var5.Data, this.Variable5.Length);
            var VarMat5 = builder.NewVariable("Var5", Var5);














            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4
            , VarMat5










              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer5
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5




    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer5(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 5;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);
            Variable5 = new MyDeSerializerContainer<Ttype5>(namevar5, lengthvar5);












        }



        public MyDeSerializer5(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4
            , MyDeSerializerContainer<Ttype5> variable5










            )
        {
            NumOfVars = 5;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;
            Variable5 = variable5;










        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4
, List<Ttype5> data5










)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }


            for (int i = 0; i < data5.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype5>(data5[i]));
            }












            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length
                + Marshal.SizeOf<Ttype5>() * Variable5.Length









              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype5>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable5.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable5.Values[i / sizeOfDatavar] = (Ttype5)mybitConverter<Ttype5>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);























            return true;
        }


    }






    public class MyDeSerializer6<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6



    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct
     where Ttype5 : struct
     where Ttype6 : struct



    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }
        public MyDeSerializerContainer<Ttype5> Variable5 { get; private set; }
        public MyDeSerializerContainer<Ttype6> Variable6 { get; private set; }












        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);


            var Var5 = builder.NewArray<Ttype5>(1, this.Variable5.Length);
            Array.Copy(this.Variable5.Values.ToArray(), Var5.Data, this.Variable5.Length);
            var VarMat5 = builder.NewVariable("Var5", Var5);


            var Var6 = builder.NewArray<Ttype6>(1, this.Variable6.Length);
            Array.Copy(this.Variable6.Values.ToArray(), Var6.Data, this.Variable6.Length);
            var VarMat6 = builder.NewVariable("Var6", Var6);













            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4
            , VarMat5
            , VarMat6









              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer6
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6



    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer6(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 6;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);
            Variable5 = new MyDeSerializerContainer<Ttype5>(namevar5, lengthvar5);
            Variable6 = new MyDeSerializerContainer<Ttype6>(namevar6, lengthvar6);











        }



        public MyDeSerializer6(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4
            , MyDeSerializerContainer<Ttype5> variable5
            , MyDeSerializerContainer<Ttype6> variable6









            )
        {
            NumOfVars = 6;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;
            Variable5 = variable5;
            Variable6 = variable6;









        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4
, List<Ttype5> data5
, List<Ttype6> data6









)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }


            for (int i = 0; i < data5.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype5>(data5[i]));
            }


            for (int i = 0; i < data6.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype6>(data6[i]));
            }











            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length
                + Marshal.SizeOf<Ttype5>() * Variable5.Length
                + Marshal.SizeOf<Ttype6>() * Variable6.Length








              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype5>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable5.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable5.Values[i / sizeOfDatavar] = (Ttype5)mybitConverter<Ttype5>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype6>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable6.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable6.Values[i / sizeOfDatavar] = (Ttype6)mybitConverter<Ttype6>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);





















            return true;
        }


    }






    public class MyDeSerializer7<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7


    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct
     where Ttype5 : struct
     where Ttype6 : struct
     where Ttype7 : struct


    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }
        public MyDeSerializerContainer<Ttype5> Variable5 { get; private set; }
        public MyDeSerializerContainer<Ttype6> Variable6 { get; private set; }
        public MyDeSerializerContainer<Ttype7> Variable7 { get; private set; }











        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);


            var Var5 = builder.NewArray<Ttype5>(1, this.Variable5.Length);
            Array.Copy(this.Variable5.Values.ToArray(), Var5.Data, this.Variable5.Length);
            var VarMat5 = builder.NewVariable("Var5", Var5);


            var Var6 = builder.NewArray<Ttype6>(1, this.Variable6.Length);
            Array.Copy(this.Variable6.Values.ToArray(), Var6.Data, this.Variable6.Length);
            var VarMat6 = builder.NewVariable("Var6", Var6);


            var Var7 = builder.NewArray<Ttype7>(1, this.Variable7.Length);
            Array.Copy(this.Variable7.Values.ToArray(), Var7.Data, this.Variable7.Length);
            var VarMat7 = builder.NewVariable("Var7", Var7);












            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4
            , VarMat5
            , VarMat6
            , VarMat7








              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer7
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7


    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer7(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 7;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);
            Variable5 = new MyDeSerializerContainer<Ttype5>(namevar5, lengthvar5);
            Variable6 = new MyDeSerializerContainer<Ttype6>(namevar6, lengthvar6);
            Variable7 = new MyDeSerializerContainer<Ttype7>(namevar7, lengthvar7);










        }



        public MyDeSerializer7(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4
            , MyDeSerializerContainer<Ttype5> variable5
            , MyDeSerializerContainer<Ttype6> variable6
            , MyDeSerializerContainer<Ttype7> variable7








            )
        {
            NumOfVars = 7;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;
            Variable5 = variable5;
            Variable6 = variable6;
            Variable7 = variable7;








        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4
, List<Ttype5> data5
, List<Ttype6> data6
, List<Ttype7> data7








)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }


            for (int i = 0; i < data5.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype5>(data5[i]));
            }


            for (int i = 0; i < data6.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype6>(data6[i]));
            }


            for (int i = 0; i < data7.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype7>(data7[i]));
            }










            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length
                + Marshal.SizeOf<Ttype5>() * Variable5.Length
                + Marshal.SizeOf<Ttype6>() * Variable6.Length
                + Marshal.SizeOf<Ttype7>() * Variable7.Length







              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype5>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable5.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable5.Values[i / sizeOfDatavar] = (Ttype5)mybitConverter<Ttype5>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype6>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable6.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable6.Values[i / sizeOfDatavar] = (Ttype6)mybitConverter<Ttype6>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype7>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable7.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable7.Values[i / sizeOfDatavar] = (Ttype7)mybitConverter<Ttype7>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);



















            return true;
        }


    }






    public class MyDeSerializer8<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8

    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct
     where Ttype5 : struct
     where Ttype6 : struct
     where Ttype7 : struct
     where Ttype8 : struct

    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }
        public MyDeSerializerContainer<Ttype5> Variable5 { get; private set; }
        public MyDeSerializerContainer<Ttype6> Variable6 { get; private set; }
        public MyDeSerializerContainer<Ttype7> Variable7 { get; private set; }
        public MyDeSerializerContainer<Ttype8> Variable8 { get; private set; }










        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);


            var Var5 = builder.NewArray<Ttype5>(1, this.Variable5.Length);
            Array.Copy(this.Variable5.Values.ToArray(), Var5.Data, this.Variable5.Length);
            var VarMat5 = builder.NewVariable("Var5", Var5);


            var Var6 = builder.NewArray<Ttype6>(1, this.Variable6.Length);
            Array.Copy(this.Variable6.Values.ToArray(), Var6.Data, this.Variable6.Length);
            var VarMat6 = builder.NewVariable("Var6", Var6);


            var Var7 = builder.NewArray<Ttype7>(1, this.Variable7.Length);
            Array.Copy(this.Variable7.Values.ToArray(), Var7.Data, this.Variable7.Length);
            var VarMat7 = builder.NewVariable("Var7", Var7);


            var Var8 = builder.NewArray<Ttype8>(1, this.Variable8.Length);
            Array.Copy(this.Variable8.Values.ToArray(), Var8.Data, this.Variable8.Length);
            var VarMat8 = builder.NewVariable("Var8", Var8);











            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4
            , VarMat5
            , VarMat6
            , VarMat7
            , VarMat8







              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer8
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8

    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer8(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 8;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);
            Variable5 = new MyDeSerializerContainer<Ttype5>(namevar5, lengthvar5);
            Variable6 = new MyDeSerializerContainer<Ttype6>(namevar6, lengthvar6);
            Variable7 = new MyDeSerializerContainer<Ttype7>(namevar7, lengthvar7);
            Variable8 = new MyDeSerializerContainer<Ttype8>(namevar8, lengthvar8);









        }



        public MyDeSerializer8(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4
            , MyDeSerializerContainer<Ttype5> variable5
            , MyDeSerializerContainer<Ttype6> variable6
            , MyDeSerializerContainer<Ttype7> variable7
            , MyDeSerializerContainer<Ttype8> variable8







            )
        {
            NumOfVars = 8;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;
            Variable5 = variable5;
            Variable6 = variable6;
            Variable7 = variable7;
            Variable8 = variable8;







        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4
, List<Ttype5> data5
, List<Ttype6> data6
, List<Ttype7> data7
, List<Ttype8> data8







)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }


            for (int i = 0; i < data5.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype5>(data5[i]));
            }


            for (int i = 0; i < data6.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype6>(data6[i]));
            }


            for (int i = 0; i < data7.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype7>(data7[i]));
            }


            for (int i = 0; i < data8.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype8>(data8[i]));
            }









            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length
                + Marshal.SizeOf<Ttype5>() * Variable5.Length
                + Marshal.SizeOf<Ttype6>() * Variable6.Length
                + Marshal.SizeOf<Ttype7>() * Variable7.Length
                + Marshal.SizeOf<Ttype8>() * Variable8.Length






              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype5>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable5.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable5.Values[i / sizeOfDatavar] = (Ttype5)mybitConverter<Ttype5>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype6>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable6.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable6.Values[i / sizeOfDatavar] = (Ttype6)mybitConverter<Ttype6>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype7>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable7.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable7.Values[i / sizeOfDatavar] = (Ttype7)mybitConverter<Ttype7>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype8>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable8.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable8.Values[i / sizeOfDatavar] = (Ttype8)mybitConverter<Ttype8>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);

















            return true;
        }


    }






    public class MyDeSerializer9<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct
     where Ttype5 : struct
     where Ttype6 : struct
     where Ttype7 : struct
     where Ttype8 : struct
     where Ttype9 : struct
    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }
        public MyDeSerializerContainer<Ttype5> Variable5 { get; private set; }
        public MyDeSerializerContainer<Ttype6> Variable6 { get; private set; }
        public MyDeSerializerContainer<Ttype7> Variable7 { get; private set; }
        public MyDeSerializerContainer<Ttype8> Variable8 { get; private set; }
        public MyDeSerializerContainer<Ttype9> Variable9 { get; private set; }









        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);


            var Var5 = builder.NewArray<Ttype5>(1, this.Variable5.Length);
            Array.Copy(this.Variable5.Values.ToArray(), Var5.Data, this.Variable5.Length);
            var VarMat5 = builder.NewVariable("Var5", Var5);


            var Var6 = builder.NewArray<Ttype6>(1, this.Variable6.Length);
            Array.Copy(this.Variable6.Values.ToArray(), Var6.Data, this.Variable6.Length);
            var VarMat6 = builder.NewVariable("Var6", Var6);


            var Var7 = builder.NewArray<Ttype7>(1, this.Variable7.Length);
            Array.Copy(this.Variable7.Values.ToArray(), Var7.Data, this.Variable7.Length);
            var VarMat7 = builder.NewVariable("Var7", Var7);


            var Var8 = builder.NewArray<Ttype8>(1, this.Variable8.Length);
            Array.Copy(this.Variable8.Values.ToArray(), Var8.Data, this.Variable8.Length);
            var VarMat8 = builder.NewVariable("Var8", Var8);


            var Var9 = builder.NewArray<Ttype9>(1, this.Variable9.Length);
            Array.Copy(this.Variable9.Values.ToArray(), Var9.Data, this.Variable9.Length);
            var VarMat9 = builder.NewVariable("Var9", Var9);










            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4
            , VarMat5
            , VarMat6
            , VarMat7
            , VarMat8
            , VarMat9






              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer9
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer9(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 9;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);
            Variable5 = new MyDeSerializerContainer<Ttype5>(namevar5, lengthvar5);
            Variable6 = new MyDeSerializerContainer<Ttype6>(namevar6, lengthvar6);
            Variable7 = new MyDeSerializerContainer<Ttype7>(namevar7, lengthvar7);
            Variable8 = new MyDeSerializerContainer<Ttype8>(namevar8, lengthvar8);
            Variable9 = new MyDeSerializerContainer<Ttype9>(namevar9, lengthvar9);








        }



        public MyDeSerializer9(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4
            , MyDeSerializerContainer<Ttype5> variable5
            , MyDeSerializerContainer<Ttype6> variable6
            , MyDeSerializerContainer<Ttype7> variable7
            , MyDeSerializerContainer<Ttype8> variable8
            , MyDeSerializerContainer<Ttype9> variable9






            )
        {
            NumOfVars = 9;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;
            Variable5 = variable5;
            Variable6 = variable6;
            Variable7 = variable7;
            Variable8 = variable8;
            Variable9 = variable9;






        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4
, List<Ttype5> data5
, List<Ttype6> data6
, List<Ttype7> data7
, List<Ttype8> data8
, List<Ttype9> data9






)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }


            for (int i = 0; i < data5.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype5>(data5[i]));
            }


            for (int i = 0; i < data6.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype6>(data6[i]));
            }


            for (int i = 0; i < data7.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype7>(data7[i]));
            }


            for (int i = 0; i < data8.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype8>(data8[i]));
            }


            for (int i = 0; i < data9.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype9>(data9[i]));
            }








            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length
                + Marshal.SizeOf<Ttype5>() * Variable5.Length
                + Marshal.SizeOf<Ttype6>() * Variable6.Length
                + Marshal.SizeOf<Ttype7>() * Variable7.Length
                + Marshal.SizeOf<Ttype8>() * Variable8.Length
                + Marshal.SizeOf<Ttype9>() * Variable9.Length





              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype5>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable5.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable5.Values[i / sizeOfDatavar] = (Ttype5)mybitConverter<Ttype5>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype6>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable6.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable6.Values[i / sizeOfDatavar] = (Ttype6)mybitConverter<Ttype6>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype7>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable7.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable7.Values[i / sizeOfDatavar] = (Ttype7)mybitConverter<Ttype7>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype8>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable8.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable8.Values[i / sizeOfDatavar] = (Ttype8)mybitConverter<Ttype8>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype9>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable9.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable9.Values[i / sizeOfDatavar] = (Ttype9)mybitConverter<Ttype9>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);















            return true;
        }


    }






    public class MyDeSerializer10<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct
     where Ttype5 : struct
     where Ttype6 : struct
     where Ttype7 : struct
     where Ttype8 : struct
     where Ttype9 : struct
     where Ttype10 : struct
    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }
        public MyDeSerializerContainer<Ttype5> Variable5 { get; private set; }
        public MyDeSerializerContainer<Ttype6> Variable6 { get; private set; }
        public MyDeSerializerContainer<Ttype7> Variable7 { get; private set; }
        public MyDeSerializerContainer<Ttype8> Variable8 { get; private set; }
        public MyDeSerializerContainer<Ttype9> Variable9 { get; private set; }
        public MyDeSerializerContainer<Ttype10> Variable10 { get; private set; }








        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);


            var Var5 = builder.NewArray<Ttype5>(1, this.Variable5.Length);
            Array.Copy(this.Variable5.Values.ToArray(), Var5.Data, this.Variable5.Length);
            var VarMat5 = builder.NewVariable("Var5", Var5);


            var Var6 = builder.NewArray<Ttype6>(1, this.Variable6.Length);
            Array.Copy(this.Variable6.Values.ToArray(), Var6.Data, this.Variable6.Length);
            var VarMat6 = builder.NewVariable("Var6", Var6);


            var Var7 = builder.NewArray<Ttype7>(1, this.Variable7.Length);
            Array.Copy(this.Variable7.Values.ToArray(), Var7.Data, this.Variable7.Length);
            var VarMat7 = builder.NewVariable("Var7", Var7);


            var Var8 = builder.NewArray<Ttype8>(1, this.Variable8.Length);
            Array.Copy(this.Variable8.Values.ToArray(), Var8.Data, this.Variable8.Length);
            var VarMat8 = builder.NewVariable("Var8", Var8);


            var Var9 = builder.NewArray<Ttype9>(1, this.Variable9.Length);
            Array.Copy(this.Variable9.Values.ToArray(), Var9.Data, this.Variable9.Length);
            var VarMat9 = builder.NewVariable("Var9", Var9);


            var Var10 = builder.NewArray<Ttype10>(1, this.Variable10.Length);
            Array.Copy(this.Variable10.Values.ToArray(), Var10.Data, this.Variable10.Length);
            var VarMat10 = builder.NewVariable("Var10", Var10);









            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4
            , VarMat5
            , VarMat6
            , VarMat7
            , VarMat8
            , VarMat9
            , VarMat10





              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer10
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer10(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 10;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);
            Variable5 = new MyDeSerializerContainer<Ttype5>(namevar5, lengthvar5);
            Variable6 = new MyDeSerializerContainer<Ttype6>(namevar6, lengthvar6);
            Variable7 = new MyDeSerializerContainer<Ttype7>(namevar7, lengthvar7);
            Variable8 = new MyDeSerializerContainer<Ttype8>(namevar8, lengthvar8);
            Variable9 = new MyDeSerializerContainer<Ttype9>(namevar9, lengthvar9);
            Variable10 = new MyDeSerializerContainer<Ttype10>(namevar10, lengthvar10);







        }



        public MyDeSerializer10(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4
            , MyDeSerializerContainer<Ttype5> variable5
            , MyDeSerializerContainer<Ttype6> variable6
            , MyDeSerializerContainer<Ttype7> variable7
            , MyDeSerializerContainer<Ttype8> variable8
            , MyDeSerializerContainer<Ttype9> variable9
            , MyDeSerializerContainer<Ttype10> variable10





            )
        {
            NumOfVars = 10;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;
            Variable5 = variable5;
            Variable6 = variable6;
            Variable7 = variable7;
            Variable8 = variable8;
            Variable9 = variable9;
            Variable10 = variable10;





        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4
, List<Ttype5> data5
, List<Ttype6> data6
, List<Ttype7> data7
, List<Ttype8> data8
, List<Ttype9> data9
, List<Ttype10> data10





)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }


            for (int i = 0; i < data5.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype5>(data5[i]));
            }


            for (int i = 0; i < data6.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype6>(data6[i]));
            }


            for (int i = 0; i < data7.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype7>(data7[i]));
            }


            for (int i = 0; i < data8.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype8>(data8[i]));
            }


            for (int i = 0; i < data9.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype9>(data9[i]));
            }


            for (int i = 0; i < data10.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype10>(data10[i]));
            }







            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length
                + Marshal.SizeOf<Ttype5>() * Variable5.Length
                + Marshal.SizeOf<Ttype6>() * Variable6.Length
                + Marshal.SizeOf<Ttype7>() * Variable7.Length
                + Marshal.SizeOf<Ttype8>() * Variable8.Length
                + Marshal.SizeOf<Ttype9>() * Variable9.Length
                + Marshal.SizeOf<Ttype10>() * Variable10.Length




              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype5>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable5.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable5.Values[i / sizeOfDatavar] = (Ttype5)mybitConverter<Ttype5>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype6>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable6.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable6.Values[i / sizeOfDatavar] = (Ttype6)mybitConverter<Ttype6>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype7>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable7.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable7.Values[i / sizeOfDatavar] = (Ttype7)mybitConverter<Ttype7>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype8>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable8.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable8.Values[i / sizeOfDatavar] = (Ttype8)mybitConverter<Ttype8>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype9>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable9.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable9.Values[i / sizeOfDatavar] = (Ttype9)mybitConverter<Ttype9>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype10>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable10.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable10.Values[i / sizeOfDatavar] = (Ttype10)mybitConverter<Ttype10>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);













            return true;
        }


    }






    public class MyDeSerializer11<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
     , Ttype11
    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct
     where Ttype5 : struct
     where Ttype6 : struct
     where Ttype7 : struct
     where Ttype8 : struct
     where Ttype9 : struct
     where Ttype10 : struct
     where Ttype11 : struct
    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }
        public MyDeSerializerContainer<Ttype5> Variable5 { get; private set; }
        public MyDeSerializerContainer<Ttype6> Variable6 { get; private set; }
        public MyDeSerializerContainer<Ttype7> Variable7 { get; private set; }
        public MyDeSerializerContainer<Ttype8> Variable8 { get; private set; }
        public MyDeSerializerContainer<Ttype9> Variable9 { get; private set; }
        public MyDeSerializerContainer<Ttype10> Variable10 { get; private set; }
        public MyDeSerializerContainer<Ttype11> Variable11 { get; private set; }







        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);


            var Var5 = builder.NewArray<Ttype5>(1, this.Variable5.Length);
            Array.Copy(this.Variable5.Values.ToArray(), Var5.Data, this.Variable5.Length);
            var VarMat5 = builder.NewVariable("Var5", Var5);


            var Var6 = builder.NewArray<Ttype6>(1, this.Variable6.Length);
            Array.Copy(this.Variable6.Values.ToArray(), Var6.Data, this.Variable6.Length);
            var VarMat6 = builder.NewVariable("Var6", Var6);


            var Var7 = builder.NewArray<Ttype7>(1, this.Variable7.Length);
            Array.Copy(this.Variable7.Values.ToArray(), Var7.Data, this.Variable7.Length);
            var VarMat7 = builder.NewVariable("Var7", Var7);


            var Var8 = builder.NewArray<Ttype8>(1, this.Variable8.Length);
            Array.Copy(this.Variable8.Values.ToArray(), Var8.Data, this.Variable8.Length);
            var VarMat8 = builder.NewVariable("Var8", Var8);


            var Var9 = builder.NewArray<Ttype9>(1, this.Variable9.Length);
            Array.Copy(this.Variable9.Values.ToArray(), Var9.Data, this.Variable9.Length);
            var VarMat9 = builder.NewVariable("Var9", Var9);


            var Var10 = builder.NewArray<Ttype10>(1, this.Variable10.Length);
            Array.Copy(this.Variable10.Values.ToArray(), Var10.Data, this.Variable10.Length);
            var VarMat10 = builder.NewVariable("Var10", Var10);


            var Var11 = builder.NewArray<Ttype11>(1, this.Variable11.Length);
            Array.Copy(this.Variable11.Values.ToArray(), Var11.Data, this.Variable11.Length);
            var VarMat11 = builder.NewVariable("Var11", Var11);








            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4
            , VarMat5
            , VarMat6
            , VarMat7
            , VarMat8
            , VarMat9
            , VarMat10
            , VarMat11




              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer11
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
     , Ttype11
    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer11(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 11;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);
            Variable5 = new MyDeSerializerContainer<Ttype5>(namevar5, lengthvar5);
            Variable6 = new MyDeSerializerContainer<Ttype6>(namevar6, lengthvar6);
            Variable7 = new MyDeSerializerContainer<Ttype7>(namevar7, lengthvar7);
            Variable8 = new MyDeSerializerContainer<Ttype8>(namevar8, lengthvar8);
            Variable9 = new MyDeSerializerContainer<Ttype9>(namevar9, lengthvar9);
            Variable10 = new MyDeSerializerContainer<Ttype10>(namevar10, lengthvar10);
            Variable11 = new MyDeSerializerContainer<Ttype11>(namevar11, lengthvar11);






        }



        public MyDeSerializer11(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4
            , MyDeSerializerContainer<Ttype5> variable5
            , MyDeSerializerContainer<Ttype6> variable6
            , MyDeSerializerContainer<Ttype7> variable7
            , MyDeSerializerContainer<Ttype8> variable8
            , MyDeSerializerContainer<Ttype9> variable9
            , MyDeSerializerContainer<Ttype10> variable10
            , MyDeSerializerContainer<Ttype11> variable11




            )
        {
            NumOfVars = 11;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;
            Variable5 = variable5;
            Variable6 = variable6;
            Variable7 = variable7;
            Variable8 = variable8;
            Variable9 = variable9;
            Variable10 = variable10;
            Variable11 = variable11;




        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4
, List<Ttype5> data5
, List<Ttype6> data6
, List<Ttype7> data7
, List<Ttype8> data8
, List<Ttype9> data9
, List<Ttype10> data10
, List<Ttype11> data11




)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }


            for (int i = 0; i < data5.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype5>(data5[i]));
            }


            for (int i = 0; i < data6.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype6>(data6[i]));
            }


            for (int i = 0; i < data7.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype7>(data7[i]));
            }


            for (int i = 0; i < data8.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype8>(data8[i]));
            }


            for (int i = 0; i < data9.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype9>(data9[i]));
            }


            for (int i = 0; i < data10.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype10>(data10[i]));
            }


            for (int i = 0; i < data11.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype11>(data11[i]));
            }






            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length
                + Marshal.SizeOf<Ttype5>() * Variable5.Length
                + Marshal.SizeOf<Ttype6>() * Variable6.Length
                + Marshal.SizeOf<Ttype7>() * Variable7.Length
                + Marshal.SizeOf<Ttype8>() * Variable8.Length
                + Marshal.SizeOf<Ttype9>() * Variable9.Length
                + Marshal.SizeOf<Ttype10>() * Variable10.Length
                + Marshal.SizeOf<Ttype11>() * Variable11.Length



              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype5>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable5.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable5.Values[i / sizeOfDatavar] = (Ttype5)mybitConverter<Ttype5>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype6>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable6.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable6.Values[i / sizeOfDatavar] = (Ttype6)mybitConverter<Ttype6>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype7>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable7.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable7.Values[i / sizeOfDatavar] = (Ttype7)mybitConverter<Ttype7>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype8>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable8.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable8.Values[i / sizeOfDatavar] = (Ttype8)mybitConverter<Ttype8>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype9>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable9.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable9.Values[i / sizeOfDatavar] = (Ttype9)mybitConverter<Ttype9>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype10>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable10.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable10.Values[i / sizeOfDatavar] = (Ttype10)mybitConverter<Ttype10>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype11>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable11.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable11.Values[i / sizeOfDatavar] = (Ttype11)mybitConverter<Ttype11>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);











            return true;
        }


    }






    public class MyDeSerializer12<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
     , Ttype11
     , Ttype12
    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct
     where Ttype5 : struct
     where Ttype6 : struct
     where Ttype7 : struct
     where Ttype8 : struct
     where Ttype9 : struct
     where Ttype10 : struct
     where Ttype11 : struct
     where Ttype12 : struct
    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }
        public MyDeSerializerContainer<Ttype5> Variable5 { get; private set; }
        public MyDeSerializerContainer<Ttype6> Variable6 { get; private set; }
        public MyDeSerializerContainer<Ttype7> Variable7 { get; private set; }
        public MyDeSerializerContainer<Ttype8> Variable8 { get; private set; }
        public MyDeSerializerContainer<Ttype9> Variable9 { get; private set; }
        public MyDeSerializerContainer<Ttype10> Variable10 { get; private set; }
        public MyDeSerializerContainer<Ttype11> Variable11 { get; private set; }
        public MyDeSerializerContainer<Ttype12> Variable12 { get; private set; }






        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);


            var Var5 = builder.NewArray<Ttype5>(1, this.Variable5.Length);
            Array.Copy(this.Variable5.Values.ToArray(), Var5.Data, this.Variable5.Length);
            var VarMat5 = builder.NewVariable("Var5", Var5);


            var Var6 = builder.NewArray<Ttype6>(1, this.Variable6.Length);
            Array.Copy(this.Variable6.Values.ToArray(), Var6.Data, this.Variable6.Length);
            var VarMat6 = builder.NewVariable("Var6", Var6);


            var Var7 = builder.NewArray<Ttype7>(1, this.Variable7.Length);
            Array.Copy(this.Variable7.Values.ToArray(), Var7.Data, this.Variable7.Length);
            var VarMat7 = builder.NewVariable("Var7", Var7);


            var Var8 = builder.NewArray<Ttype8>(1, this.Variable8.Length);
            Array.Copy(this.Variable8.Values.ToArray(), Var8.Data, this.Variable8.Length);
            var VarMat8 = builder.NewVariable("Var8", Var8);


            var Var9 = builder.NewArray<Ttype9>(1, this.Variable9.Length);
            Array.Copy(this.Variable9.Values.ToArray(), Var9.Data, this.Variable9.Length);
            var VarMat9 = builder.NewVariable("Var9", Var9);


            var Var10 = builder.NewArray<Ttype10>(1, this.Variable10.Length);
            Array.Copy(this.Variable10.Values.ToArray(), Var10.Data, this.Variable10.Length);
            var VarMat10 = builder.NewVariable("Var10", Var10);


            var Var11 = builder.NewArray<Ttype11>(1, this.Variable11.Length);
            Array.Copy(this.Variable11.Values.ToArray(), Var11.Data, this.Variable11.Length);
            var VarMat11 = builder.NewVariable("Var11", Var11);


            var Var12 = builder.NewArray<Ttype12>(1, this.Variable12.Length);
            Array.Copy(this.Variable12.Values.ToArray(), Var12.Data, this.Variable12.Length);
            var VarMat12 = builder.NewVariable("Var12", Var12);







            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4
            , VarMat5
            , VarMat6
            , VarMat7
            , VarMat8
            , VarMat9
            , VarMat10
            , VarMat11
            , VarMat12



              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer12
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
     , Ttype11
     , Ttype12
    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer12(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 12;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);
            Variable5 = new MyDeSerializerContainer<Ttype5>(namevar5, lengthvar5);
            Variable6 = new MyDeSerializerContainer<Ttype6>(namevar6, lengthvar6);
            Variable7 = new MyDeSerializerContainer<Ttype7>(namevar7, lengthvar7);
            Variable8 = new MyDeSerializerContainer<Ttype8>(namevar8, lengthvar8);
            Variable9 = new MyDeSerializerContainer<Ttype9>(namevar9, lengthvar9);
            Variable10 = new MyDeSerializerContainer<Ttype10>(namevar10, lengthvar10);
            Variable11 = new MyDeSerializerContainer<Ttype11>(namevar11, lengthvar11);
            Variable12 = new MyDeSerializerContainer<Ttype12>(namevar12, lengthvar12);





        }



        public MyDeSerializer12(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4
            , MyDeSerializerContainer<Ttype5> variable5
            , MyDeSerializerContainer<Ttype6> variable6
            , MyDeSerializerContainer<Ttype7> variable7
            , MyDeSerializerContainer<Ttype8> variable8
            , MyDeSerializerContainer<Ttype9> variable9
            , MyDeSerializerContainer<Ttype10> variable10
            , MyDeSerializerContainer<Ttype11> variable11
            , MyDeSerializerContainer<Ttype12> variable12



            )
        {
            NumOfVars = 12;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;
            Variable5 = variable5;
            Variable6 = variable6;
            Variable7 = variable7;
            Variable8 = variable8;
            Variable9 = variable9;
            Variable10 = variable10;
            Variable11 = variable11;
            Variable12 = variable12;



        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4
, List<Ttype5> data5
, List<Ttype6> data6
, List<Ttype7> data7
, List<Ttype8> data8
, List<Ttype9> data9
, List<Ttype10> data10
, List<Ttype11> data11
, List<Ttype12> data12



)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }


            for (int i = 0; i < data5.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype5>(data5[i]));
            }


            for (int i = 0; i < data6.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype6>(data6[i]));
            }


            for (int i = 0; i < data7.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype7>(data7[i]));
            }


            for (int i = 0; i < data8.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype8>(data8[i]));
            }


            for (int i = 0; i < data9.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype9>(data9[i]));
            }


            for (int i = 0; i < data10.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype10>(data10[i]));
            }


            for (int i = 0; i < data11.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype11>(data11[i]));
            }


            for (int i = 0; i < data12.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype12>(data12[i]));
            }





            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length
                + Marshal.SizeOf<Ttype5>() * Variable5.Length
                + Marshal.SizeOf<Ttype6>() * Variable6.Length
                + Marshal.SizeOf<Ttype7>() * Variable7.Length
                + Marshal.SizeOf<Ttype8>() * Variable8.Length
                + Marshal.SizeOf<Ttype9>() * Variable9.Length
                + Marshal.SizeOf<Ttype10>() * Variable10.Length
                + Marshal.SizeOf<Ttype11>() * Variable11.Length
                + Marshal.SizeOf<Ttype12>() * Variable12.Length


              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype5>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable5.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable5.Values[i / sizeOfDatavar] = (Ttype5)mybitConverter<Ttype5>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype6>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable6.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable6.Values[i / sizeOfDatavar] = (Ttype6)mybitConverter<Ttype6>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype7>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable7.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable7.Values[i / sizeOfDatavar] = (Ttype7)mybitConverter<Ttype7>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype8>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable8.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable8.Values[i / sizeOfDatavar] = (Ttype8)mybitConverter<Ttype8>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype9>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable9.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable9.Values[i / sizeOfDatavar] = (Ttype9)mybitConverter<Ttype9>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype10>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable10.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable10.Values[i / sizeOfDatavar] = (Ttype10)mybitConverter<Ttype10>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype11>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable11.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable11.Values[i / sizeOfDatavar] = (Ttype11)mybitConverter<Ttype11>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype12>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable12.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable12.Values[i / sizeOfDatavar] = (Ttype12)mybitConverter<Ttype12>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);









            return true;
        }


    }






    public class MyDeSerializer13<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
     , Ttype11
     , Ttype12
     , Ttype13
    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct
     where Ttype5 : struct
     where Ttype6 : struct
     where Ttype7 : struct
     where Ttype8 : struct
     where Ttype9 : struct
     where Ttype10 : struct
     where Ttype11 : struct
     where Ttype12 : struct
     where Ttype13 : struct
    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }
        public MyDeSerializerContainer<Ttype5> Variable5 { get; private set; }
        public MyDeSerializerContainer<Ttype6> Variable6 { get; private set; }
        public MyDeSerializerContainer<Ttype7> Variable7 { get; private set; }
        public MyDeSerializerContainer<Ttype8> Variable8 { get; private set; }
        public MyDeSerializerContainer<Ttype9> Variable9 { get; private set; }
        public MyDeSerializerContainer<Ttype10> Variable10 { get; private set; }
        public MyDeSerializerContainer<Ttype11> Variable11 { get; private set; }
        public MyDeSerializerContainer<Ttype12> Variable12 { get; private set; }
        public MyDeSerializerContainer<Ttype13> Variable13 { get; private set; }





        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);


            var Var5 = builder.NewArray<Ttype5>(1, this.Variable5.Length);
            Array.Copy(this.Variable5.Values.ToArray(), Var5.Data, this.Variable5.Length);
            var VarMat5 = builder.NewVariable("Var5", Var5);


            var Var6 = builder.NewArray<Ttype6>(1, this.Variable6.Length);
            Array.Copy(this.Variable6.Values.ToArray(), Var6.Data, this.Variable6.Length);
            var VarMat6 = builder.NewVariable("Var6", Var6);


            var Var7 = builder.NewArray<Ttype7>(1, this.Variable7.Length);
            Array.Copy(this.Variable7.Values.ToArray(), Var7.Data, this.Variable7.Length);
            var VarMat7 = builder.NewVariable("Var7", Var7);


            var Var8 = builder.NewArray<Ttype8>(1, this.Variable8.Length);
            Array.Copy(this.Variable8.Values.ToArray(), Var8.Data, this.Variable8.Length);
            var VarMat8 = builder.NewVariable("Var8", Var8);


            var Var9 = builder.NewArray<Ttype9>(1, this.Variable9.Length);
            Array.Copy(this.Variable9.Values.ToArray(), Var9.Data, this.Variable9.Length);
            var VarMat9 = builder.NewVariable("Var9", Var9);


            var Var10 = builder.NewArray<Ttype10>(1, this.Variable10.Length);
            Array.Copy(this.Variable10.Values.ToArray(), Var10.Data, this.Variable10.Length);
            var VarMat10 = builder.NewVariable("Var10", Var10);


            var Var11 = builder.NewArray<Ttype11>(1, this.Variable11.Length);
            Array.Copy(this.Variable11.Values.ToArray(), Var11.Data, this.Variable11.Length);
            var VarMat11 = builder.NewVariable("Var11", Var11);


            var Var12 = builder.NewArray<Ttype12>(1, this.Variable12.Length);
            Array.Copy(this.Variable12.Values.ToArray(), Var12.Data, this.Variable12.Length);
            var VarMat12 = builder.NewVariable("Var12", Var12);


            var Var13 = builder.NewArray<Ttype13>(1, this.Variable13.Length);
            Array.Copy(this.Variable13.Values.ToArray(), Var13.Data, this.Variable13.Length);
            var VarMat13 = builder.NewVariable("Var13", Var13);






            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4
            , VarMat5
            , VarMat6
            , VarMat7
            , VarMat8
            , VarMat9
            , VarMat10
            , VarMat11
            , VarMat12
            , VarMat13


              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer13
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
     , Ttype11
     , Ttype12
     , Ttype13
    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer13(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 13;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);
            Variable5 = new MyDeSerializerContainer<Ttype5>(namevar5, lengthvar5);
            Variable6 = new MyDeSerializerContainer<Ttype6>(namevar6, lengthvar6);
            Variable7 = new MyDeSerializerContainer<Ttype7>(namevar7, lengthvar7);
            Variable8 = new MyDeSerializerContainer<Ttype8>(namevar8, lengthvar8);
            Variable9 = new MyDeSerializerContainer<Ttype9>(namevar9, lengthvar9);
            Variable10 = new MyDeSerializerContainer<Ttype10>(namevar10, lengthvar10);
            Variable11 = new MyDeSerializerContainer<Ttype11>(namevar11, lengthvar11);
            Variable12 = new MyDeSerializerContainer<Ttype12>(namevar12, lengthvar12);
            Variable13 = new MyDeSerializerContainer<Ttype13>(namevar13, lengthvar13);




        }



        public MyDeSerializer13(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4
            , MyDeSerializerContainer<Ttype5> variable5
            , MyDeSerializerContainer<Ttype6> variable6
            , MyDeSerializerContainer<Ttype7> variable7
            , MyDeSerializerContainer<Ttype8> variable8
            , MyDeSerializerContainer<Ttype9> variable9
            , MyDeSerializerContainer<Ttype10> variable10
            , MyDeSerializerContainer<Ttype11> variable11
            , MyDeSerializerContainer<Ttype12> variable12
            , MyDeSerializerContainer<Ttype13> variable13


            )
        {
            NumOfVars = 13;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;
            Variable5 = variable5;
            Variable6 = variable6;
            Variable7 = variable7;
            Variable8 = variable8;
            Variable9 = variable9;
            Variable10 = variable10;
            Variable11 = variable11;
            Variable12 = variable12;
            Variable13 = variable13;


        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4
, List<Ttype5> data5
, List<Ttype6> data6
, List<Ttype7> data7
, List<Ttype8> data8
, List<Ttype9> data9
, List<Ttype10> data10
, List<Ttype11> data11
, List<Ttype12> data12
, List<Ttype13> data13


)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }


            for (int i = 0; i < data5.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype5>(data5[i]));
            }


            for (int i = 0; i < data6.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype6>(data6[i]));
            }


            for (int i = 0; i < data7.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype7>(data7[i]));
            }


            for (int i = 0; i < data8.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype8>(data8[i]));
            }


            for (int i = 0; i < data9.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype9>(data9[i]));
            }


            for (int i = 0; i < data10.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype10>(data10[i]));
            }


            for (int i = 0; i < data11.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype11>(data11[i]));
            }


            for (int i = 0; i < data12.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype12>(data12[i]));
            }


            for (int i = 0; i < data13.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype13>(data13[i]));
            }




            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length
                + Marshal.SizeOf<Ttype5>() * Variable5.Length
                + Marshal.SizeOf<Ttype6>() * Variable6.Length
                + Marshal.SizeOf<Ttype7>() * Variable7.Length
                + Marshal.SizeOf<Ttype8>() * Variable8.Length
                + Marshal.SizeOf<Ttype9>() * Variable9.Length
                + Marshal.SizeOf<Ttype10>() * Variable10.Length
                + Marshal.SizeOf<Ttype11>() * Variable11.Length
                + Marshal.SizeOf<Ttype12>() * Variable12.Length
                + Marshal.SizeOf<Ttype13>() * Variable13.Length

              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype5>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable5.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable5.Values[i / sizeOfDatavar] = (Ttype5)mybitConverter<Ttype5>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype6>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable6.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable6.Values[i / sizeOfDatavar] = (Ttype6)mybitConverter<Ttype6>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype7>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable7.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable7.Values[i / sizeOfDatavar] = (Ttype7)mybitConverter<Ttype7>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype8>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable8.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable8.Values[i / sizeOfDatavar] = (Ttype8)mybitConverter<Ttype8>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype9>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable9.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable9.Values[i / sizeOfDatavar] = (Ttype9)mybitConverter<Ttype9>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype10>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable10.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable10.Values[i / sizeOfDatavar] = (Ttype10)mybitConverter<Ttype10>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype11>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable11.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable11.Values[i / sizeOfDatavar] = (Ttype11)mybitConverter<Ttype11>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype12>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable12.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable12.Values[i / sizeOfDatavar] = (Ttype12)mybitConverter<Ttype12>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype13>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable13.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable13.Values[i / sizeOfDatavar] = (Ttype13)mybitConverter<Ttype13>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);







            return true;
        }


    }






    public class MyDeSerializer14<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
     , Ttype11
     , Ttype12
     , Ttype13
     , Ttype14
    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct
     where Ttype5 : struct
     where Ttype6 : struct
     where Ttype7 : struct
     where Ttype8 : struct
     where Ttype9 : struct
     where Ttype10 : struct
     where Ttype11 : struct
     where Ttype12 : struct
     where Ttype13 : struct
     where Ttype14 : struct
    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }
        public MyDeSerializerContainer<Ttype5> Variable5 { get; private set; }
        public MyDeSerializerContainer<Ttype6> Variable6 { get; private set; }
        public MyDeSerializerContainer<Ttype7> Variable7 { get; private set; }
        public MyDeSerializerContainer<Ttype8> Variable8 { get; private set; }
        public MyDeSerializerContainer<Ttype9> Variable9 { get; private set; }
        public MyDeSerializerContainer<Ttype10> Variable10 { get; private set; }
        public MyDeSerializerContainer<Ttype11> Variable11 { get; private set; }
        public MyDeSerializerContainer<Ttype12> Variable12 { get; private set; }
        public MyDeSerializerContainer<Ttype13> Variable13 { get; private set; }
        public MyDeSerializerContainer<Ttype14> Variable14 { get; private set; }




        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);


            var Var5 = builder.NewArray<Ttype5>(1, this.Variable5.Length);
            Array.Copy(this.Variable5.Values.ToArray(), Var5.Data, this.Variable5.Length);
            var VarMat5 = builder.NewVariable("Var5", Var5);


            var Var6 = builder.NewArray<Ttype6>(1, this.Variable6.Length);
            Array.Copy(this.Variable6.Values.ToArray(), Var6.Data, this.Variable6.Length);
            var VarMat6 = builder.NewVariable("Var6", Var6);


            var Var7 = builder.NewArray<Ttype7>(1, this.Variable7.Length);
            Array.Copy(this.Variable7.Values.ToArray(), Var7.Data, this.Variable7.Length);
            var VarMat7 = builder.NewVariable("Var7", Var7);


            var Var8 = builder.NewArray<Ttype8>(1, this.Variable8.Length);
            Array.Copy(this.Variable8.Values.ToArray(), Var8.Data, this.Variable8.Length);
            var VarMat8 = builder.NewVariable("Var8", Var8);


            var Var9 = builder.NewArray<Ttype9>(1, this.Variable9.Length);
            Array.Copy(this.Variable9.Values.ToArray(), Var9.Data, this.Variable9.Length);
            var VarMat9 = builder.NewVariable("Var9", Var9);


            var Var10 = builder.NewArray<Ttype10>(1, this.Variable10.Length);
            Array.Copy(this.Variable10.Values.ToArray(), Var10.Data, this.Variable10.Length);
            var VarMat10 = builder.NewVariable("Var10", Var10);


            var Var11 = builder.NewArray<Ttype11>(1, this.Variable11.Length);
            Array.Copy(this.Variable11.Values.ToArray(), Var11.Data, this.Variable11.Length);
            var VarMat11 = builder.NewVariable("Var11", Var11);


            var Var12 = builder.NewArray<Ttype12>(1, this.Variable12.Length);
            Array.Copy(this.Variable12.Values.ToArray(), Var12.Data, this.Variable12.Length);
            var VarMat12 = builder.NewVariable("Var12", Var12);


            var Var13 = builder.NewArray<Ttype13>(1, this.Variable13.Length);
            Array.Copy(this.Variable13.Values.ToArray(), Var13.Data, this.Variable13.Length);
            var VarMat13 = builder.NewVariable("Var13", Var13);


            var Var14 = builder.NewArray<Ttype14>(1, this.Variable14.Length);
            Array.Copy(this.Variable14.Values.ToArray(), Var14.Data, this.Variable14.Length);
            var VarMat14 = builder.NewVariable("Var14", Var14);





            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4
            , VarMat5
            , VarMat6
            , VarMat7
            , VarMat8
            , VarMat9
            , VarMat10
            , VarMat11
            , VarMat12
            , VarMat13
            , VarMat14

              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer14
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
     , Ttype11
     , Ttype12
     , Ttype13
     , Ttype14
    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer14(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 14;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);
            Variable5 = new MyDeSerializerContainer<Ttype5>(namevar5, lengthvar5);
            Variable6 = new MyDeSerializerContainer<Ttype6>(namevar6, lengthvar6);
            Variable7 = new MyDeSerializerContainer<Ttype7>(namevar7, lengthvar7);
            Variable8 = new MyDeSerializerContainer<Ttype8>(namevar8, lengthvar8);
            Variable9 = new MyDeSerializerContainer<Ttype9>(namevar9, lengthvar9);
            Variable10 = new MyDeSerializerContainer<Ttype10>(namevar10, lengthvar10);
            Variable11 = new MyDeSerializerContainer<Ttype11>(namevar11, lengthvar11);
            Variable12 = new MyDeSerializerContainer<Ttype12>(namevar12, lengthvar12);
            Variable13 = new MyDeSerializerContainer<Ttype13>(namevar13, lengthvar13);
            Variable14 = new MyDeSerializerContainer<Ttype14>(namevar14, lengthvar14);



        }



        public MyDeSerializer14(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4
            , MyDeSerializerContainer<Ttype5> variable5
            , MyDeSerializerContainer<Ttype6> variable6
            , MyDeSerializerContainer<Ttype7> variable7
            , MyDeSerializerContainer<Ttype8> variable8
            , MyDeSerializerContainer<Ttype9> variable9
            , MyDeSerializerContainer<Ttype10> variable10
            , MyDeSerializerContainer<Ttype11> variable11
            , MyDeSerializerContainer<Ttype12> variable12
            , MyDeSerializerContainer<Ttype13> variable13
            , MyDeSerializerContainer<Ttype14> variable14

            )
        {
            NumOfVars = 14;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;
            Variable5 = variable5;
            Variable6 = variable6;
            Variable7 = variable7;
            Variable8 = variable8;
            Variable9 = variable9;
            Variable10 = variable10;
            Variable11 = variable11;
            Variable12 = variable12;
            Variable13 = variable13;
            Variable14 = variable14;

        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4
, List<Ttype5> data5
, List<Ttype6> data6
, List<Ttype7> data7
, List<Ttype8> data8
, List<Ttype9> data9
, List<Ttype10> data10
, List<Ttype11> data11
, List<Ttype12> data12
, List<Ttype13> data13
, List<Ttype14> data14

)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }


            for (int i = 0; i < data5.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype5>(data5[i]));
            }


            for (int i = 0; i < data6.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype6>(data6[i]));
            }


            for (int i = 0; i < data7.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype7>(data7[i]));
            }


            for (int i = 0; i < data8.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype8>(data8[i]));
            }


            for (int i = 0; i < data9.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype9>(data9[i]));
            }


            for (int i = 0; i < data10.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype10>(data10[i]));
            }


            for (int i = 0; i < data11.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype11>(data11[i]));
            }


            for (int i = 0; i < data12.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype12>(data12[i]));
            }


            for (int i = 0; i < data13.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype13>(data13[i]));
            }


            for (int i = 0; i < data14.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype14>(data14[i]));
            }



            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length
                + Marshal.SizeOf<Ttype5>() * Variable5.Length
                + Marshal.SizeOf<Ttype6>() * Variable6.Length
                + Marshal.SizeOf<Ttype7>() * Variable7.Length
                + Marshal.SizeOf<Ttype8>() * Variable8.Length
                + Marshal.SizeOf<Ttype9>() * Variable9.Length
                + Marshal.SizeOf<Ttype10>() * Variable10.Length
                + Marshal.SizeOf<Ttype11>() * Variable11.Length
                + Marshal.SizeOf<Ttype12>() * Variable12.Length
                + Marshal.SizeOf<Ttype13>() * Variable13.Length
                + Marshal.SizeOf<Ttype14>() * Variable14.Length
              ;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype5>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable5.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable5.Values[i / sizeOfDatavar] = (Ttype5)mybitConverter<Ttype5>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype6>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable6.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable6.Values[i / sizeOfDatavar] = (Ttype6)mybitConverter<Ttype6>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype7>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable7.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable7.Values[i / sizeOfDatavar] = (Ttype7)mybitConverter<Ttype7>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype8>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable8.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable8.Values[i / sizeOfDatavar] = (Ttype8)mybitConverter<Ttype8>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype9>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable9.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable9.Values[i / sizeOfDatavar] = (Ttype9)mybitConverter<Ttype9>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype10>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable10.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable10.Values[i / sizeOfDatavar] = (Ttype10)mybitConverter<Ttype10>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype11>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable11.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable11.Values[i / sizeOfDatavar] = (Ttype11)mybitConverter<Ttype11>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype12>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable12.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable12.Values[i / sizeOfDatavar] = (Ttype12)mybitConverter<Ttype12>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype13>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable13.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable13.Values[i / sizeOfDatavar] = (Ttype13)mybitConverter<Ttype13>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype14>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable14.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable14.Values[i / sizeOfDatavar] = (Ttype14)mybitConverter<Ttype14>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);





            return true;
        }


    }






    public class MyDeSerializer15<
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
     , Ttype11
     , Ttype12
     , Ttype13
     , Ttype14
     , Ttype15
    > : IMyDeSerializer
     where Ttype1 : struct
     where Ttype2 : struct
     where Ttype3 : struct
     where Ttype4 : struct
     where Ttype5 : struct
     where Ttype6 : struct
     where Ttype7 : struct
     where Ttype8 : struct
     where Ttype9 : struct
     where Ttype10 : struct
     where Ttype11 : struct
     where Ttype12 : struct
     where Ttype13 : struct
     where Ttype14 : struct
     where Ttype15 : struct
    {


        public MyDeSerializerContainer<Ttype1> Variable1 { get; private set; }
        public MyDeSerializerContainer<Ttype2> Variable2 { get; private set; }
        public MyDeSerializerContainer<Ttype3> Variable3 { get; private set; }
        public MyDeSerializerContainer<Ttype4> Variable4 { get; private set; }
        public MyDeSerializerContainer<Ttype5> Variable5 { get; private set; }
        public MyDeSerializerContainer<Ttype6> Variable6 { get; private set; }
        public MyDeSerializerContainer<Ttype7> Variable7 { get; private set; }
        public MyDeSerializerContainer<Ttype8> Variable8 { get; private set; }
        public MyDeSerializerContainer<Ttype9> Variable9 { get; private set; }
        public MyDeSerializerContainer<Ttype10> Variable10 { get; private set; }
        public MyDeSerializerContainer<Ttype11> Variable11 { get; private set; }
        public MyDeSerializerContainer<Ttype12> Variable12 { get; private set; }
        public MyDeSerializerContainer<Ttype13> Variable13 { get; private set; }
        public MyDeSerializerContainer<Ttype14> Variable14 { get; private set; }
        public MyDeSerializerContainer<Ttype15> Variable15 { get; private set; }



        public IMatFile ConvertMyDeserializeVarsToMatFile()
        {
            var builder = new DataBuilder();



            var Var1 = builder.NewArray<Ttype1>(1, this.Variable1.Length);
            Array.Copy(this.Variable1.Values.ToArray(), Var1.Data, this.Variable1.Length);
            var VarMat1 = builder.NewVariable("Var1", Var1);


            var Var2 = builder.NewArray<Ttype2>(1, this.Variable2.Length);
            Array.Copy(this.Variable2.Values.ToArray(), Var2.Data, this.Variable2.Length);
            var VarMat2 = builder.NewVariable("Var2", Var2);


            var Var3 = builder.NewArray<Ttype3>(1, this.Variable3.Length);
            Array.Copy(this.Variable3.Values.ToArray(), Var3.Data, this.Variable3.Length);
            var VarMat3 = builder.NewVariable("Var3", Var3);


            var Var4 = builder.NewArray<Ttype4>(1, this.Variable4.Length);
            Array.Copy(this.Variable4.Values.ToArray(), Var4.Data, this.Variable4.Length);
            var VarMat4 = builder.NewVariable("Var4", Var4);


            var Var5 = builder.NewArray<Ttype5>(1, this.Variable5.Length);
            Array.Copy(this.Variable5.Values.ToArray(), Var5.Data, this.Variable5.Length);
            var VarMat5 = builder.NewVariable("Var5", Var5);


            var Var6 = builder.NewArray<Ttype6>(1, this.Variable6.Length);
            Array.Copy(this.Variable6.Values.ToArray(), Var6.Data, this.Variable6.Length);
            var VarMat6 = builder.NewVariable("Var6", Var6);


            var Var7 = builder.NewArray<Ttype7>(1, this.Variable7.Length);
            Array.Copy(this.Variable7.Values.ToArray(), Var7.Data, this.Variable7.Length);
            var VarMat7 = builder.NewVariable("Var7", Var7);


            var Var8 = builder.NewArray<Ttype8>(1, this.Variable8.Length);
            Array.Copy(this.Variable8.Values.ToArray(), Var8.Data, this.Variable8.Length);
            var VarMat8 = builder.NewVariable("Var8", Var8);


            var Var9 = builder.NewArray<Ttype9>(1, this.Variable9.Length);
            Array.Copy(this.Variable9.Values.ToArray(), Var9.Data, this.Variable9.Length);
            var VarMat9 = builder.NewVariable("Var9", Var9);


            var Var10 = builder.NewArray<Ttype10>(1, this.Variable10.Length);
            Array.Copy(this.Variable10.Values.ToArray(), Var10.Data, this.Variable10.Length);
            var VarMat10 = builder.NewVariable("Var10", Var10);


            var Var11 = builder.NewArray<Ttype11>(1, this.Variable11.Length);
            Array.Copy(this.Variable11.Values.ToArray(), Var11.Data, this.Variable11.Length);
            var VarMat11 = builder.NewVariable("Var11", Var11);


            var Var12 = builder.NewArray<Ttype12>(1, this.Variable12.Length);
            Array.Copy(this.Variable12.Values.ToArray(), Var12.Data, this.Variable12.Length);
            var VarMat12 = builder.NewVariable("Var12", Var12);


            var Var13 = builder.NewArray<Ttype13>(1, this.Variable13.Length);
            Array.Copy(this.Variable13.Values.ToArray(), Var13.Data, this.Variable13.Length);
            var VarMat13 = builder.NewVariable("Var13", Var13);


            var Var14 = builder.NewArray<Ttype14>(1, this.Variable14.Length);
            Array.Copy(this.Variable14.Values.ToArray(), Var14.Data, this.Variable14.Length);
            var VarMat14 = builder.NewVariable("Var14", Var14);


            var Var15 = builder.NewArray<Ttype15>(1, this.Variable15.Length);
            Array.Copy(this.Variable15.Values.ToArray(), Var15.Data, this.Variable15.Length);
            var VarMat15 = builder.NewVariable("Var15", Var15);




            var actual = builder.NewFile(new[] {
            VarMat1
            , VarMat2
            , VarMat3
            , VarMat4
            , VarMat5
            , VarMat6
            , VarMat7
            , VarMat8
            , VarMat9
            , VarMat10
            , VarMat11
            , VarMat12
            , VarMat13
            , VarMat14
            , VarMat15
              });


            return actual;

        }






        private object mybitConverter<T>(byte[] bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return (object)System.BitConverter.ToSingle(bytesdata, 0);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return (object)System.BitConverter.ToDouble(bytesdata, 0);
            }
            else if (t == typeof(bool))
            {
                return (object)System.BitConverter.ToBoolean(bytesdata, 0);
            }
            else if (t == typeof(char))
            {
                return (char)bytesdata[0];
            }
            else if (t == typeof(Int16))
            {
                return (object)System.BitConverter.ToInt16(bytesdata, 0);
            }
            else if (t == typeof(Int32))
            {
                return (object)System.BitConverter.ToInt32(bytesdata, 0);
            }
            else if (t == typeof(UInt16))
            {
                return (object)System.BitConverter.ToUInt16(bytesdata, 0);
            }
            else if (t == typeof(UInt32))
            {
                return (object)System.BitConverter.ToUInt32(bytesdata, 0);
            }


            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }




        private byte[] MybitConverterSerialize<T>(object bytesdata)
        {
            Type t = typeof(T);
            if (t == typeof(float))
            {
                return System.BitConverter.GetBytes((float)bytesdata);
            }
            /* you shouldnt ever need string
            else if (t == typeof(string))
            {
                return (object)System.BitConverter.ToString(bytesdata, 0);
            }
			*/
            else if (t == typeof(double))
            {
                return System.BitConverter.GetBytes((double)bytesdata);
            }
            else if (t == typeof(bool))
            {
                return System.BitConverter.GetBytes((bool)bytesdata);
            }
            else if (t == typeof(char))
            {
                byte[] o = new byte[1];
                o[0] = (byte)(char)bytesdata;
                return o;//System.BitConverter.GetBytes((char)bytesdata); ;
            }
            else if (t == typeof(Int16))
            {
                return System.BitConverter.GetBytes((Int16)bytesdata);
            }
            else if (t == typeof(Int32))
            {
                return System.BitConverter.GetBytes((Int32)bytesdata);
            }
            else if (t == typeof(UInt16))
            {
                return System.BitConverter.GetBytes((UInt16)bytesdata);
            }
            else if (t == typeof(UInt32))
            {
                return System.BitConverter.GetBytes((UInt32)bytesdata);
            }

            throw new FormatException("you picked a data type that bitconverter cant handle. if you meant uint8_t, that will be char");
            return null;
        }


        public int NumOfVars { get; set; }






        public static IMyDeSerializer GetNewDeserializer(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
            )
        {
            return (IMyDeSerializer)(new MyDeSerializer15
            <
     Ttype1
     , Ttype2
     , Ttype3
     , Ttype4
     , Ttype5
     , Ttype6
     , Ttype7
     , Ttype8
     , Ttype9
     , Ttype10
     , Ttype11
     , Ttype12
     , Ttype13
     , Ttype14
     , Ttype15
    >
            (
                namevar1, lengthvar1
                , namevar2, lengthvar2
                , namevar3, lengthvar3
                , namevar4, lengthvar4
                , namevar5, lengthvar5
                , namevar6, lengthvar6
                , namevar7, lengthvar7
                , namevar8, lengthvar8
                , namevar9, lengthvar9
                , namevar10, lengthvar10
                , namevar11, lengthvar11
                , namevar12, lengthvar12
                , namevar13, lengthvar13
                , namevar14, lengthvar14
                , namevar15, lengthvar15
            ));
        }



        public MyDeSerializer15(
            string namevar1, int lengthvar1
            , string namevar2, int lengthvar2
            , string namevar3, int lengthvar3
            , string namevar4, int lengthvar4
            , string namevar5, int lengthvar5
            , string namevar6, int lengthvar6
            , string namevar7, int lengthvar7
            , string namevar8, int lengthvar8
            , string namevar9, int lengthvar9
            , string namevar10, int lengthvar10
            , string namevar11, int lengthvar11
            , string namevar12, int lengthvar12
            , string namevar13, int lengthvar13
            , string namevar14, int lengthvar14
            , string namevar15, int lengthvar15
        )
        {
            NumOfVars = 15;

            Variable1 = new MyDeSerializerContainer<Ttype1>(namevar1, lengthvar1);
            Variable2 = new MyDeSerializerContainer<Ttype2>(namevar2, lengthvar2);
            Variable3 = new MyDeSerializerContainer<Ttype3>(namevar3, lengthvar3);
            Variable4 = new MyDeSerializerContainer<Ttype4>(namevar4, lengthvar4);
            Variable5 = new MyDeSerializerContainer<Ttype5>(namevar5, lengthvar5);
            Variable6 = new MyDeSerializerContainer<Ttype6>(namevar6, lengthvar6);
            Variable7 = new MyDeSerializerContainer<Ttype7>(namevar7, lengthvar7);
            Variable8 = new MyDeSerializerContainer<Ttype8>(namevar8, lengthvar8);
            Variable9 = new MyDeSerializerContainer<Ttype9>(namevar9, lengthvar9);
            Variable10 = new MyDeSerializerContainer<Ttype10>(namevar10, lengthvar10);
            Variable11 = new MyDeSerializerContainer<Ttype11>(namevar11, lengthvar11);
            Variable12 = new MyDeSerializerContainer<Ttype12>(namevar12, lengthvar12);
            Variable13 = new MyDeSerializerContainer<Ttype13>(namevar13, lengthvar13);
            Variable14 = new MyDeSerializerContainer<Ttype14>(namevar14, lengthvar14);
            Variable15 = new MyDeSerializerContainer<Ttype15>(namevar15, lengthvar15);


        }



        public MyDeSerializer15(
            MyDeSerializerContainer<Ttype1> variable1
            , MyDeSerializerContainer<Ttype2> variable2
            , MyDeSerializerContainer<Ttype3> variable3
            , MyDeSerializerContainer<Ttype4> variable4
            , MyDeSerializerContainer<Ttype5> variable5
            , MyDeSerializerContainer<Ttype6> variable6
            , MyDeSerializerContainer<Ttype7> variable7
            , MyDeSerializerContainer<Ttype8> variable8
            , MyDeSerializerContainer<Ttype9> variable9
            , MyDeSerializerContainer<Ttype10> variable10
            , MyDeSerializerContainer<Ttype11> variable11
            , MyDeSerializerContainer<Ttype12> variable12
            , MyDeSerializerContainer<Ttype13> variable13
            , MyDeSerializerContainer<Ttype14> variable14
            , MyDeSerializerContainer<Ttype15> variable15
            )
        {
            NumOfVars = 15;

            Variable1 = variable1;
            Variable2 = variable2;
            Variable3 = variable3;
            Variable4 = variable4;
            Variable5 = variable5;
            Variable6 = variable6;
            Variable7 = variable7;
            Variable8 = variable8;
            Variable9 = variable9;
            Variable10 = variable10;
            Variable11 = variable11;
            Variable12 = variable12;
            Variable13 = variable13;
            Variable14 = variable14;
            Variable15 = variable15;
        }





        public List<byte> Serialize(
List<Ttype1> data1
, List<Ttype2> data2
, List<Ttype3> data3
, List<Ttype4> data4
, List<Ttype5> data5
, List<Ttype6> data6
, List<Ttype7> data7
, List<Ttype8> data8
, List<Ttype9> data9
, List<Ttype10> data10
, List<Ttype11> data11
, List<Ttype12> data12
, List<Ttype13> data13
, List<Ttype14> data14
, List<Ttype15> data15
)
        {

            List<byte> efef = new List<byte>();


            for (int i = 0; i < data1.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype1>(data1[i]));
            }



            for (int i = 0; i < data2.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype2>(data2[i]));
            }



            for (int i = 0; i < data3.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype3>(data3[i]));
            }



            for (int i = 0; i < data4.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype4>(data4[i]));
            }


            for (int i = 0; i < data5.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype5>(data5[i]));
            }


            for (int i = 0; i < data6.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype6>(data6[i]));
            }


            for (int i = 0; i < data7.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype7>(data7[i]));
            }


            for (int i = 0; i < data8.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype8>(data8[i]));
            }


            for (int i = 0; i < data9.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype9>(data9[i]));
            }


            for (int i = 0; i < data10.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype10>(data10[i]));
            }


            for (int i = 0; i < data11.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype11>(data11[i]));
            }


            for (int i = 0; i < data12.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype12>(data12[i]));
            }


            for (int i = 0; i < data13.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype13>(data13[i]));
            }


            for (int i = 0; i < data14.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype14>(data14[i]));
            }


            for (int i = 0; i < data15.Count; i++)
            {
                efef.AddRange(MybitConverterSerialize<Ttype15>(data15[i]));
            }


            return efef;

        }




        public bool Deserialize(List<byte> Data)
        {


            //first do a confirmation that the Data size is bigger then the data
            //needed to serialize all variables
            int sizeNeeded =
                Marshal.SizeOf<Ttype1>() * Variable1.Length
                + Marshal.SizeOf<Ttype2>() * Variable2.Length
                + Marshal.SizeOf<Ttype3>() * Variable3.Length
                + Marshal.SizeOf<Ttype4>() * Variable4.Length
                + Marshal.SizeOf<Ttype5>() * Variable5.Length
                + Marshal.SizeOf<Ttype6>() * Variable6.Length
                + Marshal.SizeOf<Ttype7>() * Variable7.Length
                + Marshal.SizeOf<Ttype8>() * Variable8.Length
                + Marshal.SizeOf<Ttype9>() * Variable9.Length
                + Marshal.SizeOf<Ttype10>() * Variable10.Length
                + Marshal.SizeOf<Ttype11>() * Variable11.Length
                + Marshal.SizeOf<Ttype12>() * Variable12.Length
                + Marshal.SizeOf<Ttype13>() * Variable13.Length
                + Marshal.SizeOf<Ttype14>() * Variable14.Length
                + Marshal.SizeOf<Ttype15>() * Variable15.Length;
            if (sizeNeeded > Data.Count)
            {
                return false;
                throw new FormatException("Data given is not enough to serialize the variables");
            }





            //first get the data of the variable I am deserializing
            byte[] DataOfVar = Data.ToArray();
            int sizeOfDatavar = Marshal.SizeOf<Ttype1>();
            Array.Resize(ref DataOfVar, (Variable1.Length) * sizeOfDatavar);


            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                var ttt = DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray();
                Variable1.Values[i / sizeOfDatavar] = (Ttype1)mybitConverter<Ttype1>(ttt);//(object)System.BitConverter.ToSingle(ttt, 0);
            }

            Data.RemoveRange(0, DataOfVar.Length);






            sizeOfDatavar = Marshal.SizeOf<Ttype2>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable2.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable2.Values[i / sizeOfDatavar] = (Ttype2)mybitConverter<Ttype2>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype3>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable3.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable3.Values[i / sizeOfDatavar] = (Ttype3)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype4>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable4.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable4.Values[i / sizeOfDatavar] = (Ttype4)mybitConverter<Ttype4>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype5>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable5.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable5.Values[i / sizeOfDatavar] = (Ttype5)mybitConverter<Ttype5>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype6>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable6.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable6.Values[i / sizeOfDatavar] = (Ttype6)mybitConverter<Ttype6>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype7>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable7.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable7.Values[i / sizeOfDatavar] = (Ttype7)mybitConverter<Ttype7>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype8>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable8.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable8.Values[i / sizeOfDatavar] = (Ttype8)mybitConverter<Ttype8>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype9>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable9.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable9.Values[i / sizeOfDatavar] = (Ttype9)mybitConverter<Ttype9>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype10>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable10.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable10.Values[i / sizeOfDatavar] = (Ttype10)mybitConverter<Ttype10>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype11>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable11.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable11.Values[i / sizeOfDatavar] = (Ttype11)mybitConverter<Ttype11>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype12>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable12.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable12.Values[i / sizeOfDatavar] = (Ttype12)mybitConverter<Ttype12>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype13>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable13.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable13.Values[i / sizeOfDatavar] = (Ttype13)mybitConverter<Ttype13>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype14>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable14.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable14.Values[i / sizeOfDatavar] = (Ttype14)mybitConverter<Ttype14>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);




            sizeOfDatavar = Marshal.SizeOf<Ttype15>();
            DataOfVar = Data.ToArray();
            Array.Resize(ref DataOfVar, (Variable15.Length) * sizeOfDatavar);
            for (int i = 0; i < DataOfVar.Length; i += sizeOfDatavar)
            {
                Variable15.Values[i / sizeOfDatavar] = (Ttype15)mybitConverter<Ttype3>(DataOfVar.Skip(i).Take(sizeOfDatavar).ToArray());
            }

            Data.RemoveRange(0, DataOfVar.Length);



            return true;
        }


    }






}