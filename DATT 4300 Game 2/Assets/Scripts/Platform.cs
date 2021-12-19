using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    PlayerMovement playerMovement;
    Rigidbody rb;
    BoxCollider boxCollider;

    [SerializeField]
    float xNum = 0.25f;
    [SerializeField]
    float yNum = 0.2f;

    //[SerializeField]
   // GameObject pickUp;
    void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }

    private void Start()
    {
        //pickUp.SetActive(true);

        /*   if (playerMovement.facingRight)
           {
               transform.position = new Vector3(transform.position.x + 10, transform.position.y + yNum, 0);
           }
           if (playerMovement.facingLeft)
           {
               Debug.Log("WE ENTERED HERE");
               transform.position = new Vector3(transform.position.x - 10, transform.position.y, 0);
             //  transform.Rotate(-45f, -90f, 0);
           }*/

        transform.position = new Vector3(transform.position.x + xNum, transform.position.y + yNum, 0);
        transform.Rotate(45f, 90f, 0);
        if (playerMovement.facingLeft)
        {
            transform.position = new Vector3(playerMovement.Destination.position.x - xNum, transform.position.y, 0f);
            transform.rotation = Quaternion.Euler(-45f, 90f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  Debug.Log(playerMovement.PickUpAgain);

        if (playerMovement.PlatformIsMade)
        {
            this.transform.parent = (playerMovement.Destination.transform);
            rb.useGravity = false;
            boxCollider.enabled = false;
            playerMovement.PickUpAgain = false;
        }
        if (playerMovement.CanPlaceDown)
        {
            this.transform.parent = null;
            rb.useGravity = true;
            boxCollider.enabled = true;
            StartCoroutine(DisableGravity());
        }
        if (playerMovement.PickUpAgain)
        {
            boxCollider.isTrigger = true;
        }
    }  

    IEnumerator DisableGravity()
    {
        yield return new WaitForSeconds(0.5f);
        rb.isKinematic = true;
        playerMovement.CanPlaceDown = false;
        this.transform.parent = null;
        SphereCollider sphere = GetComponent<SphereCollider>();
        sphere.enabled = true;
        GetComponent<Platform>().enabled = false;
       
    }

    public void PlatformIsMade()
    {
              
    }
}

