using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
		[SerializeField] private float m_MaxSpeed = 10f; // The fastest the player can travel in the x axis.
		[SerializeField] private float m_MaxClimbSpeed = 3f;
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
		const float gravity = 3f; // Constant for the amount of gravity acting on the player
		private bool isClimbing = false; // State for climbing
		private Transform tree;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
				{
                    m_Grounded = true;
					isClimbing = false;
					m_Rigidbody2D.gravityScale = gravity;
					//m_Anim.SetBool("Climbing", false);
				}
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }


        public void Move(float moveX, float moveY, bool jump, bool canClimb)
        {
			if (canClimb && Mathf.Abs (moveY) > Mathf.Abs (moveX))
			{
				isClimbing = true;
				m_Rigidbody2D.gravityScale = 0;
				m_Rigidbody2D.position = new Vector2 (tree.position.x, m_Rigidbody2D.position.y);
				//m_Anim.SetBool("Climbing", true);
			}

			if (isClimbing)
			{
				if (jump || !canClimb)
				{
					isClimbing = false;
					m_Rigidbody2D.gravityScale = gravity;
					//m_Anim.SetBool("Climbing", false);
				}
				else
				{
					m_Rigidbody2D.velocity = new Vector2 (0, moveY * m_MaxClimbSpeed);
				}
			}

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl && !isClimbing) {
				// The Speed animator parameter is set to the absolute value of the horizontal input.
				m_Anim.SetFloat ("Speed", Mathf.Abs (moveX));

				// Move the character
				m_Rigidbody2D.velocity = new Vector2 (moveX * m_MaxSpeed, m_Rigidbody2D.velocity.y);

				// If the input is moving the player right and the player is facing left...
				if (moveX > 0 && !m_FacingRight) {
					// ... flip the player.
					Flip ();
				}
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (moveX < 0 && m_FacingRight) {
					// ... flip the player.
					Flip ();
				}
			}

            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

		public void SetTreePosition(Transform newTree)
		{
			// Set the tree that the player can climb up
			// This is used in User Control
			tree = newTree;
			Debug.Log (tree.position);
		}
    }
}
