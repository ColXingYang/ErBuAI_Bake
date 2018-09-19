using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

using System.Threading;

public delegate void SZHReceiveCallBack(byte[] content);

public enum ReceiceDataType
{
    Init,
    Step,
    Reset
}

public enum DataSendType
{
    Init,
    Step,
    Reset,

    Result,

    IMAGE,
    COLLIDER,
    GOODPOS,

    CAMERA,
    JXBDATA,
    CAPUTERSCREENSHOTFRAME,

    NONE,
}

[Serializable]
/// <summary>
/// 尺寸
/// </summary>
public class Size
{
    public float length;
    public float width;
    public float heigth;

    public Size() { }
    public Size(float length,float width,float heigth)
    {
        this.length = length;
        this.width = width;
        this.heigth = heigth;
    }
}

[Serializable]
/// <summary>
/// 位置3维
/// </summary>
public class Pos3
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
/// <summary>
/// 位置2维
/// </summary>
public class Pos2
{
    public float x;
    public float y;
    public Pos2()
    {
        x = 0;
        y = 0;
    }
}

[Serializable]
/// <summary>
/// 货物数据
/// </summary>
public class GoodData
{
    /// <summary>
    /// 尺寸
    /// </summary>
    public Size goodSize;
    /// <summary>
    /// 位置
    /// </summary>
    public Pos2 goodPos;
    /// <summary>
    /// 支架高度
    /// </summary>
    public float legHeigth;
    /// <summary>
    /// 摩擦系数
    /// </summary>
    public float COF;
    /// <summary>
    /// 质量
    /// </summary>
    public float mass;
    public GoodData()
    {
        goodSize = new Size(SceneSizeOptions.Good.length, SceneSizeOptions.Good.width, SceneSizeOptions.Good.heigth);
        legHeigth = SceneSizeOptions.GoodLegHeigth;
        COF = 1.0f;
        mass = 10.0f;
        goodPos = new Pos2();
    }
}

[Serializable]
/// <summary>
/// 仿真数据
/// </summary>
public class SimulateData
{
    /// <summary>
    /// 仿真步长
    /// </summary>
    public float deltaTime;
    /// <summary>
    /// 倍率
    /// </summary>
    public int k;

    public SimulateData()
    {
        deltaTime = 0.02f;
        k = 1;
    }
}

[Serializable]
/// <summary>
/// 机械臂角度数据
/// </summary>
public class JXBAngleData
{
    /// <summary>
    /// 第一个关节Y轴的角度
    /// </summary>
    public float joint1_YAxis_Angle;
    /// <summary>
    /// 第二个关节Z轴的角度
    /// </summary>
    public float joint2_ZAxis_Angle;
    /// <summary>
    /// 第三个关节Z轴的角度
    /// </summary>
    public float joint3_ZAxis_Angle;
    /// <summary>
    /// 第四个关节X轴的角度
    /// </summary>
    public float joint4_XAxis_Angle;
    /// <summary>
    /// 第五个关节Z轴的角度
    /// </summary>
    public float joint5_ZAxis_Angle;

    public JXBAngleData()
    {
        joint1_YAxis_Angle = 180;
        joint2_ZAxis_Angle = 0;
        joint3_ZAxis_Angle = -28;
        joint4_XAxis_Angle = 0;
        joint5_ZAxis_Angle = 28;
    }
}

[Serializable]
/// <summary>
/// 机械臂角速度信息
/// </summary>
public class JXBWData
{
    /// <summary>
    /// 第一个关节的角速度
    /// </summary>
    public float joint1_w;
    /// <summary>
    /// 第二个关节的角速度
    /// </summary>
    public float joint2_w;
    /// <summary>
    /// 第三个关节的角速度
    /// </summary>
    public float joint3_w;
    /// <summary>
    /// 第四个关节的角速度
    /// </summary>
    public float joint4_w;
    /// <summary>
    /// 第五个关节的角速度
    /// </summary>
    public float joint5_w;

    public JXBWData()
    {
        joint1_w = 10;
        joint2_w = 10;
        joint3_w = 10;
        joint4_w = 10;
        joint5_w = 10;
    }
}
/// <summary>
/// 接收初始化数据
/// </summary>
public class ReceiveInitData
{
    /// <summary>
    /// 货物信息
    /// </summary>
    public GoodData goodInfo;
    /// <summary>
    /// 机械臂角度信息
    /// </summary>
    public JXBAngleData jxbAngleInfo;
    /// <summary>
    /// 仿真信息
    /// </summary>
    public SimulateData simulateInfo;

    public ReceiveInitData()
    {
        goodInfo = new GoodData();
        jxbAngleInfo = new JXBAngleData();
        simulateInfo = new SimulateData();
    }
}

public class ReceiveStepData
{
    public JXBWData jxbWInfo;

