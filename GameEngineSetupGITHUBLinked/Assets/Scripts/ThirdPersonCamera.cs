using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform mPlayer; 
    TPCBase mThirdPersonCamera; 
    public float playerHeight;
    public Vector3 mPositionOffset = new Vector3(0.0f, 2.0f, -2.5f);
    public Vector3 mAngleOffset = new Vector3(0.0f, 0.0f, 0.0f);
    public float mDamping = 1.0f;


    void Start() 
    { 
        // Set to GameConstants class so that other objects can use.
        GameConstants.Damping = mDamping;
        GameConstants.CameraPositionOffset = mPositionOffset;
        GameConstants.CameraAngleOffset = mAngleOffset;

        mThirdPersonCamera = new TPCTrack(transform, mPlayer);

    } 
    void LateUpdate() 
    { 
        GameConstants.CameraPositionOffset = mPositionOffset;
        mThirdPersonCamera.Update(); 
    } 
}
public abstract class TPCBase 

{ 
    protected Transform mCameraTransform; 
    protected Transform mPlayerTransform; 
    public Transform CameraTransform 
    { 
        get 
        { 
            return mCameraTransform; 
        } 
    } 
    public Transform PlayerTransform 
    { 
        get 
        { 
            return mPlayerTransform; 
        } 
    }
 

    public TPCBase(Transform cameraTransform, Transform playerTransform) 
    { 
        mCameraTransform = cameraTransform; 
        mPlayerTransform = playerTransform; 

    } 
    public abstract void Update(); 
} 
public class TPCTrack : TPCBase 
{ 
    public TPCTrack(Transform cameraTransform, Transform playerTransform)  
        : base(cameraTransform, playerTransform) 
    { 
    } 
    public override void Update() 
    { 
        Vector3 targetPos = mPlayerTransform.position;
        Debug.Log(targetPos);
        // We add the camera offset on the Y-axis. 
        targetPos.y += GameConstants.CameraPositionOffset.y;
        mCameraTransform.LookAt(targetPos);

    } 
}
public abstract class TPCFollow : TPCBase
{
    public TPCFollow(Transform cameraTransform, Transform playerTransform)
        : base(cameraTransform, playerTransform)
    {
    }

    public override void Update()
    {
        // // Now we calculate the camera transformed axes.
        // // We do this because our camera's rotation might have changed
        // // in the derived class Update implementations. Calculate the new 
        // // forward, up and right vectors for the camera.
        // Vector3 forward = *** Your code ***;
        // Vector3 right = *** Your code ***;
        // Vector3 up = *** Your code ***;

        // // We then calculate the offset in the camera's coordinate frame. 
        // // For this we first calculate the targetPos
        // Vector3 targetPos = mPlayerTransform.position;

        // // Add the camera offset to the target position.
        // // Note that we cannot just add the offset.
        // // You will need to take care of the direction as well.
        // Vector3 desiredPosition = *** Your code ***;

        // // Finally, we change the position of the camera, 
        // // not directly, but by applying Lerp.
        // Vector3 position = Vector3.Lerp(mCameraTransform.position,
        //     desiredPosition, Time.deltaTime * GameConstants.Damping);
        // mCameraTransform.position = position;
    }
}

public static class GameConstants
{
    public static Vector3 CameraAngleOffset { get; set; }
    public static Vector3 CameraPositionOffset { get; set; }
    public static float Damping { get; set; }

}

