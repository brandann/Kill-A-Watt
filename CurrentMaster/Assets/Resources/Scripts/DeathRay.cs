using UnityEngine;
using System.Collections;

namespace Global{

    public class DeathRay : MonoBehaviour {

        public ownerShip myOwner;
        public Transform rayGunSprite;
        Camera cam;
        ScientistAbility science;
        
        void Awake(){
          
        }

        void Start () {
            cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
            science = cam.GetComponent<ScientistAbility> ();
            if (myOwner == ownerShip.Player1)
              rayGunSprite = transform.FindChild ("DeathRayYellowGun");
            else
              rayGunSprite = transform.FindChild ("DeathRayBlueGun");
        }
      
        // Add stuff to deathray later
        void Update () {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            if (Network.isServer && science.currentAbility != ScientistAbility.ability.none && myOwner == ownerShip.Player1)
              rotateWeapon (mousePos);
            if(Network.isClient && science.currentAbility != ScientistAbility.ability.none && myOwner == ownerShip.Player2)
              networkView.RPC("RPCrotateWeapon", RPCMode.Server,mousePos);
        }
      
        public void rotateWeapon(Vector3 position) {
            rayGunSprite.rotation = Quaternion.Euler (0,0, Mathf.Atan2(position.y - transform.position.y,position.x - transform.position.x) * Mathf.Rad2Deg);
        }

        [RPC]
        public void RPCrotateWeapon(Vector3 position){
            rayGunSprite.rotation = Quaternion.Euler (0,0, Mathf.Atan2(position.y - transform.position.y,position.x - transform.position.x) * Mathf.Rad2Deg);
        }
    }
}