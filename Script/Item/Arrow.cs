using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakePhysicalDamage(damage);
        }
        Destroy(gameObject);
    }
}
