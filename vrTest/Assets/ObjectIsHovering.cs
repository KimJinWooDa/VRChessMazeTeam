using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIsHovering : MonoBehaviour
{
    Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void IsHovering(bool isHovering)
    {
        if (isHovering)
        {
            renderer.materials[1].SetFloat("_Thickness", 1.05f);
        }
        else
        {
            renderer.materials[1].SetFloat("_Thickness", 1f);
        }
    }
}
