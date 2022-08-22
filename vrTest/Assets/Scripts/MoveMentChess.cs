using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMentChess : MonoBehaviour
{
    public enum ChessType { ºñ¼ó, ³ªÀÌÆ®, ·è};
    public ChessType chessType;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReStart player = other.GetComponent<ReStart>();
            player.ReStartGame();
        }
    }
}
