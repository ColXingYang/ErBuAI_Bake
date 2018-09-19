using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System.Text;

[Serializable]
public class SceneObject
{
    public Transform desk;
    public List<Transform> changeDeskLegs;
    public List<Transform> deskMoveUpLegs;
    public List<Transform> goodLegs;
    public Transform good;

    public Transform jxb;

    public List<Transform> deskLegs;

    public List<Transform> jxbJoints;

    public RectTransform rectTransform;

    public InputField deskPosX;
    public InputField deskPosZ;
    public InputField deskL;
    public InputField deskW;
    public InputField deskH;
    public InputField jxbPosX;
    public InputField jxbPosZ;


    public Transform tpoint1;
    public Transform tpoint2;
    public Transform tpoint3;
    public Transform tpoint4;
    public Transform hpoint1;
    public Transform hpoint2;
    public Transform hpoint3;
    public Transform hpoint4;
    public Transform box;
    public Transform zj;
    
}

public class SetupController : MonoSingleton<SetupController> {

    public Slider goodPosSlider;
    public GoodView goodView;
    public CaptureShotView captureShotView;

    public SceneObject sceneObject;
    public int packetNum = 0;

    


    RtsCamera rtsCamera;
    private void Start()
    {
        rtsCamera = Camera.main.GetComponent<RtsCamera>();
        goodPosSlider.onValueChanged.AddListener(ChangeGoodPos);
        ServerDemo.Instance.OnRecevieInitData += OnReceiveInitData;
        ServerDemo.Instance.OnRecevieStepData += OnReceiveStepData;
        ServerDemo.Instance.OnRecevieResetCommand += OnReset;


        if (sceneObject.deskPosX != null)
        {
            sceneObject.deskPosX.text = sceneObject.desk.position.x.ToString("0.00");
        }
        if (sceneObject.deskPosZ != null)
        {
            sceneObject.deskPosZ.text = sceneObject.desk.position.z.ToString("0.00");
        }
        if (sceneObject.deskW != null)
        {
            sceneObject.deskW.text = SceneSizeOptions.Desk.width.ToString("0.00");
        }
        if (sceneObject.deskH != null)
        {
            sceneObject.deskH.text = SceneSizeOptions.Desk.heigth.ToString("0.00");
        }
        if (sceneObject.deskL != null)
        {
            sceneObject.deskL.text = SceneSizeOptions.Desk.length.ToString("0.00");
        }
        if (sceneObject.jxbPosX != null)
        {
            sceneObject.jxbPosX.text = sceneObject.jxb.position.x.ToString("0.00");
        }
        if (sceneObject.jxbPosZ != null)
        {
            sceneObject.jxbPosZ.text = sceneObject.jxb.position.z.ToString("0.00");
        }


        screenShot = new Texture2D((int)sceneObject.rectTransform.rect.width, (int)sceneObject.rectTransform.rect.height, TextureFormat.RGB24, false);
    }

    

    public void SetUpGoodPos()
    {
        HideCameraSetup();
        goodPosSlider.gameObject.SetActive(true);       
        Camera.main.GetComponent<RtsCamera>().Rotation = -359;
    }

    public void OnReceiveInitData(ReceiveInitData initData)
    {
        isReceiveInitData = true;
        this.initData = initData;
       
    }

    public void OnReceiveStepData(JXBWData data)
    {
        isReceiveStepData = true;
        jxbWData = data;
    }

    bool isReceiveInitData = false;
    ReceiveInitData initData = null;

    bool isReceiveStepData = false;
    JXBWData jxbWData = null;

    bool isReceiveResetCommand = false;
    private void Update()
    {
        if (isReceiveInitData && initData != null)
        {
            //Debug.Log("isReceiveInitData!");
            isReceiveInitData = false;
            SetGoodSize(initData.goodInfo.goodSize);
            ChangeGoodPos(initData.goodInfo.goodPos);
            SetJXBAngle(initData.jxbAngleInfo);
            StepOnCompelte();
            

            //Debug.Log("init data222" + initData);
        }

        if (isReceiveStepData && jxbWData != null)
        {
            //Debug.Log("isReceiveStepData!");
            isReceiveStepData = false;
            Step();
            //SendResutInfo();
            
        }


        if (isReceiveResetCommand && initData != null)
        {
            //Debug.Log("isReceiveResetCommand!");
            isReceiveResetCommand = false;
            SetGoodSize(initData.goodInfo.goodSize);
            ChangeGoodPos(initData.goodInfo.goodPos);
            SetJXBAngle(initData.jxbAngleInfo);
            StepOnCompelte();
           
        }
    }

