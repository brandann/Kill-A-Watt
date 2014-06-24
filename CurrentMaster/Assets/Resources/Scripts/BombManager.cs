using UnityEngine;
using System.Collections;

namespace Global{
    public class BombManager : MonoBehaviour {

        private GameObject bombPrefab1;
        private GameObject bombPrefab2;
        private float COUNTMAX = 10;
        private const float COOLDOWNMAX = 10;
        private float count = 15;
        private float cooldown = 0;
        private bool started = false;
        public ownerShip myOwner = ownerShip.Neutral;

        // Use this for initialization
        void Start () {
            AudioManager audioManager = GameObject.Find ("Main Camera").GetComponent<AudioManager> ();
            audioManager.playBomb();
            // load the bombparticle prefab
            bombPrefab1 = Resources.Load("Prefabs/BombParticlePlayer1") as GameObject;
            bombPrefab2 = Resources.Load("Prefabs/BombParticlePlayer2") as GameObject;
        }
        
        // Update is called once per frame
        void Update () {
            if (!started)
                return;
            // check duration of running particle effect
            if (count >= COUNTMAX){
                Destroy(this.gameObject);
            }
            //ceate particle
            makeBombPoint ();
            //count up particle duration
            count++;
        }

        private void makeBombPoint() {
            //instantiate and deploy particle
            GameObject e = null;
            if (myOwner == ownerShip.Player1) {
                e = Network.Instantiate (bombPrefab1, this.transform.position, Quaternion.LookRotation (Vector3.forward, Vector3.forward), 0) as GameObject;
            } else if(myOwner == ownerShip.Player2) {
                e = Network.Instantiate (bombPrefab2, this.transform.position, Quaternion.LookRotation (Vector3.forward, Vector3.forward), 0) as GameObject;
            }
        }

        public void changeOwner(ownerShip owner) {
            if (Network.isServer) {
                myOwner = owner;
                startBomb();
            } else {
                networkView.RPC ("RPCchangeOwner", RPCMode.Server, (int)owner);
            }
        }

        [RPC]
        public void RPCchangeOwner(int owner) {
            this.myOwner = (owner == 1) ? ownerShip.Player1 : ownerShip.Player2;
            startBomb ();
        }

        public void startBomb() {
            //reset values
            count = 0;
            started = true;
        }
    }
}