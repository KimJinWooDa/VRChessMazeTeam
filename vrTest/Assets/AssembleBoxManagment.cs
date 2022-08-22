using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssembleBoxManagment : MonoBehaviour
{
    [SerializeField] GameObject[] assembleBoxs;

    public void Assemble()
    {
        foreach (var item in assembleBoxs)
        {
            item.GetComponent<AssembleGroundBox>().isAssemble = true;
        }
    }
}
