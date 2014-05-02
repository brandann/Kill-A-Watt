using UnityEngine;
using System.Collections;
public class CameraController : MonoBehaviour
{
    public GameObject player;
    //The offset of the camera to centrate the player in the X axis
    public float offsetX = 5;
    public float rotationX = 5;
    //The offset of the camera to centrate the player in the Z axis
    public float offsetZ = -5;

    public float offsetY = 10;
    //The maximum distance permited to the camera to be far from the player, its used to     make a smooth movement
    public float maximumDistance = 15;
    //The velocity of your player, used to determine que speed of the camera
    public float playerVelocity = 10;


    private float movementX;
    private float movementZ;
    private float movementY;
    private float lookDownAngle = 25;
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(lookDownAngle, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        movementX = ((player.transform.position.x + offsetX - this.transform.position.x)) / maximumDistance;
        movementZ = ((player.transform.position.z + offsetZ - this.transform.position.z)) / maximumDistance;
        movementY = ((player.transform.position.y + offsetY - this.transform.position.y)) / maximumDistance;
        //movementX = player.transform.position.x + offsetX / maximumDistance;
        //movementY = player.transform.position.y + offsetY / maximumDistance;
        //movementZ = player.transform.position.z + offsetZ / maximumDistance;
        this.transform.position += new Vector3(movementX, movementY, movementZ);
    }
}