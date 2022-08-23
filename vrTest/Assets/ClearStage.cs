using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GOALIN"))
        {
            GameObject fadeManager = GameObject.Find("Fade Manager");
            GameManager.instance.stageNum++;
            if (GameManager.instance.stageNum == 4)
            {
                fadeManager.GetComponent<FadeManager>().GoToScene(0);
                GameManager.instance.FinalUI();
            }
            
            

            Destroy(this.gameObject);
        }
    }
}
