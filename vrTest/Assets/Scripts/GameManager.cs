using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] Image[] uiImages;

    static GameManager Instance = null;

    //궁금한게 다른 씬 갔다가 다시 메인씬으로 가면 설정유지되나?
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        uiImages[1].gameObject.SetActive(false);
    }

    public static GameManager instance
    {
        get
        {
            if(Instance == null)
            {
                GameManager GameManager = new GameObject("GameManager").AddComponent<GameManager>();
                Instance = GameManager;
                DontDestroyOnLoad(Instance);
                return Instance;
            }
            return Instance;
        }
    }

    public void SetUI(int index, bool isOn)
    {
        switch (index)
        {
            case 0:
                uiImages[index].gameObject.SetActive(isOn);
                break;
            case 1:
                uiImages[index].gameObject.SetActive(isOn);
                break;
            default:
                break;
        }
        
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            //GameManager.instance.SetUI(0, true);
            GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(0);
        }
    }
}
