using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    private float scaleNum = 18.687f;
    // Update is called once per frame
    void Update()
    {
          Vector3 displacement = transform.position - player.transform.position;
          displacement.y = 0;
          float range = displacement.magnitude;
          if(range <= 3f && player.transform.position.y >= transform.position.y)
            {
             if (player.transform.position.x < transform.position.x)
             {
                transform.Translate(displacement * -0.8f * Time.deltaTime);
                transform.localScale = new Vector3(-scaleNum, scaleNum, scaleNum); 
            }
            else if(player.transform.position.x > transform.position.x)
            {
                transform.Translate(displacement * -0.8f * Time.deltaTime);
                transform.localScale = new Vector3(scaleNum, scaleNum, scaleNum);

            }
        }
    }
}
