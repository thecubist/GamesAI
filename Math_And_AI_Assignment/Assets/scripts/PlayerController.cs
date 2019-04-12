using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpawnPosition;
    public float bulletSpeed = 6.0f;

    Vector3 rotation = new Vector3(0,0,0);

    void Update()
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
}