    #region 步进

    ResultData resultData = new ResultData();
    Vector3 joints01Offset = Vector3.zero;
    Vector3 joints02Offset = Vector3.zero;
    Vector3 joints03Offset = Vector3.zero;
    Vector3 joints04Offset = Vector3.zero;
    Vector3 joints05Offset = Vector3.zero;
    Tweener joints01MoveTweener;
    Tweener joints02MoveTweener;
    Tweener joints03MoveTweener;
    Tweener joints04MoveTweener;
    Tweener joints05MoveTweener;

    float stepTime = 1.0f;
    public void Step()
    {
        //initData = new ReceiveInitData();
        //resultData.colliderArea = ColliderArea.NONE;
        sceneObject.box.position = new Vector3(sceneObject.box.position.x,0,UnityEngine.Random.Range(-0.4f,0.4f));
        sceneObject.zj.position = new Vector3(sceneObject.box.position.x,sceneObject.box.position.y,sceneObject.box.position.z);
        stepTime = initData.simulateInfo.k * initData.simulateInfo.deltaTime;
        joints01Offset.y = jxbWData.joint1_w * stepTime;
        joints02Offset.z = jxbWData.joint2_w * stepTime;
        joints03Offset.z = jxbWData.joint3_w * stepTime;
        joints04Offset.x = jxbWData.joint4_w * stepTime;
        joints05Offset.z = jxbWData.joint5_w * stepTime;

        joints01MoveTweener = sceneObject.jxbJoints[0].DOLocalRotate(sceneObject.jxbJoints[0].localEulerAngles + joints01Offset, stepTime);

        joints02MoveTweener = sceneObject.jxbJoints[1].DOLocalRotate(sceneObject.jxbJoints[1].localEulerAngles + joints02Offset, stepTime);

        joints03MoveTweener = sceneObject.jxbJoints[2].DOLocalRotate(sceneObject.jxbJoints[2].localEulerAngles + joints03Offset, stepTime);

        joints04MoveTweener = sceneObject.jxbJoints[3].DOLocalRotate(sceneObject.jxbJoints[3].localEulerAngles + joints04Offset, stepTime);

        joints05MoveTweener = sceneObject.jxbJoints[4].DOLocalRotate(sceneObject.jxbJoints[4].localEulerAngles + joints05Offset, stepTime).OnComplete(StepOnCompelte);

    }

    private void StepOnCompelte()
    {
        GetPos();
        //resultData.colliderArea = ColliderArea.NONE;
        resultData.image = new byte[0];
        SendResutInfo();
        if (resultData.colliderArea != ColliderArea.NONE)
        {
            resultData.colliderArea = ColliderArea.NONE;
        }
        //StartCoroutine(CaptureShot());
        joints01MoveTweener.Kill();
        joints02MoveTweener.Kill();
        joints03MoveTweener.Kill();
        joints04MoveTweener.Kill();
        joints05MoveTweener.Kill();
    }

    public void OnCollider(int area)
    {
        //DOTween.KillAll();
        //GetPos();
        resultData.colliderArea = (ColliderArea)area;
        Debug.Log("OnCollider：" + resultData.colliderArea);
        //StartCoroutine(CaptureShot());     
    }

