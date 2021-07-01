using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public float lerpRate = .125f;
    public bool rewind;
    public bool lockMovement;

    private void FixedUpdate()
    {
        // Follower player if player is set
        if (player == null)
        {
            return;
        }

        if (rewind || lockMovement)
        {
            return;
        }

        Vector3 position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, position, lerpRate);
    }

    private void LateUpdate()
    {
        // Follower player if player is set
        if (player == null)
        {
            return;
        }

        if (!rewind && !lockMovement)
        {
            return;
        }

        Vector3 position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

        if (lockMovement)
        {
            // Offset Y
            position = new Vector3(position.x, position.y + Random.Range(0, 1f), position.z);
            transform.position = Vector3.Lerp(transform.position, position, lerpRate);
        }
        else
        {
            transform.position = position;
        }
    }
}
