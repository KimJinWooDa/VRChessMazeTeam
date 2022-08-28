using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIsHovering : MonoBehaviour
{
    Material[] mats;

    Renderer renderer;

    public bool isHovering;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        mats = renderer.materials;
    }
    private void Update()
    {
        mats[1].SetFloat("_Thickness", isHovering ? 1.055f : 1f);
    }

}
