using UnityEngine;
using System.Collections;

public class BearHearingRadius : MonoBehaviour 
{
	public BearController Bear;

	public void OnTriggerStay2D(Collider2D other) 
    {
		string otherTag = other.gameObject.tag;

		if (otherTag == "Player") 
        {
			if ( Input.anyKey ) 
            {
                Bear.HeardPlayer(other.transform.position.x);
			}
		}
	}
}
