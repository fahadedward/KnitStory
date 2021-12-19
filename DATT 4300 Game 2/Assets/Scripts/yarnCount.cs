using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class yarnCount : MonoBehaviour
{ 
    // Start is called before the first frame update
    PlayerMovement pickup;
    public int yarnNum = 0;
    public Text yarn;

    void Awake()
    {
        pickup = gameObject.GetComponent<PlayerMovement>();
        yarn = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //yarn.text = "Yarn Count: " + pickup.KnittedItem;
    }
}
