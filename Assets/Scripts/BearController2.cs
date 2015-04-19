using UnityEngine;
using System.Collections;

public class BearController2 : MonoBehaviour {

	// This scripts assumes that when the bear is facing left,
	// the x component of localScale is positive, and negative
	// when the bear is facing right
	public enum State { 
		PROWLING,
		ALERTED,
		PLAYER_ON_GROUND,
		PLAYER_IN_TREE,
		DEAD
	};
	public enum BearOrientation {
		FACE_LEFT,
		FACE_RIGHT,
		FLIP
	};
	public State activity;
	public const State PROWLING = State.PROWLING, 
					   ALERTED = State.ALERTED, 
					   PLAYER_ON_GROUND = State.PLAYER_ON_GROUND, 
					   PLAYER_IN_TREE = State.PLAYER_IN_TREE,
					   DEAD = State.DEAD;
	public PolygonCollider2D visionCone;
	public CircleCollider2D hearingRadius;
	public GameObject player, bearModel;
	public float patrolDistance;
	public float patrolTime;
	public bool patroling;
	public bool playerInCone;

	// Use this for initialization
	void Start () {
		activity = State.PROWLING;
		StartCoroutine(PatrolLeft());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Cancel")) {
			StopAllCoroutines();
			Kill ();
			return;
		}

		if (activity == DEAD) {
			patroling = false;
		}

	}

	IEnumerator PatrolLeft() {
		patroling = true;
		OrientBear (BearOrientation.FACE_LEFT);

		// Patrol to the left
		for (float xx = 0.0f; xx < patrolDistance && activity == PROWLING; xx += patrolDistance / patrolTime) {
			transform.position -= new Vector3( patrolDistance / patrolTime, 0, 0 );
			yield return null;
		}

		OrientBear (BearOrientation.FACE_RIGHT);

		// Patrol to the right
		for (float xx = 0.0f; xx < patrolDistance; xx += patrolDistance / patrolTime) {
			transform.position += new Vector3( patrolDistance / patrolTime, 0, 0 );
			yield return null;
		}

		patroling = false;
		FinishedPatroling ();
	}

	IEnumerator PatrolRight() {
		patroling = true;
		OrientBear (BearOrientation.FACE_RIGHT);

		// Patrol to the right
		for (float xx = 0.0f; xx < patrolDistance; xx += patrolDistance / patrolTime) {
			transform.position += new Vector3( patrolDistance / patrolTime, 0, 0 );
			yield return null;
		}
		
		OrientBear (BearOrientation.FACE_LEFT);

		// Patrol to the left
		for (float xx = 0.0f; xx < patrolDistance && activity == PROWLING; xx += patrolDistance / patrolTime) {
			transform.position -= new Vector3( patrolDistance / patrolTime, 0, 0 );
			yield return null;
		}
		
		patroling = false;
		FinishedPatroling ();
		
	}

	// Called at the end of PatrolLeft() and PatrolRight()
	public void FinishedPatroling () {
		Debug.Log ("Finished Patroling()");
		switch (activity) {
		case State.ALERTED:
			// Grab bear's orientation
			if ( player.transform.position.x >= transform.position.x ) {
				StartCoroutine(PatrolLeft ());
			}
			else {
				StartCoroutine (PatrolRight ());
			}

			break;
		case State.DEAD:
			// Don't do anything...
			break;
		default:
			activity = PROWLING; // If we were prowling before, keep prowling

			// Flip the bear's direction
			OrientBear (BearOrientation.FLIP);
			if ( GetBearOrientation () == BearOrientation.FACE_RIGHT ) {
				// Bear is facing right, so we patrol right
				StartCoroutine(PatrolRight ());
			}
			else
				StartCoroutine(PatrolLeft ());
			break;
		}
	}

	// Play death animation
	public void Kill() {
		if (activity != DEAD) {
			activity = DEAD;
			StartCoroutine (CoKill ());
		}
	}

	// Death animation
	private IEnumerator CoKill() {
		// Play death animation
		var scale = transform.localScale;
		transform.localScale = new Vector3 (Mathf.Abs (scale.x), -scale.y, scale.z);

		// Wait for a few seconds
		Debug.Log ("A Bear has been killed.");

		// Fade out & destroy

		Color tempColor = bearModel.GetComponent<Renderer>().material.color;

		for (int frames = 60; frames > 0; frames--) {

			float alpha = frames / 60.0f;
			tempColor = new Color (tempColor.r, tempColor.g, tempColor.b, alpha);
			bearModel.GetComponent<Renderer> ().material.color = tempColor;
			yield return null;

		}

		// Set alpha to zero explicitly
		tempColor = new Color (tempColor.r, tempColor.g, tempColor.b, 0.0f);
		bearModel.GetComponent<Renderer> ().material.color = tempColor;

		GetComponent<BoxCollider2D> ().enabled = false; // Disable box collider
		Destroy (this);
	}

	// We heard something, or saw something. Let's look in that direction and
	// search for the player
	public void AlertBear() {
		if (/*activity != ALERTED && */activity != DEAD) {
			Debug.Log ("[!] WARNING!!!");
			activity = ALERTED;

			// Follow the player's sound
			if ( player ) {

				// Stop any patroling coroutines we started
				StopAllCoroutines();
				patroling = false;

				// Grab bear's orientation
				BearOrientation facing = GetBearOrientation();
				FinishedPatroling(); // Given our new activity, let's decide what to do next
			}
		}
	}

	// Makes the bear flip, face left or face right
	public void OrientBear(BearOrientation o) {
		var scale = transform.localScale;
		if (o == BearOrientation.FACE_LEFT) {
			transform.localScale = new Vector3 (Mathf.Abs (scale.x), scale.y, scale.z);
		} else if (o == BearOrientation.FACE_RIGHT) {
			transform.localScale = new Vector3 (-Mathf.Abs (scale.x), scale.y, scale.z);
		} else if (o == BearOrientation.FLIP) {
			transform.localScale = new Vector3 (-(scale.x), scale.y, scale.z);
		}
	}

	public BearOrientation GetBearOrientation() {
		if (transform.localScale.x >= 0) { // Left by default, this is why we include the 0 scale
			return BearOrientation.FACE_LEFT;
		} else {
			return BearOrientation.FACE_RIGHT;
		}
	}
	
	public void SetSeeingPlayer(bool b) {
		playerInCone = b;
	}

	// If we've detected a player is in a tree, shake it
	public void ShakeTree(GameObject tree) {

	}

	// Fire a laser from this bear's eyes at a given target
	public virtual void FireLaser (GameObject target) {}

	// Use your laser to burn down a tree. CAN'T ESCAPE
	public virtual void VaporizeTree (GameObject target) {}
}