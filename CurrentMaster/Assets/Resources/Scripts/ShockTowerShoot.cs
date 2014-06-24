using UnityEngine;
using System.Collections;

namespace Global{

    public class ShockTowerShoot : MonoBehaviour {

        // Use this for initialization
        float lastShot;
        private float fequency = .75f; // seconds a minion is killed
        private LineRenderer line;
        private bool Collided;
        public ownerShip parentsOwner;
        private float towerRange;
        private GameObject current;
        AudioManager audioManager;
        
        void Start () {
            lastShot = Time.realtimeSinceStartup;
            line = this.GetComponent<LineRenderer>();
            towerRange = 10;
            audioManager = GameObject.Find ("Main Camera").GetComponent<AudioManager> ();
        }
        
        void Update () {
            if ((Time.realtimeSinceStartup - lastShot) > 1)
                lightningSelf ();
            getMinions ();
        }

        private void getMinions(){
            if (parentsOwner == ownerShip.Player1) {
                GameObject[] P2Array = GameObject.FindGameObjectsWithTag ("Player2Unit");
                destroyMinions(P2Array);
            }
            if (parentsOwner == ownerShip.Player2) {
                GameObject[] P1Array = GameObject.FindGameObjectsWithTag ("Player1Unit");
                destroyMinions(P1Array);
            }
        }
	
        private void destroyMinions(GameObject[] minionArray){
            Vector3 from = this.transform.position;
            foreach (GameObject minion in minionArray) {
                if(current == null){
                  current = minion;
                }

                Vector3 to = minion.transform.position;
                float dist = Vector3.Magnitude(from - to);
                if(Mathf.Abs(dist) < towerRange){
                    lightning (current.transform);
                    if (Time.realtimeSinceStartup - lastShot > fequency){
                        lastShot = Time.realtimeSinceStartup;
                        if (Network.isServer){
                            current.GetComponent<unitBehavior>().makeBurst();
                            audioManager.playShockTower();
                            Network.Destroy (current);
                        }
                    }
                }else{
                    current = null;
                }
            }
        }

        private void lightning(Transform minion){
            if (parentsOwner == ownerShip.Player1) 
                line.SetColors (Color.yellow, Color.white);
            else if(parentsOwner == ownerShip.Player2)
                line.SetColors (Color.blue, Color.white);
                
            line.SetPosition (0, this.transform.localPosition);
            for (int i =1; i< 4; i++) {
                Vector3 pos = Vector3.Lerp (this.transform.localPosition, minion.localPosition, i / 4.0f);
                pos.x += Random.Range (-0.8f, 0.8f);
                pos.y += Random.Range (-0.8f, 0.8f);
                line.SetPosition (i, pos);
            }
            line.SetPosition (4, minion.localPosition);
        }

        private void lightningSelf(){
            if (parentsOwner == ownerShip.Player1) 
                line.SetColors (Color.yellow, Color.white);
            else if(parentsOwner == ownerShip.Player2)
                line.SetColors (Color.blue, Color.white);
                
            Vector3 updatedVector = new Vector3(this.transform.position.x,this.transform.position.y + 1.5f, 0);
            line.SetPosition (0, updatedVector);
            for (int i =1; i< 4; i++) {
                Vector3 pos = Vector3.Lerp (updatedVector, updatedVector, i / 4.0f);
                pos.x += Random.Range (-0.8f, 0.8f);
                pos.y += Random.Range (-0.8f, 0.8f);
                line.SetPosition (i, pos);
            }
            line.SetPosition (4, updatedVector);
        }
    }
}