using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovemnt : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    public float gravity = -9.81f;
    Vector3 velocity;
    public bool isGrounded;
    public float jumpHeight = 3f;
    public Animator playerAnim;
    public float x;
    public float z;
    public bool mFollowCameraForward;
    public float mTurnRate;
    #if UNITY_ANDROID
        public FixedJoystick mJoystick;
    #endif

    // Update is called once per frame
    void Update()
    {
        #if UNITY_STANDALONE
                x = Input.GetAxis("Horizontal");
                z = Input.GetAxis("Vertical");
        #endif

        #if UNITY_ANDROID
                x = 2f * mJoystick.Horizontal;
                z = 2f * mJoystick.Vertical;
        #endif

        //checks if player is grounded by drawing a sphere
        isGrounded = controller.isGrounded;
        if(isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
        }
        // if(Input.GetKey(KeyCode.LeftShift))
        // {
        //     z = Input.GetAxis("Vertical");
        //     speed = 6f;
        // }
        // else
        // {
        //     z = Input.GetAxis("Vertical")/2;
        //     speed = 3f;
        // }
        // x = Input.GetAxis("Horizontal");
        playerAnim.SetFloat("PosX",x);
        playerAnim.SetFloat("PosZ",z);

        Vector3 move = transform.right * x + transform.forward * z;
        //used to rotate the player in the event you cant use the mouse to control camera "direction" \/
        if (mFollowCameraForward && move != Vector3.zero)
        {
            // rotate Player towards the camera forward.
            Vector3 eu = Camera.main.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(0.0f, eu.y, 0.0f),
                mTurnRate * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0.0f, x * 50f * Time.deltaTime, 0.0f);
        }

        controller.Move(move * speed * Time.deltaTime);
        //mimics gravity by pulling player down
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
