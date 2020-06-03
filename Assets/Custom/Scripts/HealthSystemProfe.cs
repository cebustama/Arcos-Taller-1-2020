using UnityEngine;
using System.Collections;

public class HealthSystemProfe : MonoBehaviour
{
    public int health = 3;

    public Color hitColor;

    public int score;

    [HideInInspector]
    public int maxHealth;

    // Will be set to 0 or 1 depending on how the GameObject is tagged
    // it's -1 if the object is not a player
    private int playerNumber;

    private void Start()
    {
        // Set the player number based on the GameObject tag
        switch (gameObject.tag)
        {
            case "Player":
                playerNumber = 0;
                break;
            case "Player2":
                playerNumber = 1;
                break;
            default:
                playerNumber = -1;
                break;
        }

        // Notify the UI so it will show the right initial amount

        maxHealth = health; //note down the maximum health to avoid going over it when the player gets healed
    }


    // changes the energy from the player
    // also notifies the UI (if present)
    public void ModifyHealth(int amount)
    {
        //avoid going over the maximum health by forcin
        if (health + amount > maxHealth)
        {
            amount = maxHealth - health;
        }

        health += amount;

        // Notify the UI so it will change the number in the corner

        //
        if (amount < 0)
        {
            if (GetComponent<SpriteRenderer>() != null)
            {
                GetComponent<SpriteRenderer>().color = hitColor;
                Invoke("BackToColor", 0.1f);
            }
        }

        //DEAD
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void BackToColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}