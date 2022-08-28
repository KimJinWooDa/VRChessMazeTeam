using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowImage : MonoBehaviour {
    [SerializeField] Image followImage;
    [SerializeField] Sprite[] followSprites;
    [SerializeField] Sprite finalSprite;

    void Start() {
        if (followImage != null) {
            followImage.sprite = followSprites[0];
            if (GameManager.instance.stageNum == 1) {
                StartCoroutine(FadeOutImage(followImage));
            }
        }

        if (GameManager.instance.stageNum > 1 && GameManager.instance.stageNum < 5) {
            this.gameObject.SetActive(false);
        }

        if (GameManager.instance.stageNum == 5) {
            this.gameObject.SetActive(true);
            followImage.sprite = finalSprite;
            StartCoroutine(FadeOutFinalImage(followImage));
        }

    }
    int number = 0;
    IEnumerator FadeOutImage(Image image)
    {

        image.color = new Vector4(1, 1, 1, 1);
        yield return new WaitForSeconds(3f);


        while (image.color.a > 0)
        {
            Color a = image.color;
            a.a -= 0.35f * Time.deltaTime;
            image.color = new Vector4(1, 1, 1, a.a);
            yield return null;
        }
        number++;
        if (number == followSprites.Length)
        {
            StopCoroutine(FadeOutImage(image));
        }
        else
        {
            GameManager.instance.rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GameManager.instance.Width(number));
            GameManager.instance.rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GameManager.instance.Height(number));

            image.sprite = followSprites[number];
            StartCoroutine(FadeOutImage(image));
        }
    }

    IEnumerator FadeOutFinalImage(Image image)
    {
        GameManager.instance.rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GameManager.instance.Width(5));
        GameManager.instance.rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GameManager.instance.Height(5));
        followImage.color = new Vector4(1, 1, 1, 1);
        yield return new WaitForSeconds(3f);

        while (image.color.a > 0)
        {
            Color a = image.color;
            a.a -= 0.35f * Time.deltaTime;
            image.color = new Vector4(1, 1, 1, a.a);
            yield return null;
        }
    }


}
