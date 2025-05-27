using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public Texture2D cursor;
    public Vector2 pos = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    
    void start()
    {
        Cursor.SetCursor(cursor, pos, cursorMode);
    }
}
