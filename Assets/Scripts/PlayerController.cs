using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        object[] args = new object[2];
        args[0] = Input.GetAxis("Vertical");
        args[1] = Input.GetAxis("Horizontal");

        SendMessage("Mover", args);
    }
}