    List<byte> allResutInfoMessage = new List<byte>();
    public void SendResutInfo()
    {
        
        //Debug.Log("SendResutInfo：" + resultData.colliderArea);

        string sendMessage = JsonUtility.ToJson(resultData);
        byte[] bytes = Encoding.UTF8.GetBytes(sendMessage);
        byte[] sendType = BitConverter.GetBytes((int)DataSendType.Result);
        byte[] length = BitConverter.GetBytes(bytes.Length + 4 + 4);
        allResutInfoMessage.Clear();
        allResutInfoMessage.AddRange(sendType);
        allResutInfoMessage.AddRange(length);
        //ServerDemo.Instance.server.Send(allMessageBytes);
        //allMessage.AddRange(bytes);
        byte[] allMessageBytes = allResutInfoMessage.ToArray();
        Array.Resize(ref allMessageBytes, allMessageBytes.Length + bytes.Length);
        //Debug.Log("allMessageBytes.Length :" + (allMessageBytes.Length));
        //Debug.Log("bytes.Length :" + (bytes.Length));
        bytes.CopyTo(allMessageBytes, allMessageBytes.Length - bytes.Length);
        ServerDemo.Instance.server.USend(allMessageBytes);
        //ServerDemo.Instance.server.Send(bytes);

        if (allMessageBytes.Length != 0)
        {
            packetNum += 1;
            //Debug.Log("send packrtNum is ：" + packetNum);
            //Debug.Log("Length :" + (allMessageBytes.Length));
        }
    }

