using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    PlayerMovement player;
    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetBool("isIdle", true);
    }
    // Update is called once per frame
    void Update()
    {
        if (player.Jumping)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isKnitting", false);

        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isIdle", true);
            animator.SetBool("isKnitting", false);

        }
        if (player.Moving)
        {
           // animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isKnitting", false);

        }
        else if (!player.Moving)
        {
          //  animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
            animator.SetBool("isKnitting", false);
        }
        if(player.playAnimation)
        {
            animator.SetBool("isKnitting", true);
            StartCoroutine(StoppingAnimation());
        }
        else if (!player.playAnimation)
        {
            animator.SetBool("isKnitting", false);
        }
    }
    IEnumerator StoppingAnimation()
    {
        Debug.Log("ENTERED HERE");
        yield return new WaitForSeconds(1f);
        animator.SetBool("isKnitting", false);
    }
}
