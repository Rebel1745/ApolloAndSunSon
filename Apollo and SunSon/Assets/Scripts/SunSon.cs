using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunSon : MonoBehaviour {

    public GameObject ProjectilePrefab;
    public float ProjectileSpeed = 10f;
    public float ProjectileCost = 10f;
    public float ProjectileCooldown = 0.5f;
    float currentCooldown;
    bool canShoot = true;

    // Allow the light levels to be manipulated
    public Light PointLight;
    public Light AmbientLight;

    public float BaseLightLevelPoint = 1f;
    public float BaseLightLevelAmbient = 0.1f;
    public float PointLightMultiplier = 1f;
    public float AmbientLightMultiplier = 1f;
    readonly float maxLightLevelPoint = 50f;
    readonly float maxLightLevelAmbient = 1f;

    public float MaxEnergy = 100f;
    public float EnergyRegen = 10f;
    public float EnergyCooldown = 1.5f;
    float currentEnergyCooldown = 0;
    float currentEnergy;

    public Slider EnergySlider;

    private void Start()
    {
        currentEnergy = MaxEnergy;
        EnergySlider.value = currentEnergy;
        canShoot = true;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        CanShoot();

        UpdateEnergy();

        UpdateLighting();
	}

    void Shoot()
    {
        if (canShoot && (currentEnergy - ProjectileCost) >= 0)
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = target - myPos;
            direction.Normalize();
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            GameObject projectile = (GameObject)Instantiate(ProjectilePrefab, myPos, rotation);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * ProjectileSpeed;

            canShoot = false;
            currentCooldown = ProjectileCooldown;
            currentEnergy -= ProjectileCost;
            currentEnergyCooldown = EnergyCooldown;
        }
    }

    void UpdateLighting()
    {
        PointLight.range = Mathf.Clamp(BaseLightLevelPoint * PointLightMultiplier, BaseLightLevelPoint, maxLightLevelPoint);
        //ambientLight.intensity = Mathf.Clamp(baseLightLevelAmbient * ambientLightMultiplier, baseLightLevelAmbient, maxLightLevelAmbient);
    }

    void UpdateEnergy()
    {
        if (currentEnergyCooldown <= 0)
        {
            currentEnergy = Mathf.Min(currentEnergy + EnergyRegen * Time.deltaTime, MaxEnergy);
        }
        else
        {
            currentEnergyCooldown -= Time.deltaTime;
        }

        EnergySlider.value = currentEnergy;
    }

    void CanShoot()
    {
        if(currentEnergy <= 0)
        {
            canShoot = false;
        }
        else
        {
            if (!canShoot)
            {
                currentCooldown -= Time.deltaTime;
            }

            if (currentCooldown <= 0)
            {
                canShoot = true;
                currentCooldown = 0;
            }
        }        
    }
}
