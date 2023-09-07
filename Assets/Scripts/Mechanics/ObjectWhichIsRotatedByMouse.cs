using UnityEngine;
using UnityEngine.UI;

public class ObjectWhichIsRotatedByMouse : MonoBehaviour
{
    [SerializeField] private float m_StartSensitivityIfNoSaved;
    [SerializeField] private float m_MinimalSensitivity;
    [SerializeField] private float m_MaximalSensitivity;
    [SerializeField] private float m_MaximalYRotation;
    [SerializeField] private Slider m_SensitivitySlider;

    private float m_CurrentSensitivity;
    private Vector2 m_CurrentRotation;
    private string m_KeyForPlayerPrefs = "Sensitivity";

    private void Awake()
    {
        m_StartSensitivityIfNoSaved = Mathf.Clamp(m_StartSensitivityIfNoSaved, m_MinimalSensitivity, m_MaximalSensitivity);
        float CurrentSensitivity = PlayerPrefs.HasKey(m_KeyForPlayerPrefs) ? PlayerPrefs.GetFloat(m_KeyForPlayerPrefs) : m_StartSensitivityIfNoSaved;
        SetSensitivity(CurrentSensitivity);
        if(m_SensitivitySlider != null)
        {
            m_SensitivitySlider.minValue = m_MinimalSensitivity;
            m_SensitivitySlider.maxValue = m_MaximalSensitivity;
            m_SensitivitySlider.value = CurrentSensitivity;
            m_SensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }
    }

    private void SetSensitivity(float newSensitivity)
    {
        m_CurrentSensitivity = newSensitivity;
        PlayerPrefs.SetFloat(m_KeyForPlayerPrefs, newSensitivity);
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            m_CurrentRotation.x += Input.GetAxis("Mouse X") * m_CurrentSensitivity;
            m_CurrentRotation.y -= Input.GetAxis("Mouse Y") * m_CurrentSensitivity;
            m_CurrentRotation.x = Mathf.Repeat(m_CurrentRotation.x, 360);
            m_CurrentRotation.y = Mathf.Clamp(m_CurrentRotation.y, -m_MaximalYRotation, m_MaximalYRotation);
            transform.rotation = Quaternion.Euler(m_CurrentRotation.y, m_CurrentRotation.x, 0);
        }
    }
}