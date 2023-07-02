using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLooper : MonoBehaviour
{
    float numBGPanels = 3.5f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Triggered: " + collider.name);

        float widthOfObject = ((BoxCollider2D)collider).size.x;

        Vector3 pos = collider.transform.position;

        pos.x += widthOfObject * numBGPanels - widthOfObject / 2f;

        collider.transform.position = pos;
    }
}
