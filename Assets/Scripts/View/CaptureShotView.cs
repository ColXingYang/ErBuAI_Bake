using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraData
{
    public float captureAreaXPos;
    public float captureAreaYPos;
    public float captureAreaWidth;
    public float captureAreaHeight;
    public float cameraAngle;
}

public class CaptureShotFrameRange
{
    public int start;
    public int end;
}

public class AreaInset
{
    public float left;
    public float right;
    public float up;
    public float down;

    public AreaInset()
    {
        left = 0;
        right = 0;
        up = 0;
        down = 0;
    }
}

public enum DirType
{
    UPLEFT,
    UP,
    UPRIGHT,
    RIGHT,
    DOWNRIGHT,
    DOWN,
    DOWNLEFT,
    LEFT,
    CENTER,
    NONE
}

public class CaptureShotView : MonoBehaviour{

    public RectTransform rectTransform;

    private bool isPointerEnter = false;
    private Vector2 upPointerPos;
    private Vector2 nowPointerPos;
    private DirType dir;
    
    private AreaInset areaInset;
    private CaptureShotFrameRange captureShotFrameRange;
    // Use this for initialization
    void Start () {
        rectTransform = GetComponent<RectTransform>();
        areaInset = new AreaInset();
        //ServerDemo.Instance.OnRecevieCaputerScreenShotFrame += OnRecevieCaputerScreenShotFrame;
        gameObject.SetActive(false);
    }  

    void OnRecevieCaputerScreenShotFrame(CaptureShotFrameRange data)
    {
        captureShotFrameRange = data;
    }

    private void Update()
    {
        if (isPointerEnter)
        {
            if (Input.GetMouseButton(0) && dir != DirType.NONE)
            {
                nowPointerPos = (Vector2)Input.mousePosition;
                Move(nowPointerPos - upPointerPos, dir);
                upPointerPos = nowPointerPos;
            }
            if (Input.GetMouseButtonUp(0))
            {
                dir = DirType.NONE;
                isPointerEnter = false;
                CursorView.Instance.Set(DirType.NONE);
            }         
        }    
    }

    private void Move(Vector2 moveDir, DirType dir)
    {
        switch (dir)
        {
            case DirType.UPLEFT:
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, areaInset.down, rectTransform.rect.height + moveDir.y);
                areaInset.up -= moveDir.y;
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, areaInset.right, rectTransform.rect.width - moveDir.x);
                areaInset.left += moveDir.x;
                break;
            case DirType.UPRIGHT:
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, areaInset.down, rectTransform.rect.height + moveDir.y);
                areaInset.up -= moveDir.y;
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, areaInset.left, rectTransform.rect.width + moveDir.x);
                areaInset.right -= moveDir.x;
                break;
            case DirType.DOWNLEFT:
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, areaInset.up, rectTransform.rect.height - moveDir.y);
                areaInset.down += moveDir.y;
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, areaInset.right, rectTransform.rect.width - moveDir.x);
                areaInset.left += moveDir.x;
                break;
            case DirType.DOWNRIGHT:
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, areaInset.up, rectTransform.rect.height - moveDir.y);
                areaInset.down += moveDir.y;
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, areaInset.left, rectTransform.rect.width + moveDir.x);
                areaInset.right -= moveDir.x;
                break;
            case DirType.UP:
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, areaInset.down, rectTransform.rect.height + moveDir.y);
                areaInset.up -= moveDir.y;
                break;
            case DirType.LEFT:
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, areaInset.right, rectTransform.rect.width - moveDir.x);
                areaInset.left += moveDir.x;
                break;
            case DirType.DOWN:
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, areaInset.up, rectTransform.rect.height - moveDir.y);
                areaInset.down += moveDir.y;
                break;
            case DirType.RIGHT:
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, areaInset.left, rectTransform.rect.width + moveDir.x);
                areaInset.right -= moveDir.x;
                break;
            case DirType.CENTER:
                rectTransform.anchoredPosition += moveDir;
                break;
            case DirType.NONE:   
                break;
        }
    }

    public void OnPointerEnter(int dir)
    {
        if (!isPointerEnter)
        {
            isPointerEnter = true;
            upPointerPos = (Vector2)Input.mousePosition;
            DirType dirType = (DirType)dir;
            this.dir = dirType;
            CursorView.Instance.Set(dirType);
        }
    }

    public void OnPointerExit()
    {
        if (!Input.GetMouseButton(0))
        {
            dir = DirType.NONE;
            isPointerEnter = false;
            CursorView.Instance.Set(DirType.NONE);
        }
    }

    int i = 0;
    public void CaptureScreenShot()
    {
        i = 0;
        //StartCoroutine(CaptureShot());
    }

    private IEnumerator CaptureShot()
    {
        while (i <= captureShotFrameRange.end)
        {
            yield return new WaitForEndOfFrame();

            if (i <= captureShotFrameRange.end && i >= captureShotFrameRange.start)
            {
               
                Texture2D screenShot = new Texture2D((int)rectTransform.rect.width, (int)rectTransform.rect.height, TextureFormat.RGB24, false);

                Vector3[] corners = new Vector3[4];

                rectTransform.GetWorldCorners(corners);

                screenShot.ReadPixels(new Rect(corners[0].x, corners[0].y, rectTransform.rect.width, rectTransform.rect.height), 0, 0);

                screenShot.Apply();

                byte[] bytes = screenShot.EncodeToJPG();

                byte[] sendType = BitConverter.GetBytes((int)DataSendType.IMAGE);
                byte[] length = BitConverter.GetBytes(bytes.Length + 4 + 4);
                List<byte> allMessage = new List<byte>();
                allMessage.AddRange(sendType);
                allMessage.AddRange(length);
                allMessage.AddRange(bytes);
                byte[] allMessageBytes = allMessage.ToArray();

                ServerDemo.Instance.server.Send(allMessageBytes);

                
            }
            i++;
        }

    }

    public CameraData cameraData = new CameraData();
    public void UpdateCameraData()
    {
        cameraData.captureAreaXPos = rectTransform.anchoredPosition.x;
        cameraData.captureAreaYPos = rectTransform.anchoredPosition.x;
        cameraData.captureAreaWidth = rectTransform.rect.width;
        cameraData.captureAreaHeight = rectTransform.rect.height;
        cameraData.cameraAngle = Camera.main.transform.GetComponent<RtsCamera>().Rotation;

        string sendMessage = JsonUtility.ToJson(cameraData);
        byte[] bytes = Encoding.UTF8.GetBytes(sendMessage);
        byte[] sendType = BitConverter.GetBytes((int)DataSendType.CAMERA);
        byte[] length = BitConverter.GetBytes(bytes.Length + 4 + 4);
        List<byte> allMessage = new List<byte>();
        allMessage.AddRange(sendType);
        allMessage.AddRange(length);
        allMessage.AddRange(bytes);
        byte[] allMessageBytes = allMessage.ToArray();

        ServerDemo.Instance.server.Send(allMessageBytes);
    }

}
