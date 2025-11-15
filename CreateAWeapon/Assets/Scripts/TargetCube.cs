using UnityEngine;

public class TargetCube : MonoBehaviour
{
    public string bulletTag = "Bullet";
    public string explosionTag = "Explosion";

    public bool destroyBulletOnHit = true;

    public ParticleSystem destroyEffect;

    // Checking and handling bullet hit using OnCollisionEnter
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag(bulletTag))
        {
            HandleHit(collision.collider.gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(explosionTag))
        {
            HandleHit(other.gameObject);
        }
    }

    // Deletes Object when hit
    private void HandleHit(GameObject objectHit)
    {
        // Destroys bullet
        if (destroyBulletOnHit & objectHit.CompareTag(bulletTag))
        {
            Destroy(objectHit);
        }

        Destroy(gameObject);
    }
}
