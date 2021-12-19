using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleScript : MonoBehaviour
{
    private Camera playerCamera;
    private GameObject Player;

    public Vector3 hookshotPos;
    private float hookshotSize;
    private Ray ray;

    [SerializeField] private GameObject debugHitPointTransform;
    [SerializeField] private Transform hookShotTransform;

    PlayerMovement move;

    public State state;

    public enum State
    {
        Normal, HookshotThrown, HookshotFlyingPlayer
    }

    void Awake()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Player = GameObject.FindGameObjectWithTag("Player");
        move = gameObject.GetComponent<PlayerMovement>();

        state = State.Normal;
        hookShotTransform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(move.KnittedItem);
        switch (state)
        {
            default:
            case State.Normal:
                if (move.KnittedItem > 0)
                {
                    HandleHookshotStart();
                }
                break;
            case State.HookshotThrown:
                HandleHookshotThrow();
                break;
            case State.HookshotFlyingPlayer:
                HandleHookshotMovement();
                break;
        }

    }


    private void HandleHookshotStart()
    {
        Vector3 m_DistanceFromCamera = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, -0.008003235f);
        Plane m_Plane = new Plane(Vector3.forward, m_DistanceFromCamera);
        float enter = 0.0f;

        //move.KnittedItem--;

        ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (m_Plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                if (hitPoint.y > 1)
                {
                    debugHitPointTransform.transform.position = hitPoint;
                }
                else
                {
                    debugHitPointTransform.transform.position = new Vector3(hitPoint.x, 1.08f, hitPoint.z);
                }
                hookshotPos = debugHitPointTransform.transform.position;
                hookshotSize = 0f;
                hookShotTransform.gameObject.SetActive(true);
                //hookShotTransform.localScale = Vector3.zero;
                move.KnittedItem--;
                state = State.HookshotThrown;
            }
        }
    }

    private void HandleHookshotThrow()
    {
        float hookshotSpeed = .9f;

        hookShotTransform.LookAt(hookshotPos);
        if (hookshotPos.x >= Player.transform.position.x)
        {
            move.inputVector.x = .001f;
            Player.transform.rotation = Quaternion.Euler(0, 0, 0);
            hookShotTransform.rotation *= Quaternion.Euler(0, -90, 0);
        }
        else if (hookshotPos.x < Player.transform.position.x)
        {
            move.inputVector.x = -.001f;
            Player.transform.rotation = Quaternion.Euler(0, 180, 0);
            hookShotTransform.rotation *= Quaternion.Euler(0, -90, 0);
        }

        hookshotSize += hookshotSpeed * Time.deltaTime;
        hookShotTransform.localScale = new Vector3(hookshotSize * 17.5f, 1f, 1f);
        if (hookshotSize >= Vector3.Distance(Player.transform.position, hookshotPos))
        {
            state = State.HookshotFlyingPlayer;
        }
    }

    private void HandleHookshotMovement()
    {
        Vector3 hookshotDir = (hookshotPos - Player.transform.position).normalized;

        float hookSpeedMin = .9f;
        float hookSpeedMax = 2f;

        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(Player.transform.position, hookshotPos), hookSpeedMin, hookSpeedMax);
        float hookshotSpeedMultiplier = 1.5f;
        float reachedHookshotPositionDistance = .05f;

        move.controller.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);

        if (hookshotSize > 0)
        {
            hookshotSize -= hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime;
            hookShotTransform.localScale = new Vector3(hookshotSize * 17.5f, 1f, 1f);
        }
        else if (hookshotSize < 0 && Input.GetMouseButton(0) == false)
        {
            state = State.Normal;
            move.movement.y = 0;
            hookShotTransform.gameObject.SetActive(false);
        }
        else if (((Vector3.Distance(Player.transform.position, hookshotPos) < reachedHookshotPositionDistance) && Input.GetMouseButton(0) == false))
        {
            state = State.Normal;
            Debug.Log(state);
            move.movement.y = 0;
            hookShotTransform.gameObject.SetActive(false);
        }

        if (Input.GetButton("Jump"))
        {
            move.movement.y = move.jumpSpeed;
            move.jump = false;
            hookShotTransform.gameObject.SetActive(false);
        }
    }
}