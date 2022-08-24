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
    [SerializeField] RectTransform rectTrans;

    [SerializeField] GameObject[] chessState;

    float width;
    float height;

    [Space(10f)]
    [Header("지민님 이거 건드시면 돼용")]
    [SerializeField] float ui2_Width_Scale = 1232f;
    [SerializeField] float ui3_Width_Scale = 1381f;
    [SerializeField] float ui4_Width_Scale = 1242f;
    [Space (10f)]
    [SerializeField] float ui2_Height_Scale = 340f;
    [SerializeField] float ui3_Height_Scale = 289f;
    [SerializeField] float ui4_Height_Scale = 299f;
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
    private void Start()
    {
        if (followImage != null)
        {
            followImage.sprite = followSprites[imageCount];
            StartCoroutine(FadeOutImage(followImage));
        }
    }
    [SerializeField] GameObject frame;
    [SerializeField] Material kingMaterial;
    [SerializeField] GameObject door;
    private void Update()
    {
        if (chessState != null)
        {
            for (int i = 0; i < chessState.Length; i++)
            {
                if(i == stageNum -1)
                {
                    chessState[i].GetComponent<MeshRenderer>().material.SetColor("_ChessColor", Color.blue);
                }
                else
                {
                    chessState[i].GetComponent<MeshRenderer>().material.SetColor("_ChessColor", Color.white);  
                }
            }
        }
        if(stageNum > 4)
        {
            frame.GetComponent<MeshRenderer>().materials[2] = kingMaterial;
        }

        if(OVRInput.GetDown(OVRInput.Button.Start)){
            GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(0);
        }

    }
    public void OnCourtineFade(int plus)
    {
        needImageCount = plus == 1 ? 3 : 7;
        StopCoroutine(FadeOutImage(followImage));
        StartCoroutine(FadeOutImage(followImage));
    }
    IEnumerator FadeOutImage(Image image)
    {
        yield return new WaitForSeconds(3f); //씬 나오는 시간 Delay

        while (image.color.a > 0)
        {
            Color a = image.color;
            a.a -= 0.35f * Time.deltaTime; //UI 사라지는 속도? 0.35f
            image.color = new Vector4(1, 1, 1, a.a);
            yield return null;
        }
        
        imageCount++;
        if (imageCount == 3) //4는 이제 마지막 UI나오기
        {
            StopCoroutine(FadeOutImage(followImage));
            yield break;
        }
        else
        {
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width(imageCount));
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height(imageCount));
            followImage.sprite = followSprites[imageCount];
            followImage.color = new Vector4(1, 1, 1, 1);
            StartCoroutine(FadeOutImage(followImage));
        }
    }

    public void FinalUI()
    {
        StartCoroutine(FinalSceneUI());
    }

    IEnumerator FinalSceneUI()
    {
        yield return new WaitForSeconds(2f);
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width(4));
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height(4));
        followImage.sprite = followSprites[4];
        followImage.color = new Vector4(1, 1, 1, 1);
        yield return new WaitForSeconds(3f);


        while (followImage.color.a > 0)
        {
            Color a = followImage.color;
            a.a -= 0.35f * Time.deltaTime; //UI 사라지는 속도? 0.35f
            followImage.color = new Vector4(1, 1, 1, a.a);
            yield return null;
        }
    }

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
                break;
        }
        return height;
    }
}
