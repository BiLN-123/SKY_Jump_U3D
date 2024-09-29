using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingCtl : MonoBehaviour
{
    public Text loadingText;

    private int dotCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        // màn hình loading 2s trc khi load vào game
        //Invoke("LoadGameScene", 2f);

        InvokeRepeating("ChangeText",.35f,.35f);

        StartCoroutine(LoadGameSceneAsync());
    }
    void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    void ChangeText()
    {
        dotCount--;
        if (dotCount == -1)
        {
            dotCount = 3;
        }

        loadingText.text = "LOADING";

        if (dotCount > 0)
        {
            for (int i = 0; i < dotCount; i++)
            {
                loadingText.text += ".";
            }
        }    
    }

    IEnumerator LoadGameSceneAsync()
    {
        yield return new WaitForSeconds(2f);

        AsyncOperation async = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }

        Scene scene = SceneManager.GetSceneByName("Game");
        if (scene != null && scene.isLoaded)
        {
            SceneManager.SetActiveScene(scene);
            gameObject.SetActive(false);
        }    
    }
}
