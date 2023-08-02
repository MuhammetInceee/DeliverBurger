using MoreMountains.NiceVibrations;
using UnityEngine;

public class Food : MonoBehaviour, IDropeable
{
    private Rigidbody _rb;
    private bool _isOneTime;
    public float explosionForce = 1000f;
    public float explosionRadius = 5f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }

    public void Push(Vector3 force)
    {
        _rb.isKinematic = false;
        _rb.AddForce(_rb.mass * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var part = collision.gameObject.GetComponent<StructurePart>();
        var explosiveCube = collision.gameObject.GetComponent<ExplosivePart>();

        if (part != null)
        {
            if (!part.isActivated) part.ActivatePart();
            
            var explosionPos = collision.contacts[0].point;
            var colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            
            foreach (var hit in colliders)
            {
                var rb = hit.GetComponent<Rigidbody>();
                hit.transform.SetParent(null);
                if (rb == null) return;
                rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius);
            }

            if (!_isOneTime && transform.parent == null) _isOneTime = true;
        }
        else if (explosiveCube != null)
        {
            explosiveCube.Explosion(0.2f);
            explosiveCube.transform.SetParent(null);
            if (!_isOneTime && transform.parent == null) _isOneTime = true;
        }
        GameManager.Instance.HitHaptic(HapticTypes.MediumImpact);
    }

    public void Destroy() => Destroy(gameObject);
}