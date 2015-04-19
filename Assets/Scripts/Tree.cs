using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour
{
	public Transform root;
	public int hp = 3;
	private Transform m_transform;

	// Use this for initialization
	void Start ()
	{
		m_transform = GetComponent<Transform>();
		m_transform.Rotate (0, Random.Range(0, 360), 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Chop(Transform other)
	{
		hp--;

		if (hp <= 0)
		{
			bool fallRight = other.position.x - transform.position.x < 0;
			Fall (fallRight);
		}
	}

	private void Fall(bool fallRight)
	{

	}
}
