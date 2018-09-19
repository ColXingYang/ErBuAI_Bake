using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[Serializable]
public class JXBAngleLimit
{
    public float Max;
    public float Min;
}

public class JXBView : MonoBehaviour {

    //[Tooltip("机械臂的所有关节，从下到上的顺序")]
    //所有关节
    //public List<Transform> joints;
    //public CaptureShotView CaptureShotView;
    //JXBMove moveData;

    //public List<JXBAngleLimit> jxbAngleLimits = new List<JXBAngleLimit>();

    //private List<Vector3> initValue = new List<Vector3>();

    private void Start()
    {
        //ServerDemo.Instance.OnRecevieJXBData += OnRecevieJXBData;
        //Init();
    }

    // Update is called once per frame
    void Update()
    {
        //if (isCapture)
        //{
        //    isCapture = false;
        //    CaptureGood();
        //    CaptureShotView.CaptureScreenShot();
        //}
    }

    //bool isCapture = false;
    //void OnRecevieJXBData(JXBMove data)
    //{
    //    isCapture = true;
    //    moveData = data;
    //}

    // Use this for initialization
    void Init () {
        //initValue.Clear();
        //for (int i = 0; i < joints.Count; i++)
        //{
        //    initValue.Add( joints[i].localEulerAngles);
        //}
    }
    //Sequence seq;
    //public void CaptureGood()
    //{
    //    if (joints.Count != 5)
    //        return;
    //    seq = DOTween.Sequence();
    //    seq.Append
    //    (
    //        joints[0].DOLocalRotate(initValue[0] + new Vector3(0, moveData.datas[0].angle, 0), Mathf.Abs(moveData.datas[0].angle) / moveData.datas[0].speed)
    //    );
    //    seq.Append
    //    (
    //        joints[1].DOLocalRotate(initValue[1] + new Vector3(0, 0, moveData.datas[1].angle), Mathf.Abs(moveData.datas[1].angle) / moveData.datas[1].speed)
    //    );
    //    seq.Append
    //    (
    //        joints[2].DOLocalRotate(initValue[2] + new Vector3(0, 0, moveData.datas[2].angle), Mathf.Abs(moveData.datas[2].angle) / moveData.datas[2].speed)
    //     );
    //    seq.Append
    //    (
    //        joints[3].DOLocalRotate(initValue[3] + new Vector3(moveData.datas[3].angle, 0, 0), Mathf.Abs(moveData.datas[3].angle) / moveData.datas[3].speed)
    //     );
    //    seq.Append
    //    (
    //        joints[4].DOLocalRotate(initValue[4] + new Vector3(0, 0, moveData.datas[4].angle), Mathf.Abs(moveData.datas[4].angle) / moveData.datas[4].speed)
    //    );
    //    seq.Play();
    //}

    //public void StopCapture(int collsionArea)
    //{
    //    ColliderArea area = (ColliderArea)collsionArea;
    //    switch (area)
    //    {
    //        case ColliderArea.GOOD_FRONT:

    //        case ColliderArea.GOOD_BACK:
   
    //        case ColliderArea.GOOD_LEFT:
     
    //        case ColliderArea.GOOD_RIGHT:

    //        case ColliderArea.GOOD_UP:
           
    //        case ColliderArea.GOODLEG_LEFTFRONT:
              
    //        case ColliderArea.GOODLEG_LEFTBACK:
             
    //        case ColliderArea.GOODLEG_RIGHTFRONT:
             
    //        case ColliderArea.GOODLEG_RIGHTBACK:
              
    //        case ColliderArea.DESK:
    //            seq.Kill();
    //            break;
    //    }
    //}

    //public void PlayCaptureAnimator(int collsionArea)
    //{
    //    ColliderArea area = (ColliderArea)collsionArea;
    //    switch (area)
    //    {
    //        case ColliderArea.GOOD_FRONT:
    //            Debug.Log("抓取成功");
    //            break;
    //    }
    //}
	
}