    /// <summary>
    /// 获取机械臂和目标的位置信息
    /// </summary>
    private void GetPos()
    {
        
        //if (sceneObject.tpoint1 != null)
        //{
        //    Debug.Log("tpoint1：" + sceneObject.tpoint1.position.ToString("F7"));
        //}
        //if (sceneObject.tpoint2 != null)
        //{
        //    Debug.Log("tpoint2：" + sceneObject.tpoint2.position.ToString("F7"));
        //}
        //if (sceneObject.hpoint1 != null)
        //{
        //    Debug.Log("hpoint1：" + sceneObject.hpoint1.position.ToString("F7"));
        //}
        //if (sceneObject.hpoint2 != null)
        //{
        //    Debug.Log("hpoint2：" + sceneObject.hpoint2.position.ToString("F7"));
        //}
        //if (sceneObject.hpoint3 != null)
        //{
        //    Debug.Log("hpoint3：" + sceneObject.hpoint3.position.ToString("F7"));
        //}
        //if (sceneObject.hpoint4 != null)
        //{
        //    Debug.Log("hpoint4：" + sceneObject.hpoint4.position.ToString("F7"));
        //}

        if (sceneObject.jxbJoints.Count != 5) return;
        if (resultData == null)
        {
            resultData = new ResultData();
        }
        if (sceneObject.good == null) return;

        //resultData.joint1_pos
        resultData.joint1_pos.x = sceneObject.jxbJoints[0].position.x;
        resultData.joint1_pos.y = sceneObject.jxbJoints[0].position.y;
        resultData.joint1_pos.z = sceneObject.jxbJoints[0].position.z;

        resultData.joint2_pos.x = sceneObject.jxbJoints[1].position.x;
        resultData.joint2_pos.y = sceneObject.jxbJoints[1].position.y;
        resultData.joint2_pos.z = sceneObject.jxbJoints[1].position.z;

        resultData.joint3_pos.x = sceneObject.jxbJoints[2].position.x;
        resultData.joint3_pos.y = sceneObject.jxbJoints[2].position.y;
        resultData.joint3_pos.z = sceneObject.jxbJoints[2].position.z;

        resultData.joint4_pos.x = sceneObject.jxbJoints[3].position.x;
        resultData.joint4_pos.y = sceneObject.jxbJoints[3].position.y;
        resultData.joint4_pos.z = sceneObject.jxbJoints[3].position.z;

        resultData.joint5_pos.x = sceneObject.jxbJoints[4].position.x;
        resultData.joint5_pos.y = sceneObject.jxbJoints[4].position.y;
        resultData.joint5_pos.z = sceneObject.jxbJoints[4].position.z;

        resultData.target_pos.x = sceneObject.good.position.x;
        resultData.target_pos.y = sceneObject.good.position.y;
        resultData.target_pos.z = sceneObject.good.position.z;

        //resultData.tpoint1_pos
        resultData.tpoint1_pos.x = sceneObject.tpoint1.position.x;
        resultData.tpoint1_pos.y = sceneObject.tpoint1.position.y;
        resultData.tpoint1_pos.z = sceneObject.tpoint1.position.z;

        resultData.tpoint2_pos.x = sceneObject.tpoint2.position.x;
        resultData.tpoint2_pos.y = sceneObject.tpoint2.position.y;
        resultData.tpoint2_pos.z = sceneObject.tpoint2.position.z;

        resultData.tpoint3_pos.x = sceneObject.tpoint3.position.x;
        resultData.tpoint3_pos.y = sceneObject.tpoint3.position.y;
        resultData.tpoint3_pos.z = sceneObject.tpoint3.position.z;

        resultData.tpoint4_pos.x = sceneObject.tpoint4.position.x;
        resultData.tpoint4_pos.y = sceneObject.tpoint4.position.y;
        resultData.tpoint4_pos.z = sceneObject.tpoint4.position.z;

        //resultData.hpoint1_pos
        resultData.hpoint1_pos.x = sceneObject.hpoint1.position.x;
        resultData.hpoint1_pos.y = sceneObject.hpoint1.position.y;
        resultData.hpoint1_pos.z = sceneObject.hpoint1.position.z;

        resultData.hpoint2_pos.x = sceneObject.hpoint2.position.x;
        resultData.hpoint2_pos.y = sceneObject.hpoint2.position.y;
        resultData.hpoint2_pos.z = sceneObject.hpoint2.position.z;

        resultData.hpoint3_pos.x = sceneObject.hpoint3.position.x;
        resultData.hpoint3_pos.y = sceneObject.hpoint3.position.y;
        resultData.hpoint3_pos.z = sceneObject.hpoint3.position.z;

        resultData.hpoint4_pos.x = sceneObject.hpoint4.position.x;
        resultData.hpoint4_pos.y = sceneObject.hpoint4.position.y;
        resultData.hpoint4_pos.z = sceneObject.hpoint4.position.z;


        //jpoint1_ang
        resultData.jpoint1_ang.x = sceneObject.jxbJoints[0].rotation.eulerAngles.x;
        resultData.jpoint1_ang.y = sceneObject.jxbJoints[0].rotation.eulerAngles.y;
        resultData.jpoint1_ang.z = sceneObject.jxbJoints[0].rotation.eulerAngles.z;

        resultData.jpoint2_ang.x = sceneObject.jxbJoints[1].rotation.eulerAngles.x;
        resultData.jpoint2_ang.y = sceneObject.jxbJoints[1].rotation.eulerAngles.y;
        resultData.jpoint2_ang.z = sceneObject.jxbJoints[1].rotation.eulerAngles.z;

        resultData.jpoint3_ang.x = sceneObject.jxbJoints[2].rotation.eulerAngles.x;
        resultData.jpoint3_ang.y = sceneObject.jxbJoints[2].rotation.eulerAngles.y;
        resultData.jpoint3_ang.z = sceneObject.jxbJoints[2].rotation.eulerAngles.z;

        resultData.jpoint4_ang.x = sceneObject.jxbJoints[3].rotation.eulerAngles.x;
        resultData.jpoint4_ang.y = sceneObject.jxbJoints[3].rotation.eulerAngles.y;
        resultData.jpoint4_ang.z = sceneObject.jxbJoints[3].rotation.eulerAngles.z;

        resultData.jpoint5_ang.x = sceneObject.jxbJoints[4].rotation.eulerAngles.x;
        resultData.jpoint5_ang.y = sceneObject.jxbJoints[4].rotation.eulerAngles.y;
        resultData.jpoint5_ang.z = sceneObject.jxbJoints[4].rotation.eulerAngles.z;

        //Debug.Log("end pos：" + ((sceneObject.hpoint3.position + sceneObject.hpoint4.position)/2).ToString("F7"));
        //Debug.Log("jpoint1_ang：" + sceneObject.jxbJoints[0].rotation.eulerAngles.ToString("F7"));
        //Debug.Log("jpoint2_ang：" + sceneObject.jxbJoints[1].rotation.eulerAngles.ToString("F7"));
        //Debug.Log("jpoint3_ang：" + sceneObject.jxbJoints[2].rotation.eulerAngles.ToString("F7"));
        //Debug.Log("jpoint4_ang：" + sceneObject.jxbJoints[3].rotation.eulerAngles.ToString("F7"));
        //Debug.Log("jpoint5_ang：" + sceneObject.jxbJoints[4].rotation.eulerAngles.ToString("F7"));
    }
    Vector3[] corners = new Vector3[4];
    Texture2D screenShot;
    private IEnumerator CaptureShot()
    {
        yield return new WaitForEndOfFrame();
        resultData.image = new byte[0];
        //screenShot.Resize((int)sceneObject.rectTransform.rect.width, (int)sceneObject.rectTransform.rect.height) /*= new Texture2D((int)sceneObject.rectTransform.rect.width, (int)sceneObject.rectTransform.rect.height, TextureFormat.RGB24, false)*/;

        //sceneObject.rectTransform.GetWorldCorners(corners);

        //screenShot.ReadPixels(new Rect(corners[0].x, corners[0].y, sceneObject.rectTransform.rect.width, sceneObject.rectTransform.rect.height), 0, 0);

        //screenShot.Apply();

        //resultData.image = screenShot.EncodeToJPG();
        //resultData.image = new byte[0];
        //screenShot = null;
        SendResutInfo();
    }
    #endregion

