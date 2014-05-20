//using UnityEngine;
//using System.Collections;

//public class MinionBehavior : MonoBehaviour {
//    public float speed = 10;
//    public float rotateSpeed = 50;
//    public GameObject teslaMinion;
//    public GameObject edisonMinion;


//    // Use this for initialization
//    void Start () {
	
//    }
	
//    // Update is called once per frame
//    void Update () {
//        transform.position += 1 * transform.up * (speed * Time.smoothDeltaTime);
//        //transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") * (rotateSpeed * Time.smoothDeltaTime));
	
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        //Debug.Log("Collieded:" + tag + " with " + other.tag);
//        if ( tag != other.tag && other.tag.Contains("Minion")    )
//        {            
//            Network.Destroy(other.gameObject);
//            Debug.Log("Destroying " + other.gameObject.GetType());
//        }
//    }



//}
