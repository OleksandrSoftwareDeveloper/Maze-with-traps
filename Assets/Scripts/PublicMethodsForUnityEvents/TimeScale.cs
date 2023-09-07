using UnityEngine;

public class TimeScale : MonoBehaviour
{
    public void SetTimeScale(int newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }
}