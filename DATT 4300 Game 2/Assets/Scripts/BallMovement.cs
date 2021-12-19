using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
   
   
}

/* 
 *  PlayerMovement playerMovement;
    Rigidbody rb;
 * void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
            playerMovement.ballCount = 0;
            playerMovement.BallIsMade = false;
            playerMovement.Throwing = false;
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (playerMovement.ballCount == 0)
        {
            transform.position = playerMovement.Destination.transform.position;
        }
        if (playerMovement.ballCount == 1 && playerMovement.ShootingRight)
        {
            rb.velocity = playerMovement.Destination.right * 2;
            StartCoroutine(DestroyBall());
        }
        if (playerMovement.ballCount == 1 && playerMovement.ShootingLeft)
        {
            rb.velocity = -playerMovement.Destination.right * 2;
            StartCoroutine(DestroyBall());
        } 
        if(playerMovement.Throwing)
        {
            StartCoroutine(DestroyBall());
        }
    }
   
    IEnumerator DestroyBall()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
        playerMovement.ballCount = 0;
        playerMovement.BallIsMade = false;
        playerMovement.Throwing = false;
    }
*/