    public ReceiveStepData()
    {
        jxbWInfo = new JXBWData();
    }
}

public class SendInitData
{
    public Size goodSize;
    public Size deskSize;
}



public class ResetData
{

}

public enum SendDataType
{
    Result
}

public class ResultData
{
    public Pos3 joint1_pos;
    public Pos3 joint2_pos;
    public Pos3 joint3_pos;
    public Pos3 joint4_pos;
    public Pos3 joint5_pos;
    public Pos3 target_pos;


    public Pos3 tpoint1_pos;
    public Pos3 tpoint2_pos;
    public Pos3 tpoint3_pos;
    public Pos3 tpoint4_pos;
    public Pos3 hpoint1_pos;
    public Pos3 hpoint2_pos;
    public Pos3 hpoint3_pos;
    public Pos3 hpoint4_pos;

    public Pos3 jpoint1_ang;
    public Pos3 jpoint2_ang;
    public Pos3 jpoint3_ang;
    public Pos3 jpoint4_ang;
    public Pos3 jpoint5_ang;

    public byte[] image;
    public ColliderArea colliderArea;

    public ResultData()
    {
        joint1_pos = new Pos3();
        joint2_pos = new Pos3();
        joint3_pos = new Pos3();
        joint4_pos = new Pos3();
        joint5_pos = new Pos3();
        target_pos = new Pos3();


        tpoint1_pos = new Pos3();
        tpoint2_pos = new Pos3();
        tpoint3_pos = new Pos3();
        tpoint4_pos = new Pos3();
        hpoint1_pos = new Pos3();
        hpoint2_pos = new Pos3();
        hpoint3_pos = new Pos3();
        hpoint4_pos = new Pos3();

        jpoint1_ang = new Pos3();
        jpoint2_ang = new Pos3();
        jpoint3_ang = new Pos3();
        jpoint4_ang = new Pos3();
        jpoint5_ang = new Pos3();


        image = new byte[0];
        colliderArea = ColliderArea.NONE;
    }
}

public class SZHSocket
{
    #region 服务器
    //声明一个服务器端的套接字
    Socket serverSocket;
    //声明一个委托对象
    SZHReceiveCallBack serverCallBack;
    public OnReceiveData OnServerReceiveData;
    const int reciveSize = 1024;
    //生成一个动态数据缓存
    DynamicBufferManager serverBuffer ;

    AsyncCallback asyncCallback_ServerAccept;
    AsyncCallback asyncCallback_ServerReceive;

    Thread connectThread; //连接线程
    EndPoint clientEnd; //客户端
    //初始化服务器
    public void InitServer(/*SZHReceiveCallBack serverCallBack*/)
    {
        serverBuffer = new DynamicBufferManager(reciveSize, OnServerReceive);

        //this.serverCallBack = serverCallBack;

        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 6065);

        serverSocket.Bind(serverEP);

        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        clientEnd = (EndPoint)sender;

        //serverSocket.Listen(10);
        //asyncCallback_ServerAccept = new AsyncCallback(ServerAccept);
        //asyncCallback_ServerReceive = new AsyncCallback(ServerReceive);
        //serverSocket.BeginAccept(asyncCallback_ServerAccept, serverSocket);

