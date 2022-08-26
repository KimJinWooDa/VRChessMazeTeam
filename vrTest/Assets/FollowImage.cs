using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowImage : MonoBehaviour {
    [SerializeField] Image followImage;
    [SerializeField] Sprite[] followSprites;
    int imageCount;

    void Start() {
        imageCount = followSprites.Length;
        if (followImage != null) {
            followImage.sprite = followSprites[imageCount];
            if (GameManager.instance.stageNum == 1) {
                StartCoroutine(FadeOutImage(followImage));
            }
        }

        if (GameManager.instance.stageNum > 1 && GameManager.instance.stageNum < 5) {
            this.gameObject.SetActive(false);
        }

        if (GameManager.instance.stageNum == 5) {
            this.gameObject.SetActive(true);
            followImage.sprite = followSprites[imageCount];
            StartCoroutine(FadeOutImage(followImage));
        }

    }

    IEnumerator FadeOutImage(Image image) {
        yield return new WaitForSeconds(3f); 

        while (image.color.a > 0) {
            Color a = image.color;
            a.a -= 0.35f * Time.deltaTime; 
            image.color = new Vector4(1, 1, 1, a.a);
            yield return null;
        }
    }


}
