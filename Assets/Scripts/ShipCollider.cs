using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollider : MonoBehaviour
{
    void OnCollisionEnter2D (Collision2D collision)
    {
        SendMessageUpwards("CollideEnter", collision);
    }

    void OnCollisionExit2D (Collision2D collision)
    {
        SendMessageUpwards("CollideExit", collision);
    }

    void OnCollisionStay2D (Collision2D collision)
    {
        SendMessageUpwards("CollideStay", collision);
    }

}
