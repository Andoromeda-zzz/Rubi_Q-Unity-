using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManagerTitle : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject TitleUI_PC;
    public GameObject TitleUI_display;

   
    [Header("Button")]
    public Button Start_in_PC;
    public Button Start_in_display;

    [Header("SE")]
    public SEScript SEClick;

    [Header("Text")]
    public GameObject text_AfterTouch;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Display.displays.Length > 1)
        {
            // 2台目のディスプレイを有効化（アクティベート）する
            // (配列なので 0 が1台目、 1 が2台目)
            Display.displays[1].Activate();
                
        }

        Start_in_PC.onClick.AddListener(NextScene_Lender);
        Start_in_display.onClick.AddListener(NextScene_Lender);
        text_AfterTouch.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }

        if (Input.anyKeyDown)
        {
            SEClick.PlayClickSound();
        }
    }


    public void NextScene_Lender()
    {

        string sceneName = "ModeSelect";
        
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            UnityEngine.Debug.Log("Scene name is empty!");
        }
    }
}
