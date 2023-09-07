using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class ButtonWhichCanBePressedLimitedQuantityOfTimes : MonoBehaviour
{
    [SerializeField] private int m_QuantityOfTimes = 1;
    [SerializeField] private Text m_TextWithQuantityOfTimes;

    private Button m_ThisButton;

    private void Awake()
    {
        ShowQuantityOfTimes();
        m_ThisButton = GetComponent<Button>();
        ColorBlock ColorsOfThisButton = m_ThisButton.colors;
        ColorsOfThisButton.disabledColor = ColorsOfThisButton.normalColor;
        m_ThisButton.colors = ColorsOfThisButton;
        m_ThisButton.onClick.AddListener(() =>
        {
            m_QuantityOfTimes--;
            ShowQuantityOfTimes();
            if (m_QuantityOfTimes <= 0)
            {
                m_ThisButton.interactable = false;
            }
        });
    }

    private void ShowQuantityOfTimes()
    {
        if (m_TextWithQuantityOfTimes != null)
        {
            m_TextWithQuantityOfTimes.text = m_QuantityOfTimes.ToString();
        }
    }
}