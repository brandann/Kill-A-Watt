using UnityEngine;
using System.Collections;

public class BurstManager : MonoBehaviour {

    private GameObject burstPrefab;
    public bool isRogue;

    #region privateVar
    private float timeElapsed;
    private float duration = 5;
    #endregion

    // Use this for initialization
    void Start () {
        burstPrefab = Resources.Load("Prefabs/Burst") as GameObject;
    }

    // Update is called once per frame
    void Update () {
        timeElapsed++;
        if (timeElapsed > duration) {
            Destroy(this.gameObject);
        }
        makeBurstPoint ();
    }

    private void makeBurstPoint() {
        GameObject e = Instantiate(burstPrefab) as GameObject;
        BurstBehavior spawnedEnemy = e.GetComponent<BurstBehavior>();

        if(spawnedEnemy != null) {
          if(isRogue){
              spawnedEnemy.isRogue = true;
          }
          e.transform.position = this.transform.position;
          spawnedEnemy.transform.Rotate(new Vector3(0,0,Random.Range(0,360)));
        }
    }
}
