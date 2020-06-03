using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool autoshoot = false;

    [Header("Crosshair")]
    public Sprite crossHair;
    public float crossHairSize = 1f;

    CharacterMovement character;

    Vector2 mousePos;

    bool isEnemyWeapon = false;
    Transform target;

    GameObject crossHairObj;

    ObjectShooterProfe[] shooters;

    bool equipped = false;

    private void Awake()
    {
        gameObject.tag = "Weapon";

        if (crossHair != null)
        {
            crossHairObj = new GameObject("crosshair");
            crossHairObj.transform.SetParent(transform);
            crossHairObj.transform.localPosition = Vector3.zero;
            crossHairObj.transform.localScale = Vector3.one * crossHairSize;
            crossHairObj.AddComponent<SpriteRenderer>().sprite = crossHair;
        }

        shooters = GetComponentsInChildren<ObjectShooterProfe>();

        // Enemy weapons
        character = (transform.parent != null) ? transform.parent.GetComponent<CharacterMovement>() : null;
        if (character != null && character.isEnemy)
        {
            isEnemyWeapon = true;
            equipped = true;
            autoshoot = true;
            if (crossHairObj != null) Destroy(crossHairObj);

            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (!equipped)
        {
            if (crossHairObj != null) crossHairObj.SetActive(false);
            return;
        }

        // Asignar layer del arma a la misma que el objeto padre (ya sea player o enemy)
        if (gameObject.layer == 0)
        {
            gameObject.layer = transform.parent.gameObject.layer;
        }

        // Rotar arma
        Vector2 targetPos = Vector2.zero;

        if (isEnemyWeapon)
        {
            targetPos = target.transform.position;
        }
        else
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos = mousePos;
        }

        Vector2 lookDir = (targetPos - (Vector2)transform.position);

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;

        transform.localEulerAngles = new Vector3(0, 0, angle);

        // Mover la mira
        if (crossHairObj != null)
        {
            crossHairObj.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }

        // Disparar
        if (Input.GetMouseButton(0) || autoshoot)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        foreach (ObjectShooterProfe s in shooters)
        {
            // Disparar por cada punto de disparo
            s.Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Found weapon " + gameObject.name);

            // Si el player ya tiene un arma equipada, reemplazarla
            Weapon currentWeapon = collision.gameObject.GetComponentInChildren<Weapon>();
            if (currentWeapon != null)
            {
                currentWeapon.Drop();
            }

            transform.SetParent(collision.transform);
            transform.localPosition = Vector3.zero;
            if (crossHairObj != null)  crossHairObj.SetActive(true);
            equipped = true;
        }
    }

    public void Drop()
    {
        transform.SetParent(null);
        equipped = false;

        Invoke("ResetLayer", 1f);
    }

    public void ResetLayer()
    {
        gameObject.layer = 0;
    }
}