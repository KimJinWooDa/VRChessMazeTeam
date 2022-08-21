using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIsHovering : MonoBehaviour
{
    Material[] mats;

    Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        mats = renderer.sharedMaterials;
    }

    public void IsHovering(bool isHovering)
    {
        mats[1].SetFloat("_Thickness", isHovering ? 1.1f : 1f);
        //renderer.materials[1].SetFloat("_Thickness", isHovering ? 1.1f : 1f);
    }
}
