using UnityEngine;
using System.Collections;

namespace Global{
    public class ShockTowerRange : MonoBehaviour {

        private GameObject shockTowerShoot;
        private GameObject shockRangePrefab;

        void Start () {
            shockRangePrefab = Resources.Load("Prefabs/ShockTowerRange") as GameObject;
            if (shockRangePrefab == null) {
                print ("range prefab is not null");
            }
            if (shockRangePrefab == null) {
                print ("range prefab is null");
            }
            shockTowerShoot = (GameObject)(Instantiate(shockRangePrefab, 
                this.transform.parent.position, Quaternion.Euler(0, 0, 0)));
        }
      
        // Update is called once per frame
        void Update () {
            shockTowerShoot.GetComponent<ShockTowerShoot>().parentsOwner = 
                this.transform.parent.gameObject.GetComponent<Tower>().myOwner;
        }
    }
}