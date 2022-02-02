/** 
 *  @brief  Player Controller Class
 *  @author A Lesage 
 *  @date   01-2022
 ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    * @brief implements controls for the player
*/
public class PlayerController : MonoBehaviour
{
    // the speed at which the player moves
    public float moveSpeed = 10f;
    // gravity of player
    public float gravityScale = 10f;
    // gravity of player when falling
    public float fallingGravityScale = 50f;
    // jumping power at which the player moves
    public float jumpForce = 10f;
    // if ball is on the ground

    private bool isGrounded = false;

    // flag for initialisation
    public bool initFlag;

    // declare camera object
    public Transform cam;
    // declare Rigidbody object
    public Rigidbody rb;

    /**
    * @brief get rigidbody
    */
    private void Start()
    {
        initFlag = true;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    /**
        * @brief Move player to starting point and process inputs
    */
    void Update()
    {
        // Move player to starting point during initialization
        if(initFlag && GlobalPoints.Instance.finishedGen)
        {
            transform.position = GlobalPoints.Instance.start;
            initFlag = false;
        }
        else
        {
            ProcessInputs();
        }
        
    }
    
    /**
        * @brief get and process inputs from player
    */
    private void ProcessInputs()
    {
        // Jump
        if (Input.GetKeyDown("space") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
            isGrounded = false;
        }

        // Gravity
        if(rb.velocity.y >= 0)
        {
            // gravity in normal state
            rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass * Time.deltaTime);
            if(rb.velocity.y > 0){
                isGrounded = false;
            }
            else{
                isGrounded = true;
            }
        }
        else if (rb.velocity.y < 0)
        {
            // gravity when falling (stronger because of acceleration)
            rb.AddForce(Physics.gravity * (fallingGravityScale - 1) * rb.mass * Time.deltaTime);
        }

        // Move in XZ-Plane
        Vector3 dir = Vector3.zero;
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");

        // Correct move direction with camera rotation
        dir = MoveWithView(dir);

        // Add force to move player in XZ-Plane
        rb.AddForce(moveSpeed * Time.deltaTime * dir);
    }

    /**
    * @brief get input in direction in which player is facing
    * @param v Vector relation to camera
    */
    private Vector3 MoveWithView(Vector3 v)
    {
        if(cam != null)
        {
            Vector3 dir = cam.TransformDirection(v);
            dir.Set(dir.x, 0, dir.z);
            return dir.normalized * v.magnitude;
        }
        else
        {
            cam = Camera.main.transform;
            return v;
        }
    }

}
