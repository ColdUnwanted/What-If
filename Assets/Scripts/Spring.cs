using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public Animator animator;
    float pushForce = 10;

    private void Start()
    {
        animator.Play("Spring", -1, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        Player player = collision.GetComponent<Player>();

        if (player.rewinding)
        {
            return;
        }

        animator.Play("Spring", -1, 0f);
        player.Bounce(pushForce);
    }
}
