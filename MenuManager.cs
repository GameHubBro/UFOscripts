using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //скрипт главного меню

    [SerializeField] private GameObject optionsPanel;                                   //панель настроек
    [SerializeField] private GameObject creditsPanel;                                   //панель про автора
    [SerializeField] private Button volumeOff;                                          //кнопка выключения звука
    [SerializeField] private Button volumeOn;                                           //кнопка включения звука

    private void Awake()
    {
        optionsPanel.SetActive(false);                                                  //в начале игры делаем панель настроек и автора не активными, кнопка включения музыки - ВКЛ
        creditsPanel.SetActive(false);
        VolumeOn();
    }

    public void NewGameButton()                                                         //загружаем первый уровень
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionsButton()
    {
        OptionsPanel();
    }

    public void CreditsButton()
    {
        CreditsPanel();
    }

    public void ExitGameButton()
    {     
        Application.Quit();                                                             //выходим из игры
    }

    public void VolumeOn()
    {
        Color transparent = volumeOn.image.color;                                       //при включении музыки делаем кнопку включения более прозрачной (не активной),
        transparent.a = 0.5f;
        volumeOn.image.color = transparent;

        Color notTransparent = volumeOff.image.color;                                   //а кнопку выключения не прозрачной (активной)
        notTransparent.a = 1f;
        volumeOff.image.color = notTransparent;

    }

    public void VolumeOff()
    {
        Color transparent = volumeOff.image.color;                                      //при выключении музыки делаем кнопку выключения более прозрачной (не активной),
        transparent.a = 0.5f;
        volumeOff.image.color = transparent;

        Color notTransparent = volumeOn.image.color;                                    //а кнопку включения не прозрачной (активной)
        notTransparent.a = 1f;
        volumeOn.image.color = notTransparent;

    }

    private void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);                                                   //делаем панель настроек активной
    }

    private void HideOptionsPanel()
    {
        optionsPanel.SetActive(false);                                                  //делаем панель настроек не активной
    }

    private void OpenCreditsPanel()
    {
        creditsPanel.SetActive(true);                                                   //делаем панель автора активной
    }

    private void HideCreditsPanel()
    {
        creditsPanel.SetActive(false);                                                   //делаем панель автора не активной
    }

    private void OptionsPanel()                                                         //если панель настроек активна - делаем не активной и наоборот
    {
        if (optionsPanel.activeInHierarchy)
        {
            HideOptionsPanel();
        }
        else
        {
            OpenOptionsPanel();
        }
    }

    private void CreditsPanel()                                                         //если панель автора активна - делаем не активной и наоборот
    {
        if (creditsPanel.activeInHierarchy)
        {
            HideCreditsPanel();
        }
        else
        {
            OpenCreditsPanel();
        }
    }
}
