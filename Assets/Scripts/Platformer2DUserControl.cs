using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump, canClimb;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
			canClimb = false;
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }

        private void FixedUpdate()
        {
            float moveX = CrossPlatformInputManager.GetAxis("Horizontal");
			float moveY = CrossPlatformInputManager.GetAxis("Vertical");
            
			m_Character.Move(moveX, moveY, m_Jump, canClimb);
            m_Jump = false;
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

		public bool CanClimb()
		{
			return false;
		}
    }
}
