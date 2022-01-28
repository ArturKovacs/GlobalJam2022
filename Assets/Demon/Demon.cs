using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour
{
    public float Speed;
    public bool IsContollerTarget;

    public GameObject Platform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsContollerTarget)
        {
            if (Input.GetButtonDown("Submit"))
            {
                PlacePlatform();
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsContollerTarget)
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            var deltaPos = new Vector3(horizontal, vertical, 0) * Speed * Time.deltaTime;
            gameObject.transform.Translate(deltaPos);
        }
    }
    private void PlacePlatform()
    {
        var offset = new Vector3(0, 2, 0);
        var platformPos = transform.position + offset;
        Instantiate(Platform, platformPos, Quaternion.identity);
    }
}
