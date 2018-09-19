using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class ReceiveMessage
{
    public DataSendType type;
    public int dataLength;
    public byte[] data;

    public ReceiveMessage()
    {
        Clear();
    }

    public void Clear()
    {
        type = DataSendType.NONE;
        dataLength = 0;
        data = new byte[0];
    }
}


public delegate void OnReceiveData(ReceiveMessage message);
public class DynamicBufferManager
{

    public byte[] Buffer { get; set; } //存放内存的数组
    public int DataCount { get; set; } //写入数据的大小

    ReceiveMessage ReceiveMessage = new ReceiveMessage();
    private OnReceiveData OnReceiveData;

    public DynamicBufferManager(int bufferSize, OnReceiveData OnReceiveData)
    {
        DataCount = 0;
        Buffer = new byte[bufferSize];
        this.OnReceiveData = OnReceiveData;
    }

    public int GetDataCount() //获得当前写入的字节数
    {
        return DataCount;
    }

    public int GetReserveCount() //获得剩余的字节数
    {
        return Buffer.Length - DataCount;
    }

    public void Clear(int count) //清理指定大小的数据
    {
        if (count > DataCount) //如果需要清理的数据大于了已有的，则全部清空
        {
            DataCount = 0;
        }
        else
        {
            for (int i = 0; i < DataCount - count; i++) //否则后面的数据前移
            {
                Buffer[i] = Buffer[count + i];
            }
            DataCount = DataCount - count;
        }
    }

    public void WriteBuffer(byte[] buffer, int offset, int count)
    {
        if (ReceiveMessage.type.Equals(DataSendType.NONE))
        {
            if (buffer.Length == 0 || buffer.Length < 8) return;

            byte[] type = new byte[4];
            byte[] length = new byte[4];

            Array.Copy(buffer, 0, type, 0, type.Length);
            Array.Copy(buffer, type.Length, length, 0, length.Length);

            ReceiveMessage.type = (DataSendType)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(type, 0));
            ReceiveMessage.dataLength = IPAddress.NetworkToHostOrder( BitConverter.ToInt32(length, 0));


            //Debug.Log(ReceiveMessage.dataLength);
        }


        if (GetReserveCount() >= count) //缓存空间够用，不需要申请
        {
            Array.Copy(buffer, offset, Buffer, DataCount, count);
            DataCount = DataCount + count;
        }
        else //缓存区不够用，需要申请，并进行移位
        {
            int totalSize = Buffer.Length + count - GetReserveCount();

            byte[] tempBuffer = new byte[totalSize];
            Array.Copy(Buffer, 0, tempBuffer, 0, DataCount);
            Array.Copy(buffer, offset, tempBuffer, DataCount, count);

            DataCount = DataCount + count;
            Buffer = tempBuffer;
        }

        if (DataCount >= ReceiveMessage.dataLength)
        {
            ReceiveMessage message =  GetData();
            OnReceiveData(message);                    
            Clear(ReceiveMessage.dataLength);
            ReceiveMessage.Clear();
        }
    }

    public void WriteBuffer(byte[] buffer)
    {
        WriteBuffer(buffer, 0, buffer.Length);
    }

    public void WriteInt(int value)
    {
        value = System.Net.IPAddress.HostToNetworkOrder(value);
        byte[] buffer = BitConverter.GetBytes(value);
        WriteBuffer(buffer);
    }

    public void WriteString(string value)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(value);
        WriteBuffer(buffer);
    }


    public ReceiveMessage GetData()
    {
        if (ReceiveMessage.type.Equals(DataSendType.NONE)) return null;

        ReceiveMessage.data = new byte[ReceiveMessage.dataLength - 8];

        Array.Copy(Buffer, 8, ReceiveMessage.data, 0, ReceiveMessage.dataLength - 8);

        return ReceiveMessage;

    }
}

