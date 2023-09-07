using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour
{
    [SerializeField] private ExistingLayersAndTags m_ExistingLayersAndTags;

    private void Start()
    {
        Debug.Log(m_ExistingLayersAndTags == null);
    }

    private void OnCollisionEnter(Collision theCollision)
    {
        if(theCollision.collider.gameObject.layer == m_ExistingLayersAndTags.PlayerLayer)
        {
            GameEvents.OnWin?.Invoke();
        }
    }
}