using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 Velocity;
    
    void Update()
    {
        transform.Translate(Velocity * Time.deltaTime, Space.World);
        CheckToErase();
    }

    private void CheckToErase()
    {
        if (transform.position.y < GlobalManager.Instance.GetBottomOfScreen()) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var girl = collision.GetComponent<Girl>();
        if (girl != null)
        {
            // We hit the girl
            girl.TakeDamage();
            return;
        }
        var platform = collision.GetComponent<Platform>();
        if (platform != null)
        {
            // We hit a platform, let's destroy both of us
            Destroy(gameObject);
            Destroy(platform.gameObject);
        }
    }
}
