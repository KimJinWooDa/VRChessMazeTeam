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
    public int needImageCount = 3;
    static GameManager Instance = null;
    public bool isPingPong;
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
        myCharacter.GetComponent<SimpleCapsuleWithStickMovement>().enabled = false; //못움직이게
        followImage.sprite = followSprites[imageCount];
        StartCoroutine(FadeOutImage(followImage));
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(0);
        }


        if (imageCount == 6)
        {
            GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
        }

    }
    public void OnCourtineFade()
    {
        needImageCount++;
        StartCoroutine(FadeOutImage(followImage));
    }
    IEnumerator FadeOutImage(Image image)
    {
        followImage.color = new Vector4(1, 1, 1, 1);
        if(needImageCount < 3) yield return new WaitForSeconds(3f);
        if (imageCount < needImageCount)
        {
            if (imageCount == 2)
            {
                
                myCharacter.GetComponent<SimpleCapsuleWithStickMovement>().enabled = true;
                StopCoroutine(FadeOutImage(followImage));

            }
            while (image.color.a > 0)
            {
                Color a = image.color;
                a.a -= 0.33f * Time.deltaTime;
                image.color = new Vector4(1, 1, 1, a.a);
                yield return null;
            }
            imageCount++;
            followImage.sprite = followSprites[imageCount];
            StartCoroutine(FadeOutImage(followImage));
        }
        else StopCoroutine(FadeOutImage(followImage));
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
