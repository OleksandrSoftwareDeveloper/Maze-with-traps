using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private string m_ForwardButtonName;
    [SerializeField] private string m_BackButtonName;
    [SerializeField] private string m_LeftButtonName;
    [SerializeField] private string m_RightButtonName;
    [SerializeField] private float m_Speed = 1;
    
    private Dictionary<string, Vector3> m_ButtonsAndDirections = new();
    private Rigidbody m_Rigidbody;
    private GameObject m_MainCamera;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        m_MainCamera = Camera.main.gameObject;

        m_ButtonsAndDirections.Add(m_ForwardButtonName, Vector3.forward);
        m_ButtonsAndDirections.Add(m_BackButtonName, Vector3.back);
        m_ButtonsAndDirections.Add(m_LeftButtonName, Vector3.left);
        m_ButtonsAndDirections.Add(m_RightButtonName, Vector3.right);
    }

    private void LateUpdate()
    {
        m_Rigidbody.velocity = Vector3.zero;
        foreach(var pair in m_ButtonsAndDirections)
        {
            if(Input.GetButton(pair.Key))
            {
                float CurrentYRotationOfCamera = m_MainCamera.transform.eulerAngles.y;
                m_Rigidbody.velocity += Quaternion.AngleAxis(CurrentYRotationOfCamera, Vector3.up) * pair.Value * m_Speed;
            }
        }

        m_MainCamera.transform.position = transform.position;
    }
}