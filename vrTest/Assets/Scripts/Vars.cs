using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**<summary>
 * ������ DontDestroyOnLoad singleton�Դϴ�.
 * Game-wide DDOL �̱����� �ʿ��� ��� ��ü�� ��ü ��ü�� �̱��� ����� �߰����� ����, ���⿡ �� reference�� ����ø� �˴ϴ�.
 * </summary>
 */
public class Vars : MonoBehaviour {
    public static Vars main;

    public string firstScene;

    //����:
    //public GameManager game;

    void Awake() {
        if (main != null && main != this)
            Destroy(gameObject);
        else
            main = this;

        DontDestroyOnLoad(this);
    }

    void Start() {
        //�� �ؿ� �ʿ��� �ε� �۾��� �Ͻʽÿ�. Ư�� MonoBehavior�� �ƴ� Game-wide DDOL�� Start()�� ȣ���� �� �ֽ��ϴ�.
        //����:
        //main.game.Load();

        SceneManager.LoadScene(main.firstScene);
    }
}
