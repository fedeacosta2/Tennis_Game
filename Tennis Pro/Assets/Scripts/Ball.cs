using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 initialPos;
    [SerializeField] private Transform line;
    public bool isLineMoving = true;
    void Start()
    {
        initialPos = transform.position;
        //line.position = transform.position;
        line.gameObject.SetActive(false);
    }
    void Update()
    {
        if (isLineMoving)
        {
            line.position = new Vector3(line.position.x, line.position.y, transform.position.z);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("wall"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = initialPos;
        }
        
       
    }
}
