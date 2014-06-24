using UnityEngine;
using System.Collections;

public class BurstBehavior : MonoBehaviour {

    #region sprite
    private Sprite purpleSprite;
    private Sprite yellowSprite;
    private SpriteRenderer myRender;
    #endregion

    #region privateVar
    private float timeLived = 0;
    private float speed = 6;
    private float dacayRate = .9f;
    public bool isRogue;
    #endregion

    // Use this for initialization
    void Start () {

        // load objects
        myRender = (SpriteRenderer)renderer;

        // load and set sprites
        if (!isRogue) {
            purpleSprite = Resources.Load ("Textures/MinionYellow", typeof(Sprite)) as Sprite;
            yellowSprite = Resources.Load ("Textures/MinionBlue", typeof(Sprite)) as Sprite;
        } else {
            purpleSprite = Resources.Load ("Textures/MinionRogue", typeof(Sprite)) as Sprite;
            yellowSprite = Resources.Load ("Textures/MinionRogue", typeof(Sprite)) as Sprite;
        }
        int randomInt = Random.Range (0, 2);
        myRender.sprite = (randomInt == 0) ? purpleSprite : yellowSprite;
    }
    
    // Update is called once per frame
    void Update () {
      if (this.transform.localScale.x < .1f) {
          Destroy(this.gameObject);
      }
      transform.position += (speed * Time.smoothDeltaTime) * transform.up;
      this.transform.localScale *= dacayRate;
    }
}
