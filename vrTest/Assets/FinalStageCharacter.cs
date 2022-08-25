using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStageCharacter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GOALIN"))
        {
            GameObject fadeManager = GameObject.Find("Fade Manager");
            GameManager.instance.stageNum++;
            fadeManager.GetComponent<FadeManager>().GoToScene(0);
            if (GameManager.instance.stageNum == 5)
            {
                GameManager.instance.FinalUI();
            }

        }
    }
}
