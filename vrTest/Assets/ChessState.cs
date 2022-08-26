using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessState : MonoBehaviour {
    [SerializeField] private int chessState;
    
    private void Start() {
        Material[] mats = GetComponent<MeshRenderer>().materials;
        if (chessState == GameManager.instance.stageNum) {
            mats[1].SetFloat("_Thickness", 1.1f);
        }
        else {
            mats[0].SetColor("_BaseColor", Color.black);
            mats[1].SetFloat("_Thickness", 0f);
        }
    }
}
