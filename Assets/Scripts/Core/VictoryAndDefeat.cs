using System.Collections.Generic;
using UnityEngine;

public class VictoryAndDefeat : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_GameObjectsWhichShouldBeEnabledOnVictory = new();
    [SerializeField] private List<GameObject> m_GameObjectsWhichShouldBeDisabledOnVictory = new();

    [SerializeField] private List<GameObject> m_GameObjectsWhichShouldBeEnabledOnDefeat = new();
    [SerializeField] private List<GameObject> m_GameObjectsWhichShouldBeDisabledOnDefeat = new();

    private void StopGame()
    {
        Time.timeScale = 0;
    }

    private void Win()
    {
        StopGame();
        m_GameObjectsWhichShouldBeEnabledOnVictory.ForEach(gameObject => gameObject.SetActive(true));
        m_GameObjectsWhichShouldBeDisabledOnVictory.ForEach(gameObject => gameObject.SetActive(false));
    }

    private void Lose()
    {
        StopGame();
        m_GameObjectsWhichShouldBeEnabledOnDefeat.ForEach(gameObject => gameObject.SetActive(true));
        m_GameObjectsWhichShouldBeDisabledOnDefeat.ForEach(gameObject => gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        GameEvents.OnWin += Win;
        GameEvents.OnLose += Lose;
    }

    private void OnDestroy()
    {
        GameEvents.OnWin -= Win;
        GameEvents.OnLose -= Lose;
    }
}