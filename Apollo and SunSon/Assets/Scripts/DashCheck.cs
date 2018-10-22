using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if(DashDirection == 1)
        {
            transform.Translate(new Vector3(cc.InitialDashLength, 0f, 0f));
        }
        else if(DashDirection == 2)
        {
            transform.Translate(new Vector3(-cc.InitialDashLength, 0f, 0f));
        }

        initialDashPoint = transform.localPosition;
    }

    public CharacterController2D cc;
    public Collider2D col;

    public int DashDirection; // 1 = right, 2 = left
    Vector3 initialDashPoint;
    public float MinDistanceFromObject = 0.25f;

    ColliderDistance2D dist;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other != null)
        {
            // get the distance between the players collider and whatever is in front of them
            dist = col.Distance(other);

            // turn off dash if the player is too close to an object
            if(dist.distance < MinDistanceFromObject)
            {
                cc.CanDash(DashDirection, false, cc.InitialDashLength);
            }
            else if(dist.distance > cc.InitialDashLength)
            {
                cc.CanDash(DashDirection, true, cc.InitialDashLength);
            }
            // if they are closer than a full dash length, shorten the length to meet the object ahead
            else
            {
                cc.CanDash(DashDirection, true, dist.distance);
            }
        }        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // reset the dash ability when moving out of the collider ahead
        cc.CanDash(DashDirection, true, cc.InitialDashLength);
        transform.localPosition = initialDashPoint;
    }
}