    #region 改变尺寸

    public void SetDeskSize(Size size)
    {
        if (sceneObject.desk != null)
        {
            float halfDeltaLength = (size.length - SceneSizeOptions.Desk.length) / 2;
            SetLength(sceneObject.desk, size.length / SceneSizeOptions.Desk.length);
            if (sceneObject.deskLegs.Count > 0)
            {
                for (int i = 0; i < sceneObject.deskLegs.Count; i++)
                {
                    //deltaLength *= (i % 2 == 0 ? -1 : 1);
                    MoveRight(sceneObject.deskLegs[i], halfDeltaLength * (i % 2 == 0 ? 1 : -1));
                }
            }


            float halfDeltaWidth = (size.width - SceneSizeOptions.Desk.width) / 2;
            SetWidth(sceneObject.desk, size.width / SceneSizeOptions.Desk.width);
            if (sceneObject.deskLegs.Count > 0)
            {
                for (int i = 0; i < sceneObject.deskLegs.Count; i++)
                {
                    MoveForward(sceneObject.deskLegs[i], halfDeltaWidth * (i <2 ? 1 : -1));
                }
            }
            //设置高度
            //1.改变桌子支撑点的缩放值

            float deltaHeight = 0;
            if (sceneObject.changeDeskLegs.Count > 0)
            {
                deltaHeight = size.heigth - SceneSizeOptions.Desk.heigth;
                for (int i = 0; i < sceneObject.changeDeskLegs.Count; i++)
                {
                    SetHeigth(sceneObject.changeDeskLegs[i], (deltaHeight + SceneSizeOptions.ChangeDeskLegHeigth) / SceneSizeOptions.ChangeDeskLegHeigth);
                }
                
            }

            //2.调整桌子支撑点以上物体的位置
            if (sceneObject.deskMoveUpLegs.Count > 0)
            {
                for (int i = 0; i < sceneObject.deskMoveUpLegs.Count; i++)
                {
                    MoveUp(sceneObject.deskMoveUpLegs[i], deltaHeight);
                }
            }

            MoveUp(sceneObject.desk, deltaHeight);

            if (sceneObject.goodLegs.Count > 0)
            {
                for (int i = 0; i < sceneObject.goodLegs.Count; i++)
                {
                    MoveUp(sceneObject.goodLegs[i], deltaHeight);
                }
            }

            if (sceneObject.good != null)
            {
                MoveUp(sceneObject.good, deltaHeight);
            }
            
        }     
    }

    public void SetGoodSize(Size size)
    {
        if (sceneObject.good != null)
        {
            SetLength(sceneObject.good, size.length / SceneSizeOptions.Good.length);


            SetWidth(sceneObject.good, size.width / SceneSizeOptions.Good.width);


            SetHeigth(sceneObject.good, size.heigth / SceneSizeOptions.Good.heigth);
        }
    }

