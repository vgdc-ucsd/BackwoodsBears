using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour
{
	public Transform root;
	public int hp = 3;
	private Transform m_transform;
	private float elapsedTime;
	private bool falling;
	private bool direction;
	private float duration = 4;
	private float prevRotation = 0;
	private float currentRotation = 0;
	private bool alive = true;

	// Use this for initialization
	void Start ()
	{
		m_transform = GetComponent<Transform>();
		m_transform.Rotate (0, Random.Range(0, 360), 0);
		falling = false;
		elapsedTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (falling)
			Fall (direction);
	}

	public void Chop(Transform other)
	{
		hp--;

		if (hp <= 0 && alive)
		{
			// True if falling right, false if falling left
			direction = other.position.x - transform.position.x < 0;
			falling = true;
			elapsedTime = 0;
			currentRotation = 1;
			BoxCollider2D[] myColliders = gameObject.GetComponents<BoxCollider2D>();
			Debug.Log(myColliders.Length);
			foreach(BoxCollider2D bc in myColliders)
			{
				bc.enabled = false;
			}
			alive = false;
		}
	}

	private void Fall(bool fallRight)
	{
		elapsedTime += Time.deltaTime;

		float percentElapsed = elapsedTime / duration;

		currentRotation = 90 * percentElapsed * percentElapsed * percentElapsed;

		if (currentRotation > 90)
		{
			currentRotation = 90;
			falling = false;
			Debug.Log("I'm done");
		}

		if (fallRight)
		{
			currentRotation = -currentRotation;
		}

		root.Rotate(new Vector3(0,0, currentRotation - prevRotation), Space.World);
		prevRotation = currentRotation;
	}

	public bool isAlive()
	{
		return hp > 0;
	}
}
