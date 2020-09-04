using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera maincam;
    public float scrollspeed, closestScroll, furthestScroll, camspeed;
    public float heightRatio, widthRatio;

    /*
     * Attach to Main Camera
     * Best values so far seem to be...
     * 
     * scrollspeed = 20
     * closestScroll = 4
     * furthestScroll = 50
     * camspeed = 3
     * height & width ratio (for mouse panning) = 0.05
     * 
     * change as needed
     */

    private float speedlimit, scrolllimit;
    Rigidbody rb;
    Vector3 pos;
    public static KeyCode Left, Right, Down, Up, RotateLeft, RotateRight, Pause;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        DefaultKeys();
    }

    void Update()
    {
        DetermineSpeedLimit();
        MouseCameraMovement();
        Scroll();

        pos = maincam.transform.position;
    }

    void MouseCameraMovement()
    { 
        //In this method, I've implemented skeleton code for when we can do key-binding

        Vector3 mpos = Input.mousePosition;

            if ((mpos.x <= (Screen.width*widthRatio) || Input.GetKey(Left)) && rb.velocity.x > -speedlimit)
            {
                //left
                rb.AddForce(new Vector3(-camspeed, 0, 0), ForceMode.VelocityChange);
            }
            else if ((mpos.x >= Screen.width-(Screen.width*widthRatio) || Input.GetKey(Right)) && rb.velocity.x < speedlimit)
            {
                //right
                rb.AddForce(new Vector3(camspeed, 0, 0), ForceMode.VelocityChange);
            }
            else
            {
                rb.AddForce(new Vector3(-rb.velocity.x*5f, 0, 0), ForceMode.Acceleration);
            }

        if ((mpos.y <= (Screen.height*heightRatio) || Input.GetKey(Down)) && rb.velocity.z > -speedlimit)
        {
            //down
            rb.AddForce(new Vector3(0, 0, -camspeed), ForceMode.VelocityChange);
        }
        else if ((mpos.y >= Screen.height - (Screen.height*heightRatio) || Input.GetKey(Up)) && rb.velocity.z < speedlimit)
        {
            //up
            rb.AddForce(new Vector3(0, 0, camspeed), ForceMode.VelocityChange);
        }
        else
        {
            rb.AddForce(new Vector3(0, 0, -rb.velocity.z * 5f), ForceMode.Acceleration);
        }
    }

    void Scroll()
    {
        if (Input.mouseScrollDelta.y > 0 && pos.y > closestScroll && rb.velocity.y > -scrolllimit)
        {
            //down
            rb.AddForce(new Vector3(0, -scrollspeed, 0), ForceMode.VelocityChange);
        }
        else if (Input.mouseScrollDelta.y < 0 && pos.y < furthestScroll && rb.velocity.y<scrolllimit)
        {
            //up
            rb.AddForce(new Vector3(0, scrollspeed, 0), ForceMode.VelocityChange);
        }
        else
        {
            rb.AddForce(new Vector3(0, -rb.velocity.y * 2f, 0), ForceMode.Acceleration);
        }

        if (pos.y < closestScroll)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    void DetermineSpeedLimit()
    {
        speedlimit = pos.y + 5f;
        scrolllimit = pos.y / closestScroll * 10f;
    }


    //overwrite this later when we add keybindings
    void DefaultKeys()
    {
        Left = KeyCode.LeftArrow;
        Right = KeyCode.RightArrow;
        Down = KeyCode.DownArrow;
        Up = KeyCode.UpArrow;
    }
}
