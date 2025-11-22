using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        testInputAction = new TestInputAction();
        testInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (testInputAction.Player.Fire.triggered)
        {
            Debug.Log("ファイアー!!!!!");
        }
    }
}
