using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject m_ExplosionParticles;
    [SerializeField] private ExistingLayersAndTags m_ExistingLayersAndTags;

    private void OnCollisionEnter(Collision theCollision)
    {
        Instantiate(m_ExplosionParticles);
        m_ExplosionParticles.transform.position = theCollision.GetContact(0).point;
        if(theCollision.collider.gameObject.layer == m_ExistingLayersAndTags.PlayerLayer)
        {
            GameEvents.OnLose?.Invoke();
        }
        Destroy(gameObject);
    }
}