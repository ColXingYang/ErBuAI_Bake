using UnityEngine;

/// <summary>
/// Encapsulates mouse movement for RtsCamera.
/// </summary>
[AddComponentMenu("Camera-Control/RtsCamera-Mouse")]
public class RtsCameraMouse : MonoBehaviour
{
    public KeyCode MouseOrbitButton;

    public bool AllowScreenEdgeMove;
    public bool ScreenEdgeMoveBreaksFollow;
    public int ScreenEdgeBorderWidth;
    public float MoveSpeed;

    public bool AllowPan;
    public bool PanBreaksFollow;
    public float PanSpeed;

    public bool AllowRotate;
    public float RotateSpeed;

    public bool AllowTilt;
    public float TiltSpeed;

    public bool AllowZoom;
    public float ZoomSpeed;

    public string RotateInputAxis = "Mouse X";
    public string TiltInputAxis = "Mouse Y";
    public string ZoomInputAxis = "Mouse ScrollWheel";
    public KeyCode PanKey1 = KeyCode.LeftShift;
    public KeyCode PanKey2 = KeyCode.RightShift;

    //

    private RtsCamera _rtsCamera;

    //private Transform textparent;

    //

    protected void Reset()
    {
        MouseOrbitButton = KeyCode.Mouse2;    // middle mouse by default (probably should not use right mouse since it doesn't work well in browsers)

        AllowScreenEdgeMove = true;
        ScreenEdgeMoveBreaksFollow = true;
        ScreenEdgeBorderWidth = 4;
        MoveSpeed = 30f;

        AllowPan = true;
        PanBreaksFollow = true;
        PanSpeed = 30f;
        PanKey1 = KeyCode.LeftShift;
        PanKey2 = KeyCode.RightShift;

        AllowRotate = true;
        RotateSpeed = 360f;

        AllowTilt = true;
        TiltSpeed = 200f;

        AllowZoom = true;
        ZoomSpeed = 500f;

        RotateInputAxis = "Mouse X";
        TiltInputAxis = "Mouse Y";
        ZoomInputAxis = "Mouse ScrollWheel";
    }

    protected void Start()
    {
        _rtsCamera = gameObject.GetComponent<RtsCamera>();
    }

    protected void LateUpdate()
    {
        if (_rtsCamera == null)
            return; 

        if (AllowZoom)
        {
            var scroll = Input.GetAxisRaw(ZoomInputAxis);

            _rtsCamera.Distance -= scroll * ZoomSpeed * Time.unscaledDeltaTime * 2;
   
        }

        if (Input.GetKey(MouseOrbitButton))
        {
            if (AllowPan)
            {
     

                var panX = -1 * Input.GetAxisRaw("Mouse X") * PanSpeed * Time.unscaledDeltaTime * 2;
                var panZ = -1 * Input.GetAxisRaw("Mouse Y") * PanSpeed * Time.unscaledDeltaTime * 2;

                _rtsCamera.AddToPosition(panX, 0, panZ);

                if (PanBreaksFollow && (Mathf.Abs(panX) > 0.001f || Mathf.Abs(panZ) > 0.001f))
                {
                    _rtsCamera.EndFollow();
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (AllowTilt)
            {
                var tilt = Input.GetAxisRaw(TiltInputAxis);
                _rtsCamera.Tilt -= tilt * TiltSpeed * Time.unscaledDeltaTime * 2;
            }

            if (AllowRotate)
            {
                var rot = Input.GetAxisRaw(RotateInputAxis);
                _rtsCamera.Rotation += rot * RotateSpeed * Time.unscaledDeltaTime * 2;
            }
        }

    }
}
