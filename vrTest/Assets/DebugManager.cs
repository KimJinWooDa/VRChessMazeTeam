using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public Text[] debugText;
    public bool isHovering;
    public bool isTrigger;

    public static DebugManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
           Destroy(gameObject);
        }
    }
    public static DebugManager Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Update()
    {
        for(int i = 0; i < debugText.Length; i++)
        {
            debugText[i].text = "호버링중" + isHovering + "트리거 누름" + isTrigger;
        }
        if (isTrigger)
        {
            float t = 2f;
            if(t > 0)
            {
                t -= Time.deltaTime;
            }
            else
            {
                isTrigger = false;
                t = 2f;
            }
        }
    }
}
