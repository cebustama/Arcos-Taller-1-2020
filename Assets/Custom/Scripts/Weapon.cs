using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Crosshair")]
    public Sprite crossHair;
    public float crossHairSize = 1f;

    Vector2 mousePos;

    GameObject crossHairObj;

    ObjectShooterProfe[] shooters;

    bool equipped = false;

    private void Awake()
    {
        if (crossHair != null)
        {
            crossHairObj = new GameObject("crosshair");
            crossHairObj.transform.SetParent(transform);
            crossHairObj.transform.localPosition = Vector3.zero;
            crossHairObj.transform.localScale = Vector3.one * crossHairSize;
            crossHairObj.AddComponent<SpriteRenderer>().sprite = crossHair;
        }

        shooters = GetComponentsInChildren<ObjectShooterProfe>();
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
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = (mousePos - (Vector2)transform.position);

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;

        transform.localEulerAngles = new Vector3(0, 0, angle);

        // Mover la mira
        if (crossHairObj != null)
        {
            crossHairObj.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }

        // Disparar
        if (Input.GetMouseButton(0))
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