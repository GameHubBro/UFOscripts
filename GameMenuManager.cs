using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    //скрипт игрового меню

    [SerializeField] private GameObject menuPanel;                  //панель меню
    [SerializeField] private GameObject optionsPanel;               //панель настроек
    [SerializeField] private Button volumeOff;                      //кнопка выключени€ звука
    [SerializeField] private Button volumeOn;                       //кнопка включени€ звука


    private float timer = 1f;                                       //таймер вли€ющий на скорость игры

    private void Awake()
    {
        menuPanel.SetActive(false);                                //отключаем панель меню, панель настроек, делаем кнопку включени€ звука активной
        optionsPanel.SetActive(false);
        VolumeOn();
    }

    private void Update()                                           //присваиваем скорость игры переменной таймер
    {
        Time.timeScale = timer;
    }

    public void PauseButton()                                       
    {
        MenuPanel();
    }

    public void ResumeButton()
    {
        MenuPanel();
    }

    public void NewGameButton()                                     
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);           //перезагружаем текущую сцену
    }

    public void OptionsButton()
    {
        OptionsPanel();
    }

    public void BackToMenuButton()
    {
        SceneManager.LoadScene(0);                                                  //загружаем нулевую сцену - главное меню игры
    }

    public void VolumeOn()
    {
        Color transparent = volumeOn.image.color;                                   //при включении музыки делаем кнопку включени€ более прозрачной (не активной),
        transparent.a = 0.5f;                                                       
        volumeOn.image.color = transparent;

        Color notTransparent = volumeOff.image.color;                               
        notTransparent.a = 1f;                                                      //а кнопку выключени€ не прозрачной (активной)
        volumeOff.image.color = notTransparent;

    }

    public void VolumeOff()
    {
        Color transparent = volumeOff.image.color;                                  //при выключении музыки делаем кнопку выключени€ более прозрачной (не активной),
        transparent.a = 0.5f;
        volumeOff.image.color = transparent;

        Color notTransparent = volumeOn.image.color;                                //а кнопку включени€ не прозрачной (активной)
        notTransparent.a = 1f;
        volumeOn.image.color = notTransparent;

    }

    private void OptionsPanel()
    {
        if (optionsPanel.activeInHierarchy)                                         //если панель настроек была активна, то скрываем и наоборот
        {
            HideOptionsPanel();
        }
        else
        {
            ShowOptionsPanel();
        }
    }

    private void ShowOptionsPanel()
    {
        optionsPanel.SetActive(true);                                               //делаем активной панель настроек
    }

    private void HideOptionsPanel()
    {
        optionsPanel.SetActive(false);                                              //делаем не активной панель настроек
    }



    private void MenuPanel()
    {
        if (menuPanel.activeInHierarchy)                                            //если игровое меню не было активно, то включаем его и останавливаем игру
        {
            HideMenu();
            timer = 1f;
        }
        else                                                                        //если игровое меню было активным, то выключаем меню и продолжаем игру
        {
            OpenMenu();
            timer = 0f;
        }
    }

    private void OpenMenu()
    {
        menuPanel.SetActive(true);                                                  //делаем активным меню
    }

    private void HideMenu()
    {
        menuPanel.SetActive(false);                                                 //делаем не активным меню
    }
}
