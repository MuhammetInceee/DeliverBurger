using System.Collections;
using MoreMountains.NiceVibrations;
using UnityEngine;


public class ExplosivePart : MonoBehaviour
{
    public bool Exploded = false;
    public float ExplosionForce = 35;
    public float ExplosionRadius = 1f;
    public float ColliderGrowthRate = 2f;
    public float ColliderGrowthDuration = 0.1f;
    private float _expTime = 0.1f;
    private Rigidbody _rb;
    private BoxCollider _boxCollider;
    private Vector3 originalSize;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
    }
    public void Explosion(float expTime)
    {
        _expTime= expTime;
        StartCoroutine(ExplosionCoroutine(expTime));
    }
    private IEnumerator ExplosionCoroutine(float expTime)
    {
        yield return new WaitForSeconds(expTime);
        Exploded = true;
        _rb.isKinematic = false;
        originalSize = _boxCollider.size;
        if (_boxCollider == null)  yield return null;
      
        var targetSize = originalSize * ColliderGrowthRate;
        for (var i = 0; i < targetSize.x; i++)
        {
            yield return new WaitForSeconds(0.01f);
            _boxCollider.size += Vector3.one * (Time.deltaTime * 20);
        }
       
    }

    private void OnCollisionEnter(Collision other)
    {
        var explosivePart = other.gameObject.GetComponent<ExplosivePart>();

        if (Exploded)
        {
            if (explosivePart != null)
            {

                explosivePart.Explosion(UnityEngine.Random.Range(0, 0.3f));
            }
            var explosionPos = transform.position;
            var colliders = Physics.OverlapSphere(explosionPos, ExplosionRadius);
            foreach (var hit in colliders)
            {
                var rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.transform.SetParent(null);
                    rb.AddExplosionForce(ExplosionForce, explosionPos, ExplosionRadius);
                }
            }
            PoolingSystem.Instance.InstantiateAPS("Explotion",transform.position);
            Destroy(gameObject, _expTime);
            GameManager.Instance.HitHaptic(HapticTypes.HeavyImpact);
        }
    }


    private void OnDestroy() => StopCoroutine(ExplosionCoroutine(_expTime));
    
    public void Destroy() => Destroy(gameObject);
    
}