    public void SetGoodLegHeigth(float legHeigth)
    {
        if (sceneObject.goodLegs.Count > 0)
        {
            float deltaHeigth = legHeigth - SceneSizeOptions.GoodLegHeigth;
            for (int i = 0; i < sceneObject.goodLegs.Count; i++)
            {
                SetHeigth(sceneObject.goodLegs[i], legHeigth * 0.1F / SceneSizeOptions.GoodLegHeigth);
            }
            MoveUp(sceneObject.good, deltaHeigth);
        }
    }

    public void SetJXBAngle(JXBAngleData data)
    {
        if (sceneObject.jxbJoints.Count ==5)
        {
            sceneObject.jxbJoints[0].localEulerAngles = new Vector3(0, data.joint1_YAxis_Angle, 0);
            sceneObject.jxbJoints[1].localEulerAngles = new Vector3(0, 0, data.joint2_ZAxis_Angle);
            sceneObject.jxbJoints[2].localEulerAngles = new Vector3(0, 0, data.joint3_ZAxis_Angle);
            sceneObject.jxbJoints[3].localEulerAngles = new Vector3(data.joint4_XAxis_Angle, 0, 0);
            sceneObject.jxbJoints[4].localEulerAngles = new Vector3(0, 0, data.joint5_ZAxis_Angle);

        }
    }
    #endregion

    private void MoveUp(Transform target,float deltaHeigth)
    {
        Vector3 pos = target.position;
        pos += Vector3.up * deltaHeigth;
        target.SetPositionAndRotation(pos,Quaternion.identity);
    }

    private void MoveRight(Transform target, float deltaLeft)
    {
        Vector3 pos = target.position;
        pos += Vector3.right * deltaLeft;
        target.SetPositionAndRotation(pos, Quaternion.identity);
    }

    private void MoveForward(Transform target, float deltaForward)
    {
        Vector3 pos = target.position;
        pos += Vector3.forward * deltaForward;
        target.SetPositionAndRotation(pos, Quaternion.identity);
    }

    private void SetLength(Transform target,float length)
    {
        if (length <= 0) return;

        target.localScale = new Vector3(length, target.lossyScale.y, target.lossyScale.z);
    }

    private void SetWidth(Transform target, float width)
    {
        if (width <= 0) return;

        target.localScale = new Vector3(target.lossyScale.x, target.lossyScale.y, width);
    }

    private void SetHeigth(Transform target, float heigth)
    {
        if (heigth <= 0) return;

        target.localScale = new Vector3(target.lossyScale.x, heigth, target.lossyScale.z);
    }

    #region 改变机械臂位置

    public void ChangeJXBPosX(string value)
    {
        if(sceneObject.jxb != null)
        {
            float posx = sceneObject.jxb.position.x;

            if (float.TryParse(value, out posx))
            {
                sceneObject.jxb.position = new Vector3(posx, sceneObject.jxb.position.y, sceneObject.jxb.position.z);
            }

            if (sceneObject.jxbPosX != null)
            {
                sceneObject.jxbPosX.text = sceneObject.jxb.position.x.ToString("0.00");
            }
        }
    }

    public void AddJXBPosX(string delta)
    {
        if (sceneObject.jxb != null)
        {
            float posx = sceneObject.jxb.position.x;
            float value = 0;
            if (float.TryParse(delta, out value))
            {
                ChangeJXBPosX((posx + value).ToString());
            }
        }
    }

    public void ChangeJXBPosZ(string value)
    {
        if (sceneObject.jxb != null)
        {
            float posz = sceneObject.jxb.position.z;

            if (float.TryParse(value, out posz))
            {
                sceneObject.jxb.position = new Vector3(sceneObject.jxb.position.x, sceneObject.jxb.position.y, posz);
            }

            if (sceneObject.jxbPosZ != null)
            {
                sceneObject.jxbPosZ.text = sceneObject.jxb.position.z.ToString("0.00");
            }
        }
    }

    public void AddJXBPosZ(string delta)
    {
        if (sceneObject.jxb != null)
        {
            float posz = sceneObject.jxb.position.z;
            float value = 0;
            if (float.TryParse(delta, out value))
            {
                ChangeJXBPosZ((posz + value).ToString());
            }
        }
    }
    #endregion

    #region 改变桌子的位置和尺寸

