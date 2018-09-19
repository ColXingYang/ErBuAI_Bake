using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class CollsionData
{
    public ColliderArea area;
}
public class CollsionController : MonoBehaviour {

    private void Start()
    {
        //ServerDemo.Instance.server.Send();
    }

    [Serializable]
    public class OnCollsionEnterEvent : UnityEvent<int>
    {

    }

    public OnCollsionEnterEvent OnEnter = new OnCollsionEnterEvent();

    public void OnCollsionEnter(int area)
    {
        Debug.Log("碰到了" + (ColliderArea)area);

        //CollsionData temp = new CollsionData();
        //temp.area = (ColliderArea)area;

        //string sendMessage = JsonUtility.ToJson(temp);

        //byte[] bytes = Encoding.UTF8.GetBytes(sendMessage);
        //byte[] sendType = BitConverter.GetBytes((int)DataSendType.COLLIDER);
        //byte[] length = BitConverter.GetBytes(bytes.Length + 4 + 4);
        //List<byte> allMessage = new List<byte>();
        //allMessage.AddRange(sendType);
        //allMessage.AddRange(length);
        //allMessage.AddRange(bytes);
        //byte[] allMessageBytes = allMessage.ToArray();

        OnEnter.Invoke(area);
    }
}
