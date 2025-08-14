using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private static MainMenu instance;
    [SerializeField] private List<Image> playerImageList = new List<Image>();
    [SerializeField] private Button playButton;

    public List<Image> PlayerImageList { get => playerImageList; set => playerImageList = value; }
    public static MainMenu Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        playerImageList.ForEach(image => { image.gameObject.SetActive(false); });
        playButton.interactable = false;
    }


    public void enterPlayerMenu(int index)
    {
        PlayerImageList[index].gameObject.SetActive(true);

        //If index = 1, it means we have 2 players on scene
        if(index >= 1 && !playButton.IsInteractable())
        {
            playButton.interactable = true;
        }
    }

    public void exitPlayer(int index)
    {
        PlayerImageList[index].gameObject.SetActive(false);
    }

     public void loadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void disableUI(GameObject go) {
        go.SetActive(false);
        Debug.Log("desativou");
    }

    public void activeUI(GameObject go)
    {
        go.SetActive(true);
        Debug.Log("ativou");
    }
}
