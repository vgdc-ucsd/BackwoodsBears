using UnityEngine;
using System.Collections;

public class BearChaseBounds : MonoBehaviour 
{
    public BearController Bear;

    void OnTriggerExit2D(Collider2D other)
    {
        var objTag = other.gameObject.tag;

        if (objTag == "Player")
        {
            Bear.BecomeCautious();
        }
    }
}