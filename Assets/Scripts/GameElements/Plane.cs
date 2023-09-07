using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] private GameObject m_Bomb;
    [SerializeField] private Vector3 m_BombPositionOffset;
    [SerializeField] private int m_QuantityOfBombsInARow = 5;
    [SerializeField] private float m_IntervalBetweenBombsInOneRow = 0.5f;
    [SerializeField] private float m_IntervalBetweenRows = 2.5f;

    private int m_CounterCurrentValue = 0;
    private int m_CounterValueForNextBomb = 0;
    private int m_CounterValueForNextRow = 0;
    private int m_BombsThrownInThisRow = 0;
    private bool m_IsThrowingBombsNow = true;

    private void Awake()
    {
        m_CounterValueForNextBomb = (int)(m_IntervalBetweenBombsInOneRow / Time.fixedDeltaTime);
        m_CounterValueForNextRow = (int)(m_IntervalBetweenRows / Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        if(m_IsThrowingBombsNow)
        {
            if(m_CounterCurrentValue == m_CounterValueForNextBomb)
            {
                ThrowBomb();
                m_CounterCurrentValue = 0;
                m_BombsThrownInThisRow++;
                if(m_BombsThrownInThisRow == m_QuantityOfBombsInARow)
                {
                    m_IsThrowingBombsNow = false;
                    m_BombsThrownInThisRow = 0;
                }
            }
        }
        else
        {
            if(m_CounterCurrentValue == m_CounterValueForNextRow)
            {
                m_CounterCurrentValue = 0;
                m_IsThrowingBombsNow = true;
            }
        }
        m_CounterCurrentValue++;
    }

    private void ThrowBomb()
    {
        GameObject NewBomb = Instantiate(m_Bomb);
        NewBomb.transform.position = transform.position + m_BombPositionOffset;
    }
}