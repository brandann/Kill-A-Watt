using UnityEngine;
using System.Collections;

namespace Global
{
    public class unitBehavior : MonoBehaviour
    {
        public bool rogue;
        AudioManager audioManager;
        private GameObject burstManagerPrefab;
        private GameObject bombManagerPrefab;
        public Vector3 destination = new Vector3(0,0,0);
        public float speed;
        LineRenderer line;
        public ownerShip myOwner;
        private float spawnedTime = 0;
        private float timeToLive = 15;
        
        // Use this for initialization
        void Start()
        {
            line = this.GetComponent<LineRenderer> ();
            audioManager = GameObject.Find ("Main Camera").GetComponent<AudioManager> ();
            burstManagerPrefab = Resources.Load("Prefabs/BurstManager") as GameObject;
            bombManagerPrefab = Resources.Load ("Prefabs/BombManager") as GameObject;
            spawnedTime = Time.time;
        }
		
        // Update is called once per frame
        void Update()
        {   
            //todo consider checking to see of client and server are executing this function and moving the transform faster than otherwise
            transform.position += (Time.smoothDeltaTime * speed  * transform.up);
            lightning ();
                if (Time.time > spawnedTime + timeToLive)
                    Destroy(this.gameObject);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (tag != other.gameObject.tag && other.gameObject.tag.Contains ("Unit")) {
                audioManager.playMinionCollision ();
                GameObject e = Network.Instantiate (burstManagerPrefab, this.transform.position, Quaternion.LookRotation (Vector3.forward, Vector3.forward), 0) as GameObject;
                BurstManager BM = e.GetComponent<BurstManager> ();
                if (BM != null) {
                    if(rogue){
                    BM.isRogue = true;
                    }
                    e.transform.position = this.transform.position;
                }
            } else if (tag != other.gameObject.tag && 
                       other.gameObject.tag.Contains ("Bomb") && 
                       other.gameObject.GetComponent<BombParticleBehavior>().myOwner != myOwner) {
                Debug.Log("Unit belongs to: " + myOwner  + " killed by " + other.gameObject.GetComponent<BombParticleBehavior>().myOwner);
                if (Network.isServer) {
                  makeBurst();
                  makeBomb(other.gameObject.GetComponent<BombParticleBehavior>().myOwner);
                  Network.Destroy (other.gameObject);
                  Network.Destroy(this.gameObject);
                  return;
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

        private void makeBomb(ownerShip owner) {
            //recursion
            GameObject e = Network.Instantiate(bombManagerPrefab, this.transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.forward), 0) as GameObject;
            BombManager BM = e.GetComponent<BombManager>();
            if(BM != null) {
                BM.changeOwner(owner);
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
            if (BM != null) {
                if (rogue) {
                    BM.isRogue = true;
                }
                e.transform.position = this.transform.position;
            }
        }
    }
}