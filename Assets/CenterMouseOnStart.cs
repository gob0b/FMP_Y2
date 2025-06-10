using UnityEngine;
using System.Runtime.InteropServices;

public class CenterMouseOnStart : MonoBehaviour
{
#if UNITY_STANDALONE_WIN
    [DllImport("User32.dll")]
    private static extern bool SetCursorPos(int X, int Y);
#endif

    void Start()
    {
        // Show and unlock the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Move cursor to screen center (only works on Windows in windowed/fullscreen window)
#if UNITY_STANDALONE_WIN
        int screenCenterX = Screen.width / 2;
        int screenCenterY = Screen.height / 2;
        SetCursorPos(screenCenterX, screenCenterY);
#endif
    }
}
