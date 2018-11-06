using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float FollowSpeed;
    public Vector3 MinTargetOffset;
    public Vector3 MaxTargetOffset;
    Vector3 TargetOffset;
    public float TimeBetweenMove;
    public float currentTimeBetweenMove;

    Transform target;
    Rigidbody2D targetRb;

    public GameObject ProjectilePrefab;
    public float ProjectileSpeed = 10f;
    public float ProjectileCooldownMin = 0.5f;
    public float ProjectileCooldownMax = 3f;
    public float AimXOffset;
    float currentCooldown;
    float randXOffset;

    // Use this for initialization
    void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        targetRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        currentCooldown = ProjectileCooldownMax;
    }
	
	// Update is called once per frame
	void Update () {

        if (target != null)
        {
            if (currentTimeBetweenMove <= 0)
            {
                // get random target offset to move towards
                float randX = Random.Range(MinTargetOffset.x, MaxTargetOffset.x);
                float randY = Random.Range(MinTargetOffset.y, MaxTargetOffset.y);
                TargetOffset = new Vector3(randX, randY, 0f);
                currentTimeBetweenMove = TimeBetweenMove;
            }
            else
            {
                currentTimeBetweenMove -= Time.deltaTime;
            }

            transform.position = Vector2.MoveTowards(transform.position, target.position + TargetOffset, FollowSpeed * Time.deltaTime);

            // can we shoot?
            if (currentCooldown <= 0)
            {
                // if the player is still, aim direct, otherwise add some aim fuzzyness
                if (targetRb.velocity.x > 0.1f)
                    randXOffset = Random.Range(-AimXOffset, AimXOffset);
                else
                    randXOffset = 0;

                // reset cooldown
                currentCooldown = Random.Range(ProjectileCooldownMin, ProjectileCooldownMax);
                // spawn projectile
                Vector2 targetPos = new Vector2(target.position.x + randXOffset, target.position.y);
                Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
                Vector2 direction = targetPos - myPos;
                direction.Normalize();
                //Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
                GameObject projectile = (GameObject)Instantiate(ProjectilePrefab, myPos, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().velocity = direction * ProjectileSpeed;
            }
            else
                currentCooldown -= Time.deltaTime;
        }

	}
}
