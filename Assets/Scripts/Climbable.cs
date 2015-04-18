using UnityEngine;
using System.Collections;

// Attached to a trigger collider to set the player's CanClimb to work.
public class Climbable : MonoBehaviour 
{
    public void OnTriggerEnter2D(Collider2D other)
    { 
        var player = other.GetComponent<Platformer2DUserControl>();

        if (player == null)
        {
            return;
        }

        player.CanClimb();
    }
}