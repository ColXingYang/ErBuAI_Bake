using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorView : MonoSingleton<CursorView> {

    public Texture2D UpLeft;
    public Texture2D UpRight;
    public Texture2D DownRight;
    public Texture2D DownLeft;
    public Texture2D Horizontal;
    public Texture2D Vertical;
    public Texture2D MoveArrow;

    public void Set(DirType type)
    {
        switch (type)
        {
            case DirType.UPLEFT:
                Cursor.SetCursor(UpLeft, new Vector2(5.5f, 5.5f), CursorMode.Auto);
                break;
            case DirType.UPRIGHT:
                Cursor.SetCursor(UpRight, new Vector2(5.5f, 5.5f), CursorMode.Auto);
                break;
            case DirType.DOWNLEFT:
                Cursor.SetCursor(DownLeft, new Vector2(5.5f, 5.5f), CursorMode.Auto);
                break;
            case DirType.DOWNRIGHT:
                Cursor.SetCursor(DownRight, new Vector2(5.5f, 5.5f), CursorMode.Auto);
                break;
            case DirType.UP:
                Cursor.SetCursor(Vertical, new Vector2(0, 9.5f), CursorMode.Auto);
                break;
            case DirType.LEFT:
                Cursor.SetCursor(Horizontal, new Vector2(9.5f, 0), CursorMode.Auto);
                break;
            case DirType.DOWN:
                Cursor.SetCursor(Vertical, new Vector2(0, 9.5f), CursorMode.Auto);
                break;
            case DirType.RIGHT:
                Cursor.SetCursor(Horizontal, new Vector2(9.5f, 0), CursorMode.Auto);
                break;
            case DirType.CENTER:
                Cursor.SetCursor(MoveArrow, Vector2.zero, CursorMode.Auto);
                break;
            case DirType.NONE:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}
