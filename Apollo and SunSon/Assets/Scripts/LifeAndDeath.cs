using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeAndDeath : MonoBehaviour {

    public float ObjectHealth;
    float currentHealth;
    public GameObject DeathParticles;
    public GameObject ObjectGo;
    public Slider HealthBar;
    bool useSlider = false;
    public Image HeathBarImage;

    private void Start()
    {
        currentHealth = ObjectHealth;

        if(HealthBar != null)
        {
            // we have a heath bar, use it
            useSlider = true;
        }
        if (useSlider)
        {
            // set max heath
            HealthBar.maxValue = ObjectHealth;
            // start full health
            HealthBar.value = ObjectHealth;
        }
        else
        {
            HeathBarImage.fillAmount = 1;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            UpdateHealthBar();

            Die();
        }
    }

    void Die()
    {
        Instantiate(DeathParticles, ObjectGo.transform.position, Quaternion.identity);

        if (ObjectGo.transform.CompareTag("Player"))
        {
            GameObject ss = GameObject.FindGameObjectWithTag("SunSon");
            Instantiate(DeathParticles, ss.transform.position, Quaternion.identity);
            Destroy(ss);
        }

        Destroy(ObjectGo);
    }

    void UpdateHealthBar()
    {
        if(useSlider)
            HealthBar.value = currentHealth;
        else
        {
            float heathPercent = currentHealth / ObjectHealth;
            HeathBarImage.fillAmount = heathPercent;
        }
    }
}
