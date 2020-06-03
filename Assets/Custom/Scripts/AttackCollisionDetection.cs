using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollisionDetection : MonoBehaviour
{
    // This function gets called everytime this object collides with another
    private void OnTriggerEnter2D(Collider2D collisionData)
    {
        transform.parent.GetComponent<AttackFront>().OnTriggerChildEnter2D(collisionData, transform.position);
    }
}
