using System;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public event Action<Shell> OnHitEvent;

    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private new Collider collider;

    private int _damage;

    public void Activate(int damage, Vector3 force)
    {
        _damage = damage;
        collider.enabled = true;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(force, ForceMode.Impulse);
    }

    public void Deactivate()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(_damage);
        }

        OnHitEvent?.Invoke(this);
    }
}