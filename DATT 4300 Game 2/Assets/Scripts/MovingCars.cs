using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCars : MonoBehaviour
{
    [SerializeField]
    Transform startPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z <= -7f)
        {
            gameObject.SetActive(false);
            gameObject.transform.position = startPoint.position;
            gameObject.SetActive(true);
        }
        else if(transform.position.x <= 20f)
        {
            transform.Translate(new Vector3(0, 0, 7.5f * Time.deltaTime));
        }
    }
}
