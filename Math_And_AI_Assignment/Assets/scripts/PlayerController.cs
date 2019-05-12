using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpawnPosition;
    public float bulletSpeed = 6.0f;
    private float health = 100;
    private int kills = 0;
    private int deaths = 0;
    public float boundingCylinderSize = 1;

    public Text healthText;
    public Text killsText;
    public Text deathsText;

    Vector3 rotation = new Vector3(0,0,0);

    void Update()
    {
        inputControls();
        updateUI();
        checkForBulletCollisions();
    }

    void updateUI()
    {
        healthText.text = "Health: " + health;
        killsText.text = "Kills: " + kills;
        deathsText.text = "Deaths: " + deaths;
    }

    void inputControls()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        rotation.y = x;
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //instance the object locally
        GameObject bulletInst = (GameObject)Instantiate(bullet, bulletSpawnPosition.position, bulletSpawnPosition.rotation);

        //move the bullet
        bulletInst.GetComponent<Rigidbody>().velocity = bulletInst.transform.up * bulletSpeed;

        //destroy the bullet 
        Destroy(bulletInst, 2);
    }

    private void checkForBulletCollisions()
    {
        GameObject[] npcBullets = GameObject.FindGameObjectsWithTag("BulletNPC");

        for (int i = 0; i < npcBullets.Length; i++)
        {
            if (dotProduct(npcBullets[i].transform.position, gameObject.transform.position, false) < boundingCylinderSize)
            {
                Destroy(npcBullets[i]);
                health -= 10;
            }
        }
    }

    /**
     * returns the dot product (distance) between 2 vectors that are passed to it
     */
    float dotProduct(Vector3 vec1, Vector3 vec2, bool useYAxis)
    {
        if (useYAxis)
        {
            return (float)Math.Sqrt((vec1 - vec2).magnitude);
        }
        else
        {
            Vector2 vec1_2d = new Vector2(vec1.x, vec1.z);
            Vector2 vec2_2d = new Vector2(vec2.x, vec2.z);

            return (float)Math.Sqrt((vec1_2d - vec2_2d).magnitude);
        }
    }

    public void addKill()
    {
        kills++;
    }
}

