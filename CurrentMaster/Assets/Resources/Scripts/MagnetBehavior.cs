using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Global
{
    public class MagnetBehavior : MonoBehaviour
    {
        private GameObject p1Unit;
        private GameObject p2Unit;
        private GameObject rogueUnit;
        private List<GameObject> alreadyCollidedWith;
        public Collider2D target;
        int numPoints = 15;
        float radius = 2.8f;
        private Vector3[] points = new Vector3[15];
        public float secondsToStick = 10;
        private float startTime;
        LineRenderer line;

        // Use this for initialization
        void Start()
        {
            p1Unit = Resources.Load("Prefabs/Player1Unit") as GameObject;
            p2Unit = Resources.Load("Prefabs/Player2Unit") as GameObject;
            rogueUnit = Resources.Load("Prefabs/RogueUnit") as GameObject;
            alreadyCollidedWith = new List<GameObject>();
            line = this.GetComponent<LineRenderer> ();
            AudioManager audioManager = GameObject.Find ("Main Camera").GetComponent<AudioManager> ();
            audioManager.playMagnet ();
        }

        private void lightningSelf(){
            Vector3 updatedVector = new Vector3(this.transform.position.x,this.transform.position.y, 0);
            line.SetPosition (0, updatedVector);
            for (int i =1; i< 4; i++) {
                Vector3 pos = Vector3.Lerp (updatedVector, updatedVector, i / 4.0f);
                pos.x += Random.Range (-0.8f, 0.8f);
                pos.y += Random.Range (-0.8f, 0.8f);
                line.SetPosition (i, pos);
            }
            line.SetPosition (4, updatedVector);
        }

        void Update(){
            lightningSelf ();
        }

        void OnTriggerEnter2D(Collider2D otherCollider) {
            print("magnet hit stuff");
            if (alreadyCollidedWith.Contains(otherCollider.gameObject))
                return;

            string otherTag = otherCollider.gameObject.tag;

            //Ignore collisions with anything other than units or towers
            if (!otherTag.Contains("Unit") && !otherTag.Contains("Tower")) {
                return;
            }
            else {
                if (otherTag.Contains("Unit")) {
                    unitBehavior otherBehavior = otherCollider.gameObject.GetComponent<unitBehavior>();
                    GameObject unitToLoad = rogueUnit;
                    Vector3 loadAt = otherBehavior.gameObject.transform.position;
                    Vector3 dest = unitToLoad.GetComponent<unitBehavior>().destination = Vector3.zero;
                    Vector3 dir = otherBehavior.gameObject.transform.rotation.eulerAngles;
                    dir = new Vector3(dir.x, dir.y, dir.z + 180);
                    Network.Destroy(otherCollider.gameObject);
                    GameObject flippedUnit =
                        (GameObject)Network.Instantiate(
                                             unitToLoad, loadAt, Quaternion.Euler(dir), 0);
                    alreadyCollidedWith.Add(flippedUnit);
                }
                else if (otherCollider == target) { //Must be tower
                    gameObject.rigidbody2D.isKinematic = true;
                    gameObject.collider2D.enabled = false;
                    startTime = Time.time;
                    StartCoroutine(interfereWithTower(otherCollider.gameObject.GetComponent<Tower>()));
                }
            }
        }

        private IEnumerator interfereWithTower(Tower t) {
            if (t == null) {
                Debug.LogError("Null tower");
                yield return 0;
            }

            t.Magnetize();
            int pointNum = 0;
            for ( ; pointNum < numPoints; pointNum++) {
                // "i" now represents the progress around the circle from 0-1
                // we multiply by 1.0 to ensure we get a fraction as a result.
                double i = (pointNum * 1.0) / numPoints;

                // get the angle for this step (in radians, not degrees)
                double angle = i * Mathf.PI * 2;

                // the X & Y position for this angle are calculated using Sin & Cos
                float x = Mathf.Sin((float)angle) * radius;
                float y = Mathf.Cos((float)angle) * radius;
                points[pointNum] = new Vector3(x, y, 0) + t.gameObject.transform.position;
            }

            pointNum = 0;
            while( (Time.time < startTime + secondsToStick) ) {
                if(t.units > 0 ){
                    Quaternion rotation = Quaternion.LookRotation(t.gameObject.transform.position - points[pointNum % (numPoints - 1)], Vector3.forward);
                    rotation.x = 0;
                    rotation.y = 0;            
                    GameObject go = (GameObject)Network.Instantiate(rogueUnit, points[pointNum % (numPoints - 1)],rotation , 0);
                    t.SubUnit();
                }
                t.Blink();
                pointNum++;
                yield return new WaitForSeconds(0.15f);
            }
            t.DeMagnetize();
            t.updateSprite();
            Network.Destroy(this.gameObject);
            yield return 0;
        }
    }
}