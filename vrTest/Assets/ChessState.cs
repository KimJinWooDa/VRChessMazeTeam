using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessState : MonoBehaviour {
    [SerializeField] private int chessState;

    private void Start()
    {
        Material[] mats = GetComponent<MeshRenderer>().materials;

        mats[0].SetColor("_BaseColor", Color.black);
        mats[1].SetFloat("_Thickness", 0f);
    }

    public void TriggerState()
    {
        Material[] mats = GetComponent<MeshRenderer>().materials;
        Color color = new Color(62f,58f,128f,255f);
        mats[0].SetColor("_BaseColor", color);
        mats[1].SetFloat("_Thickness", 1.1f);
    }
}
