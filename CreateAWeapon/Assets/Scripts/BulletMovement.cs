using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed = 30f;
    [SerializeField] private float _lifespan = 5f;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(transform.forward * _bulletSpeed, ForceMode.Impulse);

        GameObject.Destroy(gameObject, _lifespan);
    }
}
