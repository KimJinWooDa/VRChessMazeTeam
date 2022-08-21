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
        uiImage.gameObject.SetActive(false);
        rectTrans = GetComponent<RectTransform>();
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


    private void Start()
    {
        followImage.sprite = followSprites[imageCount];
        StartCoroutine(FadeOutImage(followImage));
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
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

        yield return new WaitForSeconds(3f);

        while (image.color.a > 0)
        {
            Color a = image.color;
            a.a -= 0.35f * Time.deltaTime; //UI 사라지는 속도? 0.35f
            image.color = new Vector4(1, 1, 1, a.a);
            yield return null;
        }
        imageCount++;
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width(imageCount));
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height(imageCount));
        followImage.sprite = followSprites[imageCount];
        followImage.color = new Vector4(1, 1, 1, 1);
        StartCoroutine(FadeOutImage(followImage));
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

    float width;
    public float Width(int index)
    {
        switch (index)
        {
            case 0:
                break;
            case 1:
                width = 1232f;
                break;
            case 2:
                width = 1381f;
                break;
            case 3:
                width = 1242f;
                break;
            default:
                break;
        }
        return width;
    }

    float height;
    public float Height(int index)
    {
        
        switch (index)
        {
            case 0:
                break;
            case 1:
                height = 340f;
                break;
            case 2:
                height =289f;
                break;
            case 3:
                height =299f;
                break;
            default:
                break;
        }
        return height;
    }
}
