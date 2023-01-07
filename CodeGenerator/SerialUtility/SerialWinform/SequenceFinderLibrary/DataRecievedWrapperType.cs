using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolterMonitorGui
{


    public class DataRecievedWrapperType
    {

        public int Length
        {
            get { return DataAsString.Length; }
        }

        public List<byte> DataAsBytesBuffer { get; private set; }
        public string DataAsString { get; private set; }

        public DataRecievedWrapperType()
        {
            DataAsBytesBuffer = new List<byte>(1000);
        }

        public DataRecievedWrapperType(DataRecievedWrapperType fromSource)
        {
            this.DataAsBytesBuffer = new List<byte>(fromSource.DataAsBytesBuffer);
            this.DataAsString = fromSource.DataAsString;  
        }



        public void SetNewData(byte[] dataAsBytes)
        {
            SetNewData(dataAsBytes, dataAsBytes.Length); 
        }


        public void SetNewData(byte[] dataAsBytes, int sizeOfData)
        { 
            DataAsString = "";
            DataAsBytesBuffer.Clear();

            Array.Resize(ref dataAsBytes, sizeOfData);
            DataAsBytesBuffer = dataAsBytes.ToList();
            DataAsString = System.Text.Encoding.ASCII.GetString(dataAsBytes, 0, DataAsBytesBuffer.Count);
            
            //Array.Copy(dataAsBytes, DataAsBytesBuffer, dataAsBytes.Length);
        }


        public DataRecievedWrapperType AppendData(DataRecievedWrapperType data)
        {
            DataAsString = this.DataAsString + data.DataAsString;
            this.DataAsBytesBuffer.AddRange(data.DataAsBytesBuffer);
            return this;
        }

        public void Remove(int indexStart, int count)
        {
            DataAsString = DataAsString.Remove(indexStart, count);
            DataAsBytesBuffer.RemoveRange(indexStart, count);

        }

        public void Substring(int indexStart, int count)
        {
            DataAsString = DataAsString.Substring(indexStart, count);
            DataAsBytesBuffer.RemoveRange(indexStart + count, (DataAsBytesBuffer.Count - (indexStart + count)));
            DataAsBytesBuffer.RemoveRange(0, indexStart);

        }

        public bool Contains(string str)
        {
            return DataAsString.Contains(str);
        }

        public void Clear()
        {
            DataAsString = "";
            DataAsBytesBuffer.Clear();
        }



        //name
        //type
        //
       /* public MyDeSerializer Deserialize(MyDeSerializer)
        {

        }*/

         
    }
}