    public void ChangeDeskPosX(string value)
    {
        if (sceneObject.desk != null)
        {
            float posx = sceneObject.desk.position.x;
            float deltaPos = posx;
            if (float.TryParse(value, out posx))
            {
                deltaPos = posx - deltaPos;
                sceneObject.desk.position = new Vector3(posx, sceneObject.desk.position.y, sceneObject.desk.position.z);


                if (sceneObject.deskPosX != null)
                {
                    sceneObject.deskPosX.text = sceneObject.desk.position.x.ToString("0.00");
                } 

                if (sceneObject.deskLegs.Count > 0)
                {
                    for (int i = 0; i < sceneObject.deskLegs.Count; i++)
                    {
                        sceneObject.deskLegs[i].position = new Vector3(sceneObject.deskLegs[i].position.x+ deltaPos, sceneObject.deskLegs[i].position.y, sceneObject.deskLegs[i].position.z);
                    }
                }

                if (sceneObject.good != null)
                {
                    sceneObject.good.position = new Vector3(sceneObject.good.position.x + deltaPos, sceneObject.good.position.y, sceneObject.good.position.z);
                }

                if (sceneObject.goodLegs.Count > 0)
                {
                    for (int i = 0; i < sceneObject.goodLegs.Count; i++)
                    {
                        sceneObject.goodLegs[i].position = new Vector3(sceneObject.goodLegs[i].position.x + deltaPos, sceneObject.goodLegs[i].position.y, sceneObject.goodLegs[i].position.z);
                    }
                }
            }
        }
    }

    public void AddDeskPosX(string delta)
    {
        if (sceneObject.desk != null)
        {
            float posx = sceneObject.desk.position.x;
            float value = 0;
            if (float.TryParse(delta, out value))
            {
                ChangeDeskPosX((posx + value).ToString());
            }
        }
    }

    public void ChangeDeskPosZ(string value)
    {
        if (sceneObject.desk != null)
        {
            float posz = sceneObject.desk.position.z;
            float deltaPos = posz;
            if (float.TryParse(value, out posz))
            {
                deltaPos = posz - deltaPos;
                sceneObject.desk.position = new Vector3(sceneObject.desk.position.x, sceneObject.desk.position.y, posz);


                if (sceneObject.deskPosZ != null)
                {
                    sceneObject.deskPosZ.text = sceneObject.desk.position.z.ToString("0.00");
                }

                if (sceneObject.deskLegs.Count > 0)
                {
                    for (int i = 0; i < sceneObject.deskLegs.Count; i++)
                    {
                        sceneObject.deskLegs[i].position = new Vector3(sceneObject.deskLegs[i].position.x, sceneObject.deskLegs[i].position.y, sceneObject.deskLegs[i].position.z+ deltaPos);
                    }
                }

                if (sceneObject.good != null)
                {
                    sceneObject.good.position = new Vector3(sceneObject.good.position.x, sceneObject.good.position.y, posz);
                }

                if (sceneObject.goodLegs.Count > 0)
                {
                    for (int i = 0; i < sceneObject.goodLegs.Count; i++)
                    {
                        sceneObject.goodLegs[i].position = new Vector3(sceneObject.goodLegs[i].position.x , sceneObject.goodLegs[i].position.y, sceneObject.goodLegs[i].position.z+ deltaPos);
                    }
                }
            }
        }
    }

    public void AddDeskPosZ(string delta)
    {
        if (sceneObject.desk != null)
        {
            float posz = sceneObject.desk.position.z;
            float value = 0;
            if (float.TryParse(delta, out value))
            {
                ChangeDeskPosZ((posz + value).ToString());
            }
        }
    }

    public void ChangeDeskLength(string value)
    {
        if (sceneObject.desk != null)
        {
            float length = 0;
            if (float.TryParse(value, out length))
            {
                float halfDeltaLength = (length - sceneObject.desk.lossyScale.x* SceneSizeOptions.Desk.length) / 2;
                SetLength(sceneObject.desk, length / SceneSizeOptions.Desk.length);

                if (sceneObject.deskL != null)
                {
                    sceneObject.deskL.text = length.ToString("0.00");
                }

                if (sceneObject.deskLegs.Count > 0)
                {
                    for (int i = 0; i < sceneObject.deskLegs.Count; i++)
                    {
                        MoveRight(sceneObject.deskLegs[i], halfDeltaLength * (i % 2 == 0 ? 1 : -1));
                    }
                }
            }

           
        }
    }

