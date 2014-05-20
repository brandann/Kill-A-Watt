using UnityEngine;
using System.Collections;

namespace Global
{
	public class unitBehavior : MonoBehaviour
	{
		AudioManager audioManager;

		private float speed = 2f;
		
		
		public Sprite play1;
		public Sprite play2;
		public ownerShip myOwner;
		
		// Use this for initialization
		void Start()
		{
			audioManager = GameObject.Find ("Main Camera").GetComponent<AudioManager> ();
		}
		
		// Update is called once per frame
		void Update()
		{   //todo consider checking to see of client and server are executing this function and moving the transform faster than otherwise
			transform.position += (speed * Time.smoothDeltaTime) * transform.up;
		}
		
		
		// 
		void OnTriggerEnter2D(Collider2D other)
		{
			if (tag != other.gameObject.tag && other.gameObject.tag.Contains("Unit")) {
				audioManager.playMinionCollision();
			}
			if (Network.isServer)
			{
				if (tag != other.gameObject.tag && other.gameObject.tag.Contains("Unit")) {
					//audioManager.playMinionCollision();
					Network.Destroy(other.gameObject);
				}
			}           
		}
	}
	
}