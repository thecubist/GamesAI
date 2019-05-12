using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    enum State
    {
        idle = 0,
        wandering = 1,
        chase = 2,
        attack = 3,
        pathfinding = 4,
        dead = 5,
    };

    private State currentState = State.wandering;
    private GameObject playerRef;
    private float distanceToPlayer = 0;
    private float health = 100;
    private bool hasGivenKill;

    [Header("Basic Variables")]
    public float detectionRange = 10;
    public float attackRange = 5;
    public Transform shootPosition;
    public float moveSpeed = 0.07f;
    public float wanderSpeedMult = 30;
    public float fireRate = 2;
    public GameObject bullet;
    public float boundingCylinderSize = 1;
    public float bulletSpeed = 6.0f;
    [Header("UI Variables")]
    public Text stateTextUI;
    public Text distanceTextUI;
    public Text healthTextUI;

    // Use this for initialization
	void Start ()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");

        if (playerRef == null)
            Debug.Log("Player was not found");
	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit ray;
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        checkForBulletCollisions();

        if (playerRef != null)
        {
            checkForStateChange(); //check if the current state should be changed
            updateUI(); //updating the UI 

            if (currentState == State.idle)
            {
                //Wait(2);
            }
            else if (currentState == State.wandering)
            {
                Vector3 randRot = new Vector3(0,0,0);
                int moveLoopCount;

                if (Physics.Raycast(transform.position, forward, out ray))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * ray.distance, Color.green);
                    moveLoopCount = UnityEngine.Random.Range(1000, 10000);

                    if (ray.distance > 2.0f)
                    {
                        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed * wanderSpeedMult;
                    }
                    else
                    {
                        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                        randRot.y = UnityEngine.Random.Range(10f, 360f);
                        transform.eulerAngles = randRot;
                    }
                }
            }
            else if (currentState == State.chase)
            {
                Physics.Raycast(transform.position, forward, out ray);
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * ray.distance, Color.blue);

                moveTowardsPlayer(true);
            }
            else if (currentState == State.attack)
            {
                Physics.Raycast(transform.position, forward, out ray);
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * ray.distance, Color.magenta);

                moveTowardsPlayer(false);
                Shoot();
            }
            else if (currentState == State.pathfinding)
            {

            }
            else if (currentState == State.dead)
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

                if (!hasGivenKill)
                {
                    playerRef.GetComponent<PlayerController>().addKill();
                    hasGivenKill = true;
                }

                Destroy(gameObject, 5.0f);
            }
        }
    }

    private void checkForStateChange()
    {
        distanceToPlayer = dotProduct(playerRef.transform.position, gameObject.transform.position, true);

        if (health > 0)
        {
            if (distanceToPlayer < detectionRange) //detect if player is close enough for state overrides
            {
                if (distanceToPlayer < attackRange)
                {
                    currentState = State.attack;
                }
                else
                {
                    currentState = State.chase;
                }
                return;
            }
            else
            {
                currentState = State.wandering;
            }
        }
        else
        {
            currentState = State.dead;
        }
    }

    private void updateUI()
    {
        if (stateTextUI != null)
        {
            stateTextUI.text = "State: " + currentState.ToString();
        }

        if (distanceTextUI != null)
        {
            distanceTextUI.text = "Player distance: " + distanceToPlayer;
        }

        if (healthTextUI != null)
        {
            healthTextUI.text = "Health: " + health;
        }
    }

    private void moveTowardsPlayer(bool move)
    {
        gameObject.transform.LookAt(playerRef.transform);

        if(move)
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, playerRef.transform.position, moveSpeed);
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

    private void checkForBulletCollisions()
    {
        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("BulletPlayer");

        for (int i = 0; i < playerBullets.Length; i++)
        {
            if (dotProduct(playerBullets[i].transform.position, gameObject.transform.position, false) < boundingCylinderSize)
            {
                Destroy(playerBullets[i]);
                health -= 10;
            }
        }
    }

    void Shoot()
    {
        //Transform aimLocation = shootPosition.transform.LookAt(playerRef.transform);
        //instance the object locally
        //Quaternion shootAngle = 
        //shootPosition.transform.LookAt(playerRef.transform.position);
        //shootPosition.transform.eulerAngles = new Vector3(shootPosition.transform.eulerAngles.x, 0, 0);
        GameObject bulletInst = (GameObject)Instantiate(bullet, shootPosition.position, shootPosition.rotation);
        
        //move the bullet
        bulletInst.GetComponent<Rigidbody>().velocity = bulletInst.transform.up * bulletSpeed;

        //destroy the bullet 
        Destroy(bulletInst, 2);
    }

    /**
     * prevents code running until the started amount of time passes
     */
    void Wait(int seconds)
    {
        System.DateTime unpauseTime = System.DateTime.Now.AddSeconds(seconds);
        while (System.DateTime.Now < unpauseTime)
        {
            Debug.Log("waiting");
        }
    }
}
