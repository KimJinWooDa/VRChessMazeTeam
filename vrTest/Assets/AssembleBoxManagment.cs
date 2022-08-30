using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssembleBoxManagment : MonoBehaviour
{
    [SerializeField] GameObject[] assembleBoxs;
    private bool ended = false;

    private void Awake() {
        ended = false;
    }

    public void Assemble()
    {
        foreach (var item in assembleBoxs)
        {
            item.GetComponent<AssembleGroundBox>().isAssemble = true;
        }
        if (!ended) {
            ended = true;
            AudioSource s;
            if(TryGetComponent(out s)) s.Play();
        }
    }
}
