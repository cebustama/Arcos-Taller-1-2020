using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    Rigidbody2D rb;
    TrailRenderer trail;

    [Header("Jump")]
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpStrength = 30f;
    public string groundTag = "Ground";
    public int maxJumps = 3;

    bool checkGround = true;
    int currentJump = 0;

    [Header("Dash")]
    public float dashForce = 50f;
    public float dashCooldownTime = 1f;

    bool canJump = true;
    bool canDash = true;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailRenderer>();
        trail.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Jump
        if (canJump
            && Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

        // Dash
        if (Input.GetMouseButtonDown(0) && canDash)
        {
            Dash();    
        }
    }

    #region Player Inputs
    void Jump()
    {
        // Apply an instantaneous upwards force
        rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);

        currentJump++;

        if (currentJump >= maxJumps)
        {
            canJump = !checkGround;
        }
    }

    void Dash()
    {
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        rb.AddForce(dir * dashForce, ForceMode2D.Impulse);
        canDash = false;
        trail.emitting = true;

        Invoke("RestoreDash", dashCooldownTime);
    }

    void RestoreDash()
    {
        canDash = true;
        trail.emitting = false;
    }

    #endregion


    private void OnCollisionEnter2D(Collision2D collisionData)
    {
        if (checkGround
            && collisionData.gameObject.CompareTag(groundTag))
        {
            canJump = true;
            currentJump = 0;
        }
    }
}
