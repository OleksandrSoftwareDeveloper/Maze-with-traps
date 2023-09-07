using UnityEngine;

[CreateAssetMenu(fileName = "ExistingLayersAndTags", menuName = "ScriptableObjects/GameData/ExistingLayersAndTags")]
public class ExistingLayersAndTags : ScriptableObject
{
    [SerializeField] private int m_PlayerLayer;
    public int PlayerLayer { get => m_PlayerLayer; }

    [SerializeField] private string m_PlayerTag;
    public string PlayerTag { get => m_PlayerTag; }
}