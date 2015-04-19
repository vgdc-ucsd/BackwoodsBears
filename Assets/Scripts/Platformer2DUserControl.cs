using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
		private PlatformerCharacter2D m_Character;
        private bool m_Jump, m_Chop, canClimb;
		private float chopStart;
		private bool chopping;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
			canClimb = false;
        }


        private void Update()
        {
			if (!m_Chop)
			{
				m_Chop = CrossPlatformInputManager.GetButtonDown("Fire1");

				if (m_Chop)
				{
					chopping = true;
					chopStart = Time.time;
					m_Character.Chop ();
				}
			}

            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }

        private void FixedUpdate()
        {
			if (chopping)
			{
				if (Time.time - chopStart > m_Character.m_ChopDuration)
				{
					m_Chop = false;
					chopping = false;

					if (!m_Character.IsClimbing())
						canClimb = false;
				}
			}
			else
			{
				float moveX = CrossPlatformInputManager.GetAxis ("Horizontal");
				float moveY = CrossPlatformInputManager.GetAxis ("Vertical");
            
				m_Character.Move (moveX, moveY, m_Jump, canClimb);

				m_Jump = false;
			}
        }

		public void OnTriggerEnter2D(Collider2D other)
		{ 
			var tree = other.GetComponent<Climbable>();
			
			if (tree != null)
			{
				canClimb = true;
				m_Character.SetTreePosition(other.transform);
			}
		}

		public void OnTriggerExit2D(Collider2D other)
		{
			var tree = other.GetComponent<Climbable>();

			if (tree != null)
			{
				canClimb = false;
			}
		}
    }
}
