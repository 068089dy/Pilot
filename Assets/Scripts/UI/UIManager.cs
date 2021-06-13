using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text bigToastText;

    public Text keyToastText;
    public Image keyToastImg;
    public Text keyToastImgKey;

    float bTLastShowTime;
    float bTShowDuration;

    float kTLastShowTime;
    float kTShowDuration;

    public GameObject ESCPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > bTLastShowTime + bTShowDuration)
        {
            bigToastText.enabled = false;
        } else
        {
            bigToastText.enabled = true;
        }

        if (Time.time > kTLastShowTime + kTShowDuration)
        {
            keyToastText.enabled = false;
            keyToastImg.enabled = false;
            keyToastImgKey.enabled = false;
        }
        else
        {
            keyToastText.enabled = true;
            keyToastImg.enabled = true;
            keyToastImgKey.enabled = true;
        }

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (Cursor.visible == )
        //    {
        //        showESCPanel();
        //    } else
        //    {
        //        hideESCPanel();
        //    }
        //}
    }

    public void showBigToast(string content, float time)
    {
        bigToastText.text = content;
        bTLastShowTime = Time.time;
        bTShowDuration = time;
    }

    public void showKeyToast(string key, string content, float time)
    {
        keyToastImgKey.text = key;
        keyToastText.text = content;
        kTLastShowTime = Time.time;
        kTShowDuration = time;
    }

    public void showESCPanel()
    {
        ESCPanel.SetActive(true);
    }

    public void hideESCPanel()
    {
        ESCPanel.SetActive(false);
    }

    public void Cancel()
    {
        ESCPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
