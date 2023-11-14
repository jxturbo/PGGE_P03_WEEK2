using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Destroy the bullet after 10 seconds if it does not hit any object.
        StartCoroutine(Coroutine_Destroy(10.0f));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable obj = collision.gameObject.GetComponent<IDamageable>();
        if(obj != null)
        {
            obj.TakeDamage();
        }

        StartCoroutine(Coroutine_Destroy(0.1f));
    }

    IEnumerator Coroutine_Destroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
