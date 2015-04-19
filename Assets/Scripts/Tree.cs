using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour
{
	private int hp = 3;
	Transform m_transform;

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

	public void Chop()
	{

	}
}
