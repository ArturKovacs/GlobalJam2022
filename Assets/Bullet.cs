using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float speed;
    void Awake()
    {
        speed = GlobalManager.Instance.GetBulletSpeed();
    }
    void Update()
    {
        transform.Translate(new Vector3(0, -1, 0) * speed * Time.deltaTime);
        CheckToErase();
    }

    private void CheckToErase()
    {
        if (transform.position.y < GlobalManager.Instance.GetBottomOfScreen()) Destroy(this.gameObject);
    }
}
