using System;
using System.Security.Cryptography;
using UnityEngine;

public class SpecialBullet : BulletMovement
{
    [SerializeField] ParticleSystem _explosionVFX;
    private void OnCollisionEnter(Collision collision)
    {
        ParticleSystem fx = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        fx.Play();
        Destroy(gameObject);
    }

}
