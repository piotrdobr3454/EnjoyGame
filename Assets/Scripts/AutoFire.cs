﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFire : MonoBehaviour
{
    public float bulletForce = 20f;
    public float fpRadius = 0.7f;

    public Transform firePoint;
    public GameObject projectilePrefab;
    public Rigidbody2D rb;
    public Camera cam;

    Vector2 mousePos;

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x);
        firePoint.position = new Vector2(fpRadius * Mathf.Cos(angle) + rb.position.x,
                                         fpRadius * Mathf.Sin(angle) + rb.position.y);
        firePoint.transform.eulerAngles = new Vector3(firePoint.transform.eulerAngles.x,
                                                      firePoint.transform.eulerAngles.y,
                                                      angle * Mathf.Rad2Deg);
    }

    public void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }
}
