using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoundaries : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.isTrigger && collision.CompareTag("Player"))
        {

        }
    }
}
