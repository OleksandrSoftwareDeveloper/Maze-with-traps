using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float m_TimeInSeconds = 60;
    [SerializeField] private List<Text> m_TextsWithTimerValue;

    private void FixedUpdate()
    {
        m_TimeInSeconds -= Time.fixedDeltaTime;
        m_TextsWithTimerValue.ForEach(timerText => timerText.text = $"{(int)m_TimeInSeconds / 60}:{(int)m_TimeInSeconds % 60:00}");
        if(m_TimeInSeconds <= 0)
        {
            GameEvents.OnLose?.Invoke();
        }
    }
}