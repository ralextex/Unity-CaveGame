using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float gravityScale = 10f;
    public float fallingGravityScale = 50f;
    public float jumpSpeed = 10f;
    private float xInput, zInput;
    public Vector3 MoveVector{set; get;}

    private bool isGrounded = false;
    
    public bool init;

    public Transform cam;
    public Rigidbody rb;

    private void Start()
    {
        init = true;
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(init && Singleton.Instance.finishedGen)
        {
            transform.position = Singleton.Instance.start;
            init = false;
        }
        // Good for handling inputs and animations
        else
        {
            ProcessInputs();
            MoveVector = PoolInput();
            MoveVector = RotateWithView();
            Move();
        }
        
    }
    private void ProcessInputs()
    {
        if (Input.GetKeyDown("space") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpSpeed * Time.deltaTime, ForceMode.Impulse);
            isGrounded = false;
        }

        if(rb.velocity.y >= 0)
        {
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
            rb.AddForce(Physics.gravity * (fallingGravityScale - 1) * rb.mass * Time.deltaTime);
        }
    }

    private Vector3 PoolInput()
    {
        Vector3 dir = Vector3.zero;

        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
        return dir;
    }

    private void Move()
    {
        rb.AddForce(moveSpeed * Time.deltaTime * MoveVector);
    }

    private Vector3 RotateWithView()
    {
        if(cam != null)
        {
            Vector3 dir = cam.TransformDirection(MoveVector);
            dir.Set(dir.x, 0, dir.z);
            return dir.normalized * MoveVector.magnitude;
        }
        else
        {
            cam = Camera.main.transform;
            return MoveVector;
        }
    }

}
