using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnRecevieGoodPos(float pos);
public delegate void OnRecevieJXBData(JXBMove data);
public delegate void OnRecevieCaputerScreenShotFrame(CaptureShotFrameRange data);
public delegate void OnRecevieInitData(ReceiveInitData data);
public delegate void OnRecevieStepData(JXBWData data);
public delegate void OnRecevieResetCommand();

[Serializable]
public class JXBMoveData
{
    public float angle;
    public float speed;
    public JXBMoveData() { }
    public JXBMoveData(float angle, float speed)
    {
        this.angle = angle;
        this.speed = speed;
    }
}

public class JXBMove
{
    [SerializeField]
    public List<JXBMoveData> datas;

    public JXBMove()
    {
        datas = new List<JXBMoveData>(5);
        datas.Add(new JXBMoveData(-15, 15));
        datas.Add(new JXBMoveData(-10, 10));
        datas.Add(new JXBMoveData(-10, 10));
        datas.Add(new JXBMoveData(-10, 10));
        datas.Add(new JXBMoveData(-10, 10));
    }
}


public class ServerDemo : MonoSingleton<ServerDemo>
{
    public SZHSocket server;
    public OnRecevieGoodPos OnRecevieGoodPos;
    public OnRecevieJXBData OnRecevieJXBData;
    public OnRecevieCaputerScreenShotFrame OnRecevieCaputerScreenShotFrame;
    public OnRecevieInitData OnRecevieInitData;
    public OnRecevieStepData OnRecevieStepData;
    public OnRecevieResetCommand OnRecevieResetCommand;

    public override void Init()
    {
        server = new SZHSocket();
        //执行初始化服务器方法，传入委托函数  
        server.InitServer(/*ShowMsg*/);
        server.OnServerReceiveData += OnReceive;

        //ReceiveInitData data = new ReceiveInitData();
        //Debug.Log(JsonUtility.ToJson(data));
        //ReceiveStepData data = new ReceiveStepData();
        //Debug.Log(JsonUtility.ToJson(data));
    }

    StringBuilder data = new StringBuilder();
    void OnReceive(ReceiveMessage message)
    {
        switch (message.type)
        {
            case DataSendType.Init:
                if (OnRecevieInitData != null)
                {
                    data.Length = 0;
                    data.Append(Encoding.UTF8.GetString(message.data));
                    //string data = Encoding.UTF8.GetString(message.data);
                    Debug.Log("init data" + data);
                    OnRecevieInitData(JsonUtility.FromJson<ReceiveInitData>(data.ToString()));
                }
                break;
            case DataSendType.Step:
                if (OnRecevieStepData != null)
                {
                    data.Length = 0;
                    data.Append(Encoding.UTF8.GetString(message.data));
                    //Debug.Log(data);
                    //JXBWData temp = JsonUtility.FromJson<JXBWData>(data);
                    OnRecevieStepData(JsonUtility.FromJson<JXBWData>(data.ToString()));
                }
                break;
            case DataSendType.Reset:
                if (OnRecevieResetCommand != null)
                {
                    OnRecevieResetCommand();
                }
                break;
        }
    }



    private void OnDestroy()
    {
        if (server == null) return;
        server.OnServerReceiveData -= OnReceive;
        //server.DisConnect();
        server.SocketQuit();

    }



}