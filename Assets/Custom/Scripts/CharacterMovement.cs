using System;
using UnityEngine;

using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    public bool flipX;
    public float speed;

    [Header("References")]
    public Image healthBar;

    [Header("Enemy AI")]
    public bool isEnemy = false;
    public float wanderMove = 0f;
    public float followPlayerMove = 0f;
    public float playerDistanceLimit = 3f;

    Rigidbody2D rb;
    Animator animator;
    GameObject animatorObject;

    Vector2 move;
    Vector2 force;
    float baseScaleX;
    float t;

    // Scripts de Playground
    HealthSystemProfe healthSystem;

    GameObject player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Couldn't find Animator component for " + gameObject.name);
        }
        animatorObject = animator.gameObject;

        baseScaleX = transform.localScale.x;

        healthSystem = GetComponent<HealthSystemProfe>();

        // Si es enemigo, buscar al player
        if (isEnemy)
        {
            InitEnemy();    
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Obtener el movimiendo desde los controles
        if (!isEnemy)
        {
            move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        // Comportamiento del enemigo
        else
        {
            UpdateEnemy();
        }

        UpdateAnimation();

        UpdateUI();    
    }

    private void FixedUpdate()
    {
        if (!isEnemy)
        {
            // Mover objeto a través del RigidBody2D
            rb.MovePosition(transform.position + (Vector3)move * Time.fixedDeltaTime * speed);
        }
        else
        {
            rb.AddForce(force);
        }
    }

    void UpdateAnimation()
    {
        // Setear parámetros de animación
        if (move != Vector2.zero)
        {
            animator.SetFloat("Move X", move.x);
            animator.SetFloat("Move Y", move.y);

            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        // Dar vuelta el personaje
        if (move.x > 0)
        {
            animatorObject.transform.localScale = new Vector2(baseScaleX, transform.localScale.y);
        }
        else if (move.x < 0)
        {
            animatorObject.transform.localScale = new Vector2(-baseScaleX, transform.localScale.y);
        }
    }

    void UpdateUI()
    {
        // Barra de vida
        if (healthBar != null)
        {
            if (healthSystem != null)
            {
                healthBar.fillAmount = ((float)healthSystem.health / (float)healthSystem.maxHealth);
            }
        }
    }

    #region Enemy Behaviour

    void InitEnemy()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        UnityEngine.Random.InitState(System.Environment.TickCount);
        t = UnityEngine.Random.Range(0.1f, 99.9f);

        gameObject.tag = "Enemy";
    }

    void UpdateEnemy()
    {
        force = Vector2.zero;

        WanderMove();
        FollowPlayer();

        move = force.normalized;
    }

    void WanderMove()
    {
        Vector2 dir;
        dir.x = Mathf.PerlinNoise(t, 0) * 2 - 1;
        dir.y = Mathf.PerlinNoise(0, t) * 2 - 1;
        dir.Normalize();
        t += 0.001f;

        Vector2 wanderForce = dir * wanderMove;
        force += wanderForce;
    }

    void FollowPlayer()
    {
        // Obtener vector de dirección hacia el player y normalizarlo, luego multiplar por factor
        Vector3 playerDir = (player.transform.position - transform.position);
        float playerDist = playerDir.sqrMagnitude;
        playerDir.Normalize();

        Vector3 point = (player.transform.position - playerDistanceLimit * playerDir);
        Vector3 pointDir = (point - transform.position);
        float pointDist = pointDir.sqrMagnitude;
        pointDir.Normalize();

        Vector2 followForce = pointDir;
        followForce *= (pointDist >= 0.2f) ? followPlayerMove : 0f;
        force += followForce;
    }

    #endregion
}
