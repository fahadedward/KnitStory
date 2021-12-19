using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    Vector3 offset;

    void Awake()
    {
        offset = new Vector3(0, 0.1f, -0.8f);
    }
    void FixedUpdate()
    {
        Vector3 camPos = player.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, camPos, 5 * Time.deltaTime);
        transform.position = smoothPos;
    }
}
