using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoodView : MonoBehaviour
{
    //readonly float maxLeft = 1f;
    //readonly float maxRight = -1f;

    Transform self;

    public Good good;
    private float speed;

    //public UploadGoodInfoView uploadGoodInfoView;
   


    private void Start()
    {
        self = transform;
        good = new Good();
        speed = 0.5f;

        ServerDemo.Instance.OnRecevieGoodPos += SetPos;
    }

    public void Move(float distance)
    {
        Vector3 pos = self.transform.position;
        pos += new Vector3(distance, 0, 0);

        //if (pos.x > maxLeft)
        //{
        //    pos = new Vector3(maxLeft, pos.y, pos.z);
        //}
        //if (pos.x < maxRight)
        //{
        //    pos = new Vector3(maxRight, pos.y, pos.z);
        //}

        self.transform.SetPositionAndRotation(pos, self.rotation);

        good.pos = pos.x;
    }

    private void Update()
    {
        if (isSetPos)
        {
            ChangePos(moveDistance);

            isSetPos = false;
        }
    }

    public void ChangePos(Pos2 posData)
    {
        Vector3 pos = self.transform.position;
        pos = new Vector3(posData.x, pos.y, posData.y);

        //TODO需要限制不掉出桌面的条件
        //if (pos.x > maxLeft)
        //{
        //    pos = new Vector3(maxLeft, pos.y, pos.z);
        //}
        //if (pos.x < maxRight)
        //{
        //    pos = new Vector3(maxRight, pos.y, pos.z);
        //}
        if (SetupController.Instance.sceneObject.goodLegs.Count > 0)
        {
            SetupController.Instance.sceneObject.goodLegs[0].parent.SetParent(self.transform);
        }
        
        self.transform.SetPositionAndRotation(pos, self.rotation);

        if (SetupController.Instance.sceneObject.goodLegs.Count > 0)
        {
            SetupController.Instance.sceneObject.goodLegs[0].parent.SetParent(null);
        }

        good.pos = pos.x;
    }

    public void ChangePos(float value)
    {
        Vector3 pos = self.transform.position;
        pos = new Vector3(value, pos.y, pos.z);

        //if (pos.x > maxLeft)
        //{
        //    pos = new Vector3(maxLeft, pos.y, pos.z);
        //}
        //if (pos.x < maxRight)
        //{
        //    pos = new Vector3(maxRight, pos.y, pos.z);
        //}

        self.transform.SetPositionAndRotation(pos, self.rotation);

        good.pos = pos.x;
    }

    

    bool isSetPos = false;
    float moveDistance = 0f;
    public void SetPos(float value)
    {
        isSetPos = true;
        moveDistance = value;
    }


    float mouseX;
    private void OnMouseDown()
    {
        mouseX = Input.mousePosition.x;
    }

    float distance;
    private void OnMouseDrag()
    {
        distance = Input.mousePosition.x - mouseX;
        mouseX = Input.mousePosition.x;

        Move(-distance * Time.deltaTime * speed);
    }

    private void OnMouseUp()
    {
        //uploadGoodInfoView.gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = false;
    }

    public void UpdateGoodPos()
    {
        byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(good));

        byte[] sendType = BitConverter.GetBytes((int)DataSendType.GOODPOS);
        byte[] length = BitConverter.GetBytes(bytes.Length + 4 + 4);
        List<byte> allMessage = new List<byte>();
        allMessage.AddRange(sendType);
        allMessage.AddRange(length);
        allMessage.AddRange(bytes);
        byte[] allMessageBytes = allMessage.ToArray();

        ServerDemo.Instance.server.Send(allMessageBytes);


    }

    //private void OnDestroy()
    //{
       
    //}
}
