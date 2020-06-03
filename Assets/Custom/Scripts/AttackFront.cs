using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFront : MonoBehaviour
{
    [Header("Input key")]
    public KeyCode key = KeyCode.Space;

    [Header("Attack settings")]
    public GameObject attackEffect;
    public int healthChange = -1;
    public float force = 5f;

    float attackTime = 0.2f;
    public float cooldown = 0.5f;

    float attackTimer;
    float cooldownTimer;

    GameObject attackHitbox;
    Vector2 direction;

    private Vector2 movement, cachedDirection;
    private float moveHorizontal;
    private float moveVertical;

    private bool keyPressed = false;

    void Awake()
    {
        attackHitbox = transform.GetChild(0).gameObject;
        attackHitbox.SetActive(false);

        direction = Vector2.right;
    }

    void Update()
    {
        GetMoveDirection();

        Vector2 moveNormalized = movement.normalized;
        float angle = Mathf.Atan2(moveNormalized.y, moveNormalized.x) * Mathf.Rad2Deg;

        if (attackTimer <= 0 && moveNormalized != Vector2.zero)
        {
            transform.localEulerAngles = new Vector3(0, 0, angle);
        }

        keyPressed = Input.GetKey(key);

        // Timer management
        attackTimer -= Time.deltaTime;
        cooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (keyPressed && attackTimer <= 0 && cooldownTimer <= 0)
        {
            direction = movement.normalized;
            attackHitbox.SetActive(true);
            attackTimer = attackTime;

            if (attackEffect != null)
            {
                GameObject newObject = Instantiate<GameObject>(attackEffect);
                newObject.transform.position = attackHitbox.transform.position;
                newObject.transform.localScale = Vector2.one * 3f;
            }
        }

        if (attackTimer <= 0)
        {
            if (attackHitbox.activeSelf)
            {
                attackHitbox.SetActive(false);
                cooldownTimer = cooldown;
            }
        }
    }

    // Replicar movimiento de script move para no tener que acceder a variables privadas
    void GetMoveDirection()
    {
        // Moving with the arrow keys
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical);
    }


    public void OnTriggerChildEnter2D(Collider2D colliderData, Vector2 position)
    {
        HealthSystemAttribute healthScript = colliderData.gameObject.GetComponent<HealthSystemAttribute>();
        if (healthScript != null)
        {
            // subtract health from the player
            healthScript.ModifyHealth(healthChange);
        }

        Rigidbody2D rb = colliderData.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}
