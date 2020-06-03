using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Vector2 move;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");

        move.Normalize();

        if (move.x < 0) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = new Vector3(1, 1, 1);

        animator.SetFloat("Speed", move.magnitude * 10);

        animator.SetFloat("Move X", move.x);
        animator.SetFloat("Move Y", move.y);
    }
}
