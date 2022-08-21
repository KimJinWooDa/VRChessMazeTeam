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
        uiImage.gameObject.SetActive(false);
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


        if (imageCount < needImageCount)
        {
            myCharacter.GetComponent<SimpleCapsuleWithStickMovement>().enabled = false;
            while (image.color.a > 0)
            {
                Color a = image.color;
                a.a -= 0.35f * Time.deltaTime; //UI 사라지는 속도? 0.35f
                image.color = new Vector4(1, 1, 1, a.a);
                yield return null;
            }
            if (imageCount == 2 && !isOnce)
            {
                isOnce = true;
                StopCoroutine(FadeOutImage(followImage));
            }
            if (imageCount == 6)
            {
                myCharacter.GetComponent<SimpleCapsuleWithStickMovement>().enabled = true;
                GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
                StopCoroutine(FadeOutImage(followImage));
                yield break;
            }


            followImage.sprite = followSprites[++imageCount];
            followImage.color = new Vector4(1, 1, 1, 1);
            StartCoroutine(FadeOutImage(followImage));
        }
        else
        {
            myCharacter.GetComponent<SimpleCapsuleWithStickMovement>().enabled = true;
            StopCoroutine(FadeOutImage(followImage));
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
}
