﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon")) return;
        if (collision.CompareTag("Bullet")) return;

        Debug.Log(collision.gameObject.name);

        Destroy(gameObject);
    }
}
