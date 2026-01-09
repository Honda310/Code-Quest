using UnityEngine;

public class ScreenScales : MonoBehaviour
{
    public enum ScreenScale
    {
        x4 = 4,
        x5 = 5,
        x6 = 6
    }

    public void SetScale(ScreenScale scale)
    {
        int baseWidth = 320;
        int baseHeight = 180;

        int s = (int)scale;
        Screen.SetResolution(baseWidth * s, baseHeight * s, FullScreenMode.Windowed);
    }
}