    public void ChangeDeskWidth(string value)
    {
        if (sceneObject.desk != null)
        {
            float width = 0;
            if (float.TryParse(value, out width))
            {
                float halfDeltaWidth = (width - sceneObject.desk.lossyScale.z * SceneSizeOptions.Desk.width) / 2;
                SetWidth(sceneObject.desk, width / SceneSizeOptions.Desk.width);

                if (sceneObject.deskW != null)
                {
                    sceneObject.deskW.text = width.ToString("0.00");
                }
                if (sceneObject.deskLegs.Count > 0)
                {
                    for (int i = 0; i < sceneObject.deskLegs.Count; i++)
                    {
                        MoveForward(sceneObject.deskLegs[i], halfDeltaWidth * (i < 2 ? 1 : -1));
                    }
                }
            }
        }
    }

    public void ChangeDeskHeigth(string value)
    {
        if (sceneObject.desk != null)
        {
            float heigth = 0;
            if (float.TryParse(value, out heigth))
            {
                //设置高度
                //1.改变桌子支撑点的缩放值

                float deltaHeight = 0;
                if (sceneObject.changeDeskLegs.Count > 0)
                {
                    deltaHeight = heigth - (sceneObject.changeDeskLegs[0].localScale.y - 1)* SceneSizeOptions.ChangeDeskLegHeigth - SceneSizeOptions.Desk.heigth ;
                    for (int i = 0; i < sceneObject.changeDeskLegs.Count; i++)
                    {
                        SetHeigth(sceneObject.changeDeskLegs[i], (heigth - SceneSizeOptions.Desk.heigth + SceneSizeOptions.ChangeDeskLegHeigth) / SceneSizeOptions.ChangeDeskLegHeigth);
                    }

                }


                if (sceneObject.deskH != null)
                {
                    sceneObject.deskH.text = heigth.ToString("0.00");
                }

                //2.调整桌子支撑点以上物体的位置
                if (sceneObject.deskMoveUpLegs.Count > 0)
                {
                    for (int i = 0; i < sceneObject.deskMoveUpLegs.Count; i++)
                    {
                        MoveUp(sceneObject.deskMoveUpLegs[i], deltaHeight);
                    }
                }

                MoveUp(sceneObject.desk, deltaHeight);

                if (sceneObject.goodLegs.Count > 0)
                {
                    for (int i = 0; i < sceneObject.goodLegs.Count; i++)
                    {
                        MoveUp(sceneObject.goodLegs[i], deltaHeight);
                    }
                }

                if (sceneObject.good != null)
                {
                    MoveUp(sceneObject.good, deltaHeight);
                }
            }
        }
    }

    #endregion

    public void OnReset()
    {
        isReceiveResetCommand = true;
    }

    public void ChangeGoodPos(Pos2 pos)
    {
        goodView.ChangePos(pos);
    }

    public void ChangeGoodPos(float pos)
    {
        goodView.ChangePos(pos);
    }

    public void HideSetGoodPos()
    {
        goodPosSlider.gameObject.SetActive(false);
        rtsCamera.Rotation = captureShotView.cameraData.cameraAngle;
 
    }

    private void OnDestroy()
    {
        goodPosSlider.onValueChanged.RemoveListener(ChangeGoodPos);
        //ServerDemo.Instance.OnRecevieInitData -= OnReceiveInitData;
        //ServerDemo.Instance.OnRecevieStepData -= OnReceiveStepData;
    }

    public void SetUpCameraSetup()
    {
        //HideSetGoodPos();
        captureShotView.gameObject.SetActive(!captureShotView.gameObject.activeSelf);
    }

    public void HideCameraSetup()
    {
        captureShotView.gameObject.SetActive(false);
    }

    public void Quit()
    {
        //Debug.Log("tuichu");
        Application.Quit();
    }
}
