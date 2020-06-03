using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public bool flipX = false;
    public float velocidad = 10f;

    Rigidbody2D rb;
    Animator animator;

    Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        move = new Vector2();
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        move.Normalize();

        if (flipX)
        {
            if (move.x < 0) transform.localScale = new Vector2(-1, 1);
            else if (move.x > 0) transform.localScale = new Vector2(1, 1);
        }

        if (move != Vector2.zero)
        {
            animator.SetFloat("Movimiento X", move.x);
            animator.SetFloat("Movimiento Y", move.y);
            animator.SetBool("Moviendo", true);
        }
        else
        {
            animator.SetBool("Moviendo", false);
        }
        

        rb.MovePosition(transform.position + (Vector3)move * Time.deltaTime * velocidad);
    }
}
