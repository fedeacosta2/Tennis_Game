using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameHitbox : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private Bot bot;
    void Start()
    {
        
    }

    
    void Update()
    {
        GameHitboxMove();
    }

   

    private void GameHitboxMove()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, bot.PositionOfThErandomTarget.z); 
    }
}
