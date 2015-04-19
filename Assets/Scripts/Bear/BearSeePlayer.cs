using UnityEngine;
using System.Collections;

public class BearSeePlayer : MonoBehaviour 
{
	public BearController Bear;

	public void OnTriggerStay2D(Collider2D other) 
    {
		var objTag = other.gameObject.tag;

		if (objTag == "Player") 
        {
			Bear.BecomeAlerted();
		}
	}
}