using UnityEngine;
using System.Collections;

namespace Global{

    public class BoxSelecting : MonoBehaviour {

        GameManager manager = null;
        Camera mainCamera = null;
        StateManager stateManager = null;
        Vector2 mouseStart, mouseEnd;
        float screenHeight, screenWidth;
        private bool boxSelecting = false;
        private GUISkin boxSelectStyle;//Used to draw the box selector

        // Use this for initialization
        void Start () {
             manager = gameObject.GetComponent<GameManager>();
             mainCamera = gameObject.GetComponent<Camera>();
             stateManager = gameObject.GetComponent<StateManager>();
             boxSelectStyle = Resources.Load("Textures/GUITextures/selectionGUI") as GUISkin;
             screenHeight = mainCamera.pixelHeight;
             screenWidth = mainCamera.pixelWidth;
        }

        #region BoxSelect

        // Update is called once per frame
        void Update () {
            if (Input.GetMouseButtonDown(0)) {
                mouseStart = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0) && boxSelecting) {
                boxSelecting = false;
                mouseEnd = Input.mousePosition;
                if (Vector2.Distance(mouseStart, mouseEnd) < 15) {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if ((hit.collider.gameObject.GetComponent<Tower>() == null)) {
                        manager.DeselectTowers(Network.isServer);
                        return;
                    }
                }
                else {
                    Vector2 topLeft = mainCamera.ScreenToWorldPoint(mouseStart);
                    Vector2 bottomRight = mainCamera.ScreenToWorldPoint(mouseEnd);
                    Collider2D[] colliders = Physics2D.OverlapAreaAll(topLeft, bottomRight);
                    manager.DeselectTowers(Network.isServer);
                    foreach (Collider2D c in colliders) {
                        Tower t = c.GetComponent<Tower>();
                        if (t != null) {
                            if (Network.isServer && t.myOwner == ownerShip.Player1 && t.selected == false) {
                                t.ToggleSelect();
                                t.updateSprite();
                            }
                            else if (Network.isClient && t.myOwner == ownerShip.Player2 && t.selected == false) {
                                t.networkView.RPC("ToggleSelect", RPCMode.Server);
                            }
                        }
                    }
                }
            }
        }
        
        void OnGUI() {
            if (stateManager.status != WorldGameState.InGame)
                return;
            if(Input.GetMouseButton(0) && Vector2.Distance(mouseStart, Input.mousePosition) > 15 ){
                boxSelecting = true;
                GUI.Box(new Rect(mouseStart.x,screenHeight - mouseStart.y, Input.mousePosition.x - mouseStart.x, -( Input.mousePosition.y - mouseStart.y) ), "", boxSelectStyle.customStyles[0]);
            }
        }

        #endregion
    }
}