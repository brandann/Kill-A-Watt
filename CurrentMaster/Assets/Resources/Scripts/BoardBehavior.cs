using UnityEngine;
using System.Collections;

namespace Global{

public class BoardBehavior : MonoBehaviour {

    GameManager manager = null;
    Camera mainCamera = null;
    StateManager stateManager = null;
    
    Vector2 mouseStart, mouseEnd;
    float screenHeight, screenWidth;
    private bool boxSelecting = false;

    //Used to draw the box selector
    private GUISkin boxSelectStyle;

	// Use this for initialization
	void Start () {
       GameObject goCam =  GameObject.FindGameObjectWithTag("MainCamera");
       manager = goCam.GetComponent<GameManager>();
       mainCamera = goCam.GetComponent<Camera>();
       stateManager = goCam.GetComponent<StateManager>();

       boxSelectStyle = Resources.Load("Textures/GUITextures/selectionGUI") as GUISkin;
       screenHeight = mainCamera.pixelHeight;
       screenWidth = mainCamera.pixelWidth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Box Selection

    //Used to Deselect Towers
    void OnMouseUpAsButton()
    {
        boxSelecting = false;
        mouseEnd = Input.mousePosition;

        if(Vector2.Distance(mouseStart, mouseEnd) < 15)
            manager.DeselectTowers(Network.isServer);
        else
        {
            Vector2 topLeft = mainCamera.ScreenToWorldPoint(mouseStart);
            Vector2 bottomRight = mainCamera.ScreenToWorldPoint(mouseEnd);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(topLeft, bottomRight);
            //print("found: " + colliders.Length +  " colliders from x: " + topLeft.x + " y: " + topLeft.y + "  to x: " + bottomRight.x + " y: " + bottomRight.y);

            //Should always collide with at least the GameBoard so if less than one none are selected and all selections are cleared
            if (colliders.Length < 2)
            {
                manager.DeselectTowers(Network.isServer);
                return;
            }
            foreach (Collider2D c in colliders)
            {
                Tower t = c.GetComponent<Tower>();
                if (t != null)
                {
                   
                    if (Network.isServer && t.myOwner == ownerShip.Player1 && t.selected == false)
                    {
                        t.ToggleSelect();
                        t.updateSprite();
                    }
                    else if (Network.isClient && t.myOwner == ownerShip.Player2 && t.selected == false)
                    {
                        t.networkView.RPC("ToggleSelect", RPCMode.Server);
                    }
                }
            }
        }
       
    }

    //------------------------------------------------------------------------------------------------------------------------------------
    

    void OnGUI()
    {
       
        if (stateManager.status != WorldGameState.InGame && boxSelecting)
            return;
        if(Input.GetMouseButton(0) && Vector2.Distance(mouseStart, Input.mousePosition) > 15 ){

            boxSelecting = true;
            GUI.Box(new Rect(mouseStart.x,screenHeight - mouseStart.y, Input.mousePosition.x - mouseStart.x, -( Input.mousePosition.y - mouseStart.y) ), "", boxSelectStyle.customStyles[0]);
        
        }
        
        
    }
    //----------------------------------------------------------------------------------------------------------------------------------

    void OnMouseDown()
    {
        boxSelecting = true;
        mouseStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        
    }

    #endregion



}

}
