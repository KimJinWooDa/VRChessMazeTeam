using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] Image uiImage;
    [SerializeField] Sprite[] followSprites;
    [SerializeField] Image followImage;
    [SerializeField] Transform myCharacter;

    [SerializeField] int imageCount;
    public int needImageCount;
    static GameManager Instance = null;
    public bool isPingPong;
    bool isOnce;
    public RectTransform rectTrans;

    [SerializeField] GameObject[] chessState;

    float width;
    float height;

    [Space(10f)]
    [Header("Áö¹Î´Ô ÀÌ°Å °Çµå½Ã¸é µÅ¿ë")]
    [SerializeField] float ui2_Width_Scale;
    [SerializeField] float ui3_Width_Scale;
    [SerializeField] float ui4_Width_Scale;
    [SerializeField] float ui5_Width_Scale;
    [Space (10f)]
    [SerializeField] float ui2_Height_Scale;
    [SerializeField] float ui3_Height_Scale;
    [SerializeField] float ui4_Height_Scale;
    [SerializeField] float ui5_Height_Scale;
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
        if(uiImage != null) uiImage.gameObject.SetActive(false);
        if(rectTrans != null) rectTrans = rectTrans.GetComponent<RectTransform>();
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

    public int stageNum = 1;

    [SerializeField] GameObject frame;
    [SerializeField] Material kingMaterial;
    [SerializeField] GameObject door;
   


    public void SetUI(bool isOn)
    {
        uiImage.gameObject.SetActive(isOn);
        isPingPong = isOn;
    }
    public void StartPingPong()
    {
        StartCoroutine(PingPongAlpha());
    }
    public IEnumerator PingPongAlpha()
    {
        //uiImages.color = new Vector4(1, 1, 1, Mathf.PingPong(t * Time.deltaTime, 1));
        uiImage.color = new Vector4(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.5f);
        uiImage.color = new Vector4(1, 1, 1, 0);
        yield return new WaitForSeconds(0.5f);
        if (isPingPong) StartCoroutine(PingPongAlpha());
        else StopCoroutine(PingPongAlpha());
    }

    
    public float Width(int index)
    {
        switch (index)
        {
            case 0:
                break;
            case 1:
                width = ui2_Width_Scale;
                break;
            case 2:
                width =ui3_Width_Scale ;
                break;
            case 3:
                width = ui4_Width_Scale;
                break;
            default:
                width = ui5_Width_Scale;
                break;
        }
        return width;
    }

    public float Height(int index)
    {
        
        switch (index)
        {
            case 0:
                break;
            case 1:
                height =ui2_Height_Scale ;
                break;
            case 2:
                height =ui3_Height_Scale;
                break;
            case 3:
                height =ui4_Height_Scale;
                break;
            default:
                height = ui5_Height_Scale;
                break;
        }
        return height;
    }
}
