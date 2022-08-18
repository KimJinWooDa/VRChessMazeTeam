using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**<summary>
 * 유일한 DontDestroyOnLoad singleton입니다.
 * Game-wide DDOL 싱글톤이 필요한 모든 개체는 개체 자체에 싱글톤 기능을 추가하지 말고, 여기에 그 reference를 만드시면 됩니다.
 * </summary>
 */
public class Vars : MonoBehaviour {
    public static Vars main;

    public string firstScene;

    //예시:
    //public GameManager game;

    void Awake() {
        if (main != null && main != this)
            Destroy(gameObject);
        else
            main = this;

        DontDestroyOnLoad(this);
    }

    void Start() {
        //이 밑에 필요한 로드 작업을 하십시오. 특히 MonoBehavior가 아닌 Game-wide DDOL의 Start()를 호출할 수 있습니다.
        //예시:
        //main.game.Load();

        SceneManager.LoadScene(main.firstScene);
    }
}
