using UnityEngine;
using System.Collections;

public class ObjectShooterProfe : MonoBehaviour
{
    [Header("Shot settings")]
    public float shakeAmount = 0f;
    public AudioClip effect;

    [Header("Object creation")]

    public GameObject prefabToSpawn;
    public bool autoShoot = false;

    // The key to press to create the objects/projectiles
    //public KeyCode keyToPress = KeyCode.Space;

    [Header("Other options")]

    // The rate of creation, as long as the key is pressed
    public float creationRate = .5f;

    // The speed at which the object are shot along the Y axis
    public float shootSpeed = 5f;

    public Vector2 shootDirection = new Vector2(1f, 1f);

    public bool relativeToRotation = true;

    private float timeOfLastSpawn;

    // Will be set to 0 or 1 depending on how the GameObject is tagged
    private int playerNumber;

    AudioSource source;

    // Use this for initialization
    void Start()
    {
        timeOfLastSpawn = -creationRate;

        // Set the player number based on the GameObject tag
        playerNumber = (gameObject.CompareTag("Player")) ? 0 : 1;

        // Obtener o crear audio source
        source = GetComponent<AudioSource>();
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (autoShoot)
        {
            Shoot();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (this.enabled)
        {
            float extraAngle = (relativeToRotation) ? transform.rotation.eulerAngles.z : 0f;
            Utils.DrawShootArrowGizmo(transform.position, shootDirection, extraAngle, 1f);
        }
    }

    public bool Shoot(bool ignoreRate = false)
    {
        if (Time.time >= timeOfLastSpawn + creationRate || ignoreRate)
        {
            Vector2 actualBulletDirection = (relativeToRotation) ? (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * shootDirection) : shootDirection;

            GameObject newObject = Instantiate<GameObject>(prefabToSpawn);
            newObject.transform.position = this.transform.position;
            newObject.transform.eulerAngles = new Vector3(0f, 0f, Utils.Angle(actualBulletDirection));
            newObject.tag = "Bullet";

            // push the created objects, but only if they have a Rigidbody2D
            Rigidbody2D rigidbody2D = newObject.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                rigidbody2D.AddForce(actualBulletDirection * shootSpeed, ForceMode2D.Impulse);
            }

            // add a Bullet component if the prefab doesn't already have one, and assign the player ID
            BulletAttribute b = newObject.GetComponent<BulletAttribute>();
            if (b == null)
            {
                b = newObject.AddComponent<BulletAttribute>();
            }
            b.playerId = playerNumber;

            timeOfLastSpawn = Time.time;

            // Screenshake
            if (shakeAmount > 0 && ScreenShakeManager.instance != null)
            {
                ScreenShakeManager.instance.AddShake(shakeAmount / 100);
            }

            // Efecto de sonido
            if (effect != null)
            {
                source.PlayOneShot(effect);
            }

            return true;
        }

        return false;
    }
}
