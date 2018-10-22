using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
        cc = GetComponentInParent<CharacterController2D>();
	}

    public CharacterController2D cc;

    public int DashDirection; // 1 = right, 2 = left

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(Vector2.Distance(cc.transform.position, other.transform.position));
        cc.CanDash(DashDirection, false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        cc.CanDash(DashDirection, true);
    }
}
