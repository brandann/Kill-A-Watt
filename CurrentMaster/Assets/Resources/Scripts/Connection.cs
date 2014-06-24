    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    namespace Global{

    public class Connection : MonoBehaviour {

        public GameObject connectionPrefab; //set by inspector
        public Dictionary<GameObject,LineRenderer> connections = new Dictionary<GameObject,LineRenderer>();
        private double connectionDistance = 12; // change in inspector
        private ownerShip parentsOwner;
        
        void Start () {
            if(this.transform.parent.gameObject.name == "tower(Clone)")
                parentsOwner = this.transform.parent.gameObject.GetComponent<Tower>().myOwner;
            else if(this.transform.parent.gameObject.name == "ShockTower(Clone)")
                parentsOwner = this.transform.parent.gameObject.GetComponent<Tower>().myOwner;
            else
                parentsOwner = this.transform.parent.gameObject.GetComponent<DeathRay>().myOwner;

            findAdjacentTowers ();
            buildConnections ();
            updateConnectionColors ();
        }


        void Update () {
            if(this.transform.parent.gameObject.name == "tower(Clone)")
                parentsOwner = this.transform.parent.gameObject.GetComponent<Tower>().myOwner;
            else if(this.transform.parent.gameObject.name == "ShockTower(Clone)")
                parentsOwner = this.transform.parent.gameObject.GetComponent<Tower>().myOwner;
            else
                parentsOwner = this.transform.parent.gameObject.GetComponent<DeathRay>().myOwner;
            buildConnections ();
            updateConnectionColors ();
        }


        private void findAdjacentTowers(){
            GameObject[] objects = GameObject.FindGameObjectsWithTag( "Tower" );
            Vector3 from = this.transform.parent.position;
            foreach (GameObject go in objects) {
                Vector3 to = go.transform.position;
                float dist = Vector3.Magnitude(from - to);
                if(Mathf.Abs(dist) < connectionDistance && go != this.gameObject){
                    connections.Add(go,instantiateConnection());
                }
            }
        }

        private LineRenderer instantiateConnection(){
            GameObject go = (GameObject)(Instantiate(connectionPrefab, transform.position, Quaternion.Euler(0, 0, 0)));
            LineRenderer line  = go.GetComponent<LineRenderer> ();
            return line;
        }

        private void buildConnections(){
            foreach (var tower in connections) {
                Vector3 start = new Vector3(this.transform.parent.position.x,this.transform.parent.position.y, 1);
                Vector3 end =  new Vector3(tower.Key.transform.position.x,tower.Key.transform.position.y, 1);
                tower.Value.SetPosition(0 , start);
                tower.Value.SetPosition(1, end);
            }
        }

        private void updateConnectionColors(){
            foreach (var tower in connections) {
                if(parentsOwner == tower.Key.GetComponent<Tower>().myOwner && parentsOwner == ownerShip.Player1){
                    Color32 DarkYellow = new Color32(152,142,24,200);
                    tower.Value.SetColors(DarkYellow,DarkYellow);
                }
              
                //make red
                if(parentsOwner == tower.Key.GetComponent<Tower>().myOwner && parentsOwner == ownerShip.Player2){
                    Color32 DarkBlue = new Color32(22,74,144,200);
                    tower.Value.SetColors(DarkBlue,DarkBlue);
                }
                   
                //make blue
                if(ownerShip.Neutral == tower.Key.GetComponent<Tower>().myOwner || parentsOwner != tower.Key.GetComponent<Tower>().myOwner){
                    Color32 DarkGrey = new Color32(139,139,139,66);
                    tower.Value.SetColors(DarkGrey,DarkGrey);
                }
                //make line grey
            }
        }
    }
}
