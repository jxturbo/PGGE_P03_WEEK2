using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public FSM mFsm = new FSM();
    public Animator mAnimator;
    public PlayerMovement mPlayerMovement;

    [HideInInspector]
    public bool[] mAttackButtons = new bool[3];

    // This is the maximum number of bullets that the player 
    // needs to fire before reloading.
    public int mMaxAmunitionBeforeReload = 40;

    // This is the total number of bullets that the 
    // player has.
    [HideInInspector]
    public int mAmunitionCount = 100;

    // This is the count of bullets in the magazine.
    [HideInInspector]
    public int mBulletsInMagazine = 40;
    public Transform mGunTransform;
    public LayerMask mPlayerMask;
    public RectTransform mCrossHair;
    public Canvas mCanvas;
    public GameObject mBulletPrefab;
    public float mBulletSpeed = 10.0f;
    public int[] RoundsPerSecond = new int[3];
    bool[] mFiring = new bool[3];




    // Start is called before the first frame update
    void Start()
    {
        mFsm.Add(new PlayerState_MOVEMENT(this));
        mFsm.Add(new PlayerState_ATTACK(this));
        mFsm.Add(new PlayerState_RELOAD(this));
        mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
    }

    // Update is called once per frame
    void Update()
    {
        mFsm.Update();
        Aim();

        // For Student ----------------------------------------------------------//
        // Implement the logic of button clicks for shooting. 
            //-----------------------------------------------------------------------//
        if (Input.GetButton("Fire1"))
        {
        mAttackButtons[0] = true;
        mAttackButtons[1] = false;
        mAttackButtons[2] = false;
        }
        else
        {
        mAttackButtons[0] = false;
        }

        if (Input.GetButton("Fire2"))
        {
        mAttackButtons[0] = false;
        mAttackButtons[1] = true;
        mAttackButtons[2] = false;
        }
        else
        {
        mAttackButtons[1] = false;
        }

        if (Input.GetButton("Fire3"))
        {
        mAttackButtons[0] = false;
        mAttackButtons[1] = false;
        mAttackButtons[2] = true;
        }
        else
        {
        mAttackButtons[2] = false;
        }
    }
    public void Move()
    {
        mPlayerMovement.HandleInputs();
        mPlayerMovement.Move();
    }
    public void Aim()
    {
        // For Student ----------------------------------------------------------//
        // Implement the logic of aiming and showing the crosshair
        // if there is an intersection.
        //
        // Hints:
        // Find the direction of fire.
        // Find gunpoint as mentioned in the worksheet.
        // Find the layer mask for objects that you want to intersect with.
        //
        //Do the Raycast\
        // Vector3 GunDirection = -mGunTransform.right.normalized;
        // LayerMask layerMask = ~mPlayerMask;
        // Vector3 GunPoint = mGunTransform.transform.position + dir * 1.2f - mGunTransform.forward * 0.1f;
        // bool intersected = Physics.Raycast(mGunTransform.position, GunDirection, out RaycastHit hit, 50f, layerMask);
        // if (intersected)
        // {
        //     Debug.DrawRay(mGunTransform.position, hit.point, Color.blue);
        //     mCrossHair.gameObject.SetActive(true);
        //     mCrossHair.position = hit.point;

        //     // Draw a line as debug to show the aim of fire in scene view.
        //     // Find the transformed intersected point to screenspace
        //     // and then transform the crosshair position to this
        //     // new position.
        //     // Enable or set active the crosshair gameobject.
        // }
        // else
        // {
        //     // Hide or set inactive the crosshair gameobject.
        // }
        //-----------------------------------------------------------------------//

        // Uncomment the code below and start adding your codes.
        
        Vector3 dir = -mGunTransform.right.normalized;;
        // Find gunpoint as mentioned in the worksheet.
        Vector3 gunpoint = mGunTransform.transform.position +
                           dir * 1.2f -
                           mGunTransform.forward * 0.1f;
        // Fine the layer mask for objects that you want to intersect with.
        LayerMask objectsMask = ~mPlayerMask;

        // Do the Raycast
        RaycastHit hit;
        bool flag = Physics.Raycast(gunpoint, dir,
                        out hit, 50.0f, objectsMask);
        if (flag)
        {
            // Draw a line as debug to show the aim of fire in scene view.
            Debug.DrawLine(gunpoint, gunpoint +
                (dir * hit.distance), Color.red, 0.0f);
            RectTransform CanvasRect = mCanvas.GetComponent<RectTransform>();
            Vector2 ViewpointPosition = Camera.main.WorldToViewportPoint(hit.point);
            Vector2 PositionOfCrossHairOnCanvas = new Vector2(((ViewpointPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewpointPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

 
            mCrossHair.anchoredPosition = PositionOfCrossHairOnCanvas;

            // Find the transformed intersected point to screenspace
            // and then transform the crosshair position to this
            // new position.

            // Enable or set active the crosshair gameobject.
            mCrossHair.gameObject.SetActive(true);
        }
        else
        {
            // Hide or set inactive the crosshair gameobject.
            mCrossHair.gameObject.SetActive(true);
        }
        
    }

    public void NoAmmo()
    {
    }

    public void Reload()
    {
    }

    public void Fire(int id)
    {
        if(mFiring[id] == false)
        {
            StartCoroutine(Coroutine_Firing(id));
        } 

    }
    public void FireBullet()
    {
        if (mBulletPrefab == null) return;

        Vector3 dir = -mGunTransform.right.normalized;
        Vector3 firePoint = mGunTransform.transform.position + dir * 
            1.2f - mGunTransform.forward * 0.1f; 
        GameObject bullet = Instantiate(mBulletPrefab, firePoint,
            Quaternion.LookRotation(dir) * Quaternion.AngleAxis(90.0f, Vector3.right));

        bullet.GetComponent<Rigidbody>().AddForce(dir * mBulletSpeed, ForceMode.Impulse);
    }
    IEnumerator Coroutine_Firing(int id)
    {
        mFiring[id] = true; 
        FireBullet(); 
        yield return new WaitForSeconds(1.0f / RoundsPerSecond[id]); 
        mFiring[id] = false; 
         mBulletsInMagazine -= 1;
    }



}
