using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    //������ �������� ����

    [SerializeField] private GameObject menuPanel;                  //������ ����
    [SerializeField] private GameObject optionsPanel;               //������ ��������
    [SerializeField] private Button volumeOff;                      //������ ���������� �����
    [SerializeField] private Button volumeOn;                       //������ ��������� �����


    private float timer = 1f;                                       //������ �������� �� �������� ����

    private void Awake()
    {
        menuPanel.SetActive(false);                                //��������� ������ ����, ������ ��������, ������ ������ ��������� ����� ��������
        optionsPanel.SetActive(false);
        VolumeOn();
    }

    private void Update()                                           //����������� �������� ���� ���������� ������
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);           //������������� ������� �����
    }

    public void OptionsButton()
    {
        OptionsPanel();
    }

    public void BackToMenuButton()
    {
        SceneManager.LoadScene(0);                                                  //��������� ������� ����� - ������� ���� ����
    }

    public void VolumeOn()
    {
        Color transparent = volumeOn.image.color;                                   //��� ��������� ������ ������ ������ ��������� ����� ���������� (�� ��������),
        transparent.a = 0.5f;                                                       
        volumeOn.image.color = transparent;

        Color notTransparent = volumeOff.image.color;                               
        notTransparent.a = 1f;                                                      //� ������ ���������� �� ���������� (��������)
        volumeOff.image.color = notTransparent;

    }

    public void VolumeOff()
    {
        Color transparent = volumeOff.image.color;                                  //��� ���������� ������ ������ ������ ���������� ����� ���������� (�� ��������),
        transparent.a = 0.5f;
        volumeOff.image.color = transparent;

        Color notTransparent = volumeOn.image.color;                                //� ������ ��������� �� ���������� (��������)
        notTransparent.a = 1f;
        volumeOn.image.color = notTransparent;

    }

    private void OptionsPanel()
    {
        if (optionsPanel.activeInHierarchy)                                         //���� ������ �������� ���� �������, �� �������� � ��������
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
        optionsPanel.SetActive(true);                                               //������ �������� ������ ��������
    }

    private void HideOptionsPanel()
    {
        optionsPanel.SetActive(false);                                              //������ �� �������� ������ ��������
    }



    private void MenuPanel()
    {
        if (menuPanel.activeInHierarchy)                                            //���� ������� ���� �� ���� �������, �� �������� ��� � ������������� ����
        {
            HideMenu();
            timer = 1f;
        }
        else                                                                        //���� ������� ���� ���� ��������, �� ��������� ���� � ���������� ����
        {
            OpenMenu();
            timer = 0f;
        }
    }

    private void OpenMenu()
    {
        menuPanel.SetActive(true);                                                  //������ �������� ����
    }

    private void HideMenu()
    {
        menuPanel.SetActive(false);                                                 //������ �� �������� ����
    }
}
