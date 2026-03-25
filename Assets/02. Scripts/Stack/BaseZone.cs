using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseZone : MonoBehaviour
{
    GameObject zone;
    Color oriColor;
    Color activeColor = Color.green;

    Material mat;

    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;

        zone = this.gameObject;

        oriColor = mat.color;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        mat.color = activeColor;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        mat.color = oriColor;
    }
}
