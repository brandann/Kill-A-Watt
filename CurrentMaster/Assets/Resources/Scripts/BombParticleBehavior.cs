using UnityEngine;
using System.Collections;

namespace Global{
    public class BombParticleBehavior : MonoBehaviour {

        #region behaviorStuff
        private float speed = 15;
        private float decayRate = 1.1f;
        private float destroySize = 1;
        private float startSize = .1f;
        private int startmin = 1;
        private int startmax = 20;
        #endregion

        public ownerShip myOwner;

        #region spriteStuff
        private SpriteRenderer myRender;
        private string spriteY1 = "Textures/Bomb/bomby1";
        private string spriteY2 = "Textures/Bomb/bomby2";
        private string spriteY3 = "Textures/Bomb/bomby3";
        private string spriteB1 = "Textures/Bomb/bombb1";
        private string spriteB2 = "Textures/Bomb/bombb2";
        private string spriteB3 = "Textures/Bomb/bombb3";
        #endregion

        // Use this for initialization
        void Start () {
            myRender = (SpriteRenderer)renderer;
            transform.Rotate(new Vector3(0,0,Random.Range(0,360)));
            int randomNumber = Random.Range (1, 4);
            print ("BOMB RANDOM NUMBER: " + randomNumber);
            if (randomNumber < 3)  {
                setSprite(randomNumber);
            }

            // set small start size
            scale (startSize);
            move ((float) Random.Range(startmin,startmax));
        }
        
        // Update is called once per frame
        void Update () {
            //move and scale the particle
            move (0);
            scale(decayRate);
            destroy ();
        }

        // destroy the particle if it gets equals destroy size
        void destroy() {
            if (this.transform.localScale.x > destroySize) {
                Destroy(this.gameObject);
            }
        }

        // move the particle
        void move(float rate) {
          transform.position += (rate * speed * Time.smoothDeltaTime) * transform.up;
        }

        // decay the particle over time
        void scale(float percent) {
          this.transform.localScale *= percent;
        }

        // set the particles sprite randomly
        void setSprite(int randomNumber) {
          string makeSprite = "";
          Sprite currentSprite = null;

          if (myOwner == ownerShip.Player1) {
              switch (randomNumber) {
              case(1):
                  currentSprite = Resources.Load("Textures/Bomb/bomby1",typeof(Sprite)) as Sprite;
                  break;
              case(2):
                  currentSprite = Resources.Load("Textures/shadow",typeof(Sprite)) as Sprite;
                  break;
              }
          }
          else
          {
              switch(randomNumber) {
              case (1):
                  currentSprite = Resources.Load("Textures/Bomb/bombb1",typeof(Sprite)) as Sprite;
                  break;
              case(2):
                  currentSprite = Resources.Load("Textures/shadow",typeof(Sprite)) as Sprite;
                  break;
              }
          }
          myRender.sprite = currentSprite;
        }
    }
}