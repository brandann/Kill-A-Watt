using UnityEngine;
using System.Collections;

namespace Global
{
	public class unitBehavior : MonoBehaviour
	{
		AudioManager audioManager;
		private GameObject burstManagerPrefab;
		public Vector3 destination = new Vector3(0,0,0);
		
		public float speed;
		
		LineRenderer line;
		public Sprite play1;
		public Sprite play2;
		public ownerShip myOwner;
		
		// Use this for initialization
		void Start()
		{
			line = this.GetComponent<LineRenderer> ();
			audioManager = GameObject.Find ("Main Camera").GetComponent<AudioManager> ();
			burstManagerPrefab = Resources.Load("Prefabs/BurstManager") as GameObject;
		}
		
		// Update is called once per frame
		void Update()
		{   //todo consider checking to see of client and server are executing this function and moving the transform faster than otherwise
			transform.position += (speed * Time.smoothDeltaTime) * transform.up;
			lightning ();
		}
		
		
		// 
		void OnTriggerEnter2D(Collider2D other)
		{
			if (tag != other.gameObject.tag && other.gameObject.tag.Contains("Unit")) {
				audioManager.playMinionCollision();
				GameObject e = Network.Instantiate(burstManagerPrefab, this.transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.forward), 0) as GameObject;
				BurstManager BM = e.GetComponent<BurstManager>();
				if(BM != null) {
					e.transform.position = this.transform.position;
				}
			}
			if (Network.isServer)
			{
				if (tag != other.gameObject.tag && other.gameObject.tag.Contains("Unit")) {
					//audioManager.playMinionCollision();
					Network.Destroy(other.gameObject);
				}
			}           
		}
		
		private void lightning(){
			
			line.SetPosition (0, this.transform.localPosition);

			for (int i =1; i< 4; i++) {
				Vector3 pos = Vector3.Lerp (this.transform.localPosition, this.transform.localPosition  , i / 4.0f);

				float length = .75f;
				pos.x += Random.Range (-length, length);
				pos.y += Random.Range (-length, length);
				
				line.SetPosition (i, pos);
			}
			
			line.SetPosition (4, this.transform.localPosition);
			
		}
		
		public void makeBurst() {
			GameObject e = Network.Instantiate(burstManagerPrefab, this.transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.forward), 0) as GameObject;
			BurstManager BM = e.GetComponent<BurstManager>();
			if(BM != null) {
				e.transform.position = this.transform.position;
			}
		}
	}
	
	
	
}