using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public float speed = 15f;
    public float minDistance;
    public GameObject target;
    public Vector3 offset;

    private Vector3 targetPos;

    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position + offset;
            Vector3 targetDirection = (target.transform.position - posNoZ);
            float interpVelocity = targetDirection.magnitude * speed;
            targetPos = (transform.position) + (targetDirection.normalized * interpVelocity * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.25f);

        }
    }
}