using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public GameObject explosion;
    public float lifetime = 5f;
    public float ProjectileDamage = 1f;

    private void Update()
    {
        if(lifetime <= 0)
        {
            Explode();
        }
        else
        {
            lifetime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (
            (other.transform.CompareTag("Enemy") && !transform.CompareTag("Enemy"))
            || (other.transform.CompareTag("Player") && !transform.CompareTag("Player"))
            || other.transform.CompareTag("Environment"))
        {
            if (other.transform.CompareTag("Enemy") || other.transform.CompareTag("Player"))
            {
                // Enemy hit... remove health
                other.GetComponent<LifeAndDeath>().TakeDamage(ProjectileDamage);
            }
            Explode();
        }
    }

    public void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
