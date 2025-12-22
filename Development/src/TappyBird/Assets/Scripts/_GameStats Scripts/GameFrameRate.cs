using UnityEngine;

public class GameFrameRate : MonoBehaviour
{
    void Start()
    {
        //Application.targetFrameRate = -1;
        QualitySettings.vSyncCount = 0;

        var refreshRate = (float)Screen.currentResolution.refreshRateRatio.value;
        Application.targetFrameRate = Mathf.RoundToInt(refreshRate);
    }
}
