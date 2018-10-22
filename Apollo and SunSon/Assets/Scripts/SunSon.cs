using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSon : MonoBehaviour {

    public GameObject projectilePrefab;
    public float projectileSpeed = 20;

    // Allow the light levels to be manipulated
    public Light pointLight;
    public Light ambientLight;

    public float baseLightLevelPoint = 1f;
    public float baseLightLevelAmbient = 0.1f;
    public float pointLightMultiplier = 1f;
    public float ambientLightMultiplier = 1f;
    readonly float maxLightLevelPoint = 50f;
    readonly float maxLightLevelAmbient = 1f;

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        UpdateLighting();
	}

    void Shoot()
    {
        // TODO: Some sort of bug that alters the velocity of the projectile according to how close you click to SunSon
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 origin = transform.position;
        Vector3 dir = mousePos - origin;

        GameObject projectile = Instantiate(projectilePrefab, origin, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;
    }

    void UpdateLighting()
    {
        pointLight.range = Mathf.Clamp(baseLightLevelPoint * pointLightMultiplier, baseLightLevelPoint, maxLightLevelPoint);
        //ambientLight.intensity = Mathf.Clamp(baseLightLevelAmbient * ambientLightMultiplier, baseLightLevelAmbient, maxLightLevelAmbient);
    }
}