        //Debug.Log("Server Has Init");
    }

    //服务器接收
    void SocketReceive()
    {
        //进入接收循环
        while (true)
        {
            //获取客户端，获取客户端数据，用引用给客户端赋值
            int byteCount = serverSocket.ReceiveFrom(tempServerBuffer, ref clientEnd);
            if (byteCount > 0)
            {
                byte[] temp = new byte[byteCount];
                Array.Copy(tempServerBuffer, 0, temp, 0, byteCount);
                serverBuffer.WriteBuffer(temp);
            }
        }
    }


    //连接关闭
   public void SocketQuit()
    {
        //关闭线程
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //最后关闭socket
        if (serverSocket != null)
        {
            serverSocket.Close();
        }
    }

    private void OnServerReceive(ReceiveMessage message)
    {
        OnServerReceiveData(message);
    }

    Socket client;
    byte[] tempServerBuffer = new byte[reciveSize];
    private void ServerAccept(IAsyncResult ar)
    {
        serverSocket = ar.AsyncState as Socket;

        client = serverSocket.EndAccept(ar);
 
        client.BeginReceive(tempServerBuffer, 0, reciveSize, SocketFlags.None, asyncCallback_ServerReceive /*new AsyncCallback(ServerReceive)*/, client);

        client.BeginAccept(asyncCallback_ServerAccept/*new AsyncCallback(ServerAccept)*/, client);
    }

    private void ServerReceive(IAsyncResult ar)
    {
        Socket workingSocket = ar.AsyncState as Socket;

        int byteCount = 0;

        try
        {
            byteCount = workingSocket.EndReceive(ar);
        }
        catch (Exception)
        {
            
        }

        if (byteCount > 0)
        {
            //Send(workingSocket, "服务器收到你方发送的数据");
            byte[] temp = new byte[byteCount];
            Array.Copy(tempServerBuffer, 0, temp, 0, byteCount);
            //serverSocket.BeginAccept(new AsyncCallback(ServerAccept), serverSocket);
            serverBuffer.WriteBuffer(temp);

            workingSocket.BeginReceive(tempServerBuffer, 0, tempServerBuffer.Length, SocketFlags.None, asyncCallback_ServerReceive /*new AsyncCallback(ServerReceive)*/, workingSocket);

        }
        else
        {
            workingSocket.Shutdown(SocketShutdown.Both);
            workingSocket.Close();
            serverSocket.BeginAccept(new System.AsyncCallback(ServerAccept), serverSocket);
        }
    }

    public void Send(Socket sendSocket, byte[] data)
    {
        if (sendSocket == null || data.Length == 0) return;
        try
        {
            //开始发送消息  
            sendSocket.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
            {
                //完成消息发送  
                sendSocket.EndSend(asyncResult);
                //Debug.Log("服务器发送的数据长度：" + data.Length);
                //输出消息  
                //Debug.Log("服务器发出消息");
            }, null);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void USend(byte[] data)
    {
        if (data.Length == 0) return;
        serverSocket.SendTo(data, data.Length, SocketFlags.None, clientEnd);
    }

    public void Send(byte[] data)
    {
        if (client == null || data.Length == 0) return;
        Send(client, data);
    }

    public void Send(string content)
    {
        Send(Encoding.UTF8.GetBytes(content));
    }

    public void Send(Socket sendSocket, string content)
    {
        Send(sendSocket, Encoding.UTF8.GetBytes(content));
    }

    public void DisConnect()
    {
        //if (clientSocket != null)
        //{
        //    clientSocket.Close();
        //}

        if (serverSocket != null)
        {
            serverSocket.Close();
        }
        if (client == null || client.Connected == false)
            return;
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }
    #endregion

    //#region 客户端
    ////声明客户端的套接字
    //Socket clientSocket;

    //SZHReceiveCallBack clientReceiveCallBack;
    //public OnReceiveData OnClientReceiveData;
    //public DynamicBufferManager clientBuffer/* = new DynamicBufferManager(reciveSize, OnReceiveData)*/;

    //byte[] tempClientBuffer = new byte[reciveSize];

    //public void InitClient(string ip,int port,SZHReceiveCallBack clientReceiveCallBack)
    //{
    //    clientBuffer = new DynamicBufferManager(reciveSize, OnClientReceive);
    //    this.clientReceiveCallBack = clientReceiveCallBack;

    //    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    //    IPEndPoint clientEP = new IPEndPoint(IPAddress.Parse(ip), port);

    //    clientSocket.Connect(clientEP);

    //    clientSocket.BeginReceive(tempClientBuffer, 0, reciveSize, SocketFlags.None,
    //     new System.AsyncCallback(clientReceive), this.clientSocket);
    //}

    //private void OnClientReceive(ReceiveMessage message)
    //{
    //    OnClientReceiveData(message);
    //}

    //private void clientReceive(IAsyncResult ar)
    //{
    //    //获取一个客户端正在接受数据的对象  
    //    Socket workingSocket = ar.AsyncState as Socket;
    //    int byteCount = 0;

    //    try
    //    {
    //        //结束接受数据 完成储存  
    //        byteCount = workingSocket.EndReceive(ar);

    //    }
    //    catch (SocketException ex)
    //    {
    //        //如果接受消息失败  
    //        Debug.LogWarning(ex.ToString());
    //    }
    //    if (byteCount > 0)
    //    {
    //        //转换已经接受到得Byte数据为字符串  
    //        byte[] temp = new byte[byteCount];
    //        Array.Copy(tempClientBuffer, 0, temp, 0, byteCount);
    //        clientBuffer.WriteBuffer(temp);
    //    }
    //    //发送数据  
    //    //clientReceiveCallBack(content);
    //    //接受下一波数据  
    //    clientSocket.BeginReceive(tempClientBuffer, 0, reciveSize, SocketFlags.None,
    //        new System.AsyncCallback(clientReceive), this.clientSocket);
    //}

    //public void ClientSend(string data)
    //{
    //    ClientSend(UTF8Encoding.UTF8.GetBytes(data));
    //}

    //public void ClientSend(byte[] data)
    //{
    //    clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None,
    //     new System.AsyncCallback(ClienSendMsg),
    //     clientSocket);
    //}

    //void ClienSendMsg(System.IAsyncResult ar)
    //{
    //    Socket workingSocket = ar.AsyncState as Socket;
    //    workingSocket.EndSend(ar);
    //}
    //#endregion
}

