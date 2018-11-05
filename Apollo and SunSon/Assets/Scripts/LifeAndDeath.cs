using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeAndDeath : MonoBehaviour {

    public float objectHealth;
    public GameObject DeathParticles;
    public GameObject ObjectGo;
	
	public void TakeDamage(float damage)
    {
        objectHealth -= damage;
        if(objectHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(DeathParticles, ObjectGo.transform.position, Quaternion.identity);
        Destroy(ObjectGo);
    }
}
