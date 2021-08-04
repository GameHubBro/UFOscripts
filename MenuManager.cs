using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //������ �������� ����

    [SerializeField] private GameObject optionsPanel;                                   //������ ��������
    [SerializeField] private GameObject creditsPanel;                                   //������ ��� ������
    [SerializeField] private Button volumeOff;                                          //������ ���������� �����
    [SerializeField] private Button volumeOn;                                           //������ ��������� �����

    private void Awake()
    {
        optionsPanel.SetActive(false);                                                  //� ������ ���� ������ ������ �������� � ������ �� ���������, ������ ��������� ������ - ���
        creditsPanel.SetActive(false);
        VolumeOn();
    }

    public void NewGameButton()                                                         //��������� ������ �������
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
        Application.Quit();                                                             //������� �� ����
    }

    public void VolumeOn()
    {
        Color transparent = volumeOn.image.color;                                       //��� ��������� ������ ������ ������ ��������� ����� ���������� (�� ��������),
        transparent.a = 0.5f;
        volumeOn.image.color = transparent;

        Color notTransparent = volumeOff.image.color;                                   //� ������ ���������� �� ���������� (��������)
        notTransparent.a = 1f;
        volumeOff.image.color = notTransparent;

    }

    public void VolumeOff()
    {
        Color transparent = volumeOff.image.color;                                      //��� ���������� ������ ������ ������ ���������� ����� ���������� (�� ��������),
        transparent.a = 0.5f;
        volumeOff.image.color = transparent;

        Color notTransparent = volumeOn.image.color;                                    //� ������ ��������� �� ���������� (��������)
        notTransparent.a = 1f;
        volumeOn.image.color = notTransparent;

    }

    private void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);                                                   //������ ������ �������� ��������
    }

    private void HideOptionsPanel()
    {
        optionsPanel.SetActive(false);                                                  //������ ������ �������� �� ��������
    }

    private void OpenCreditsPanel()
    {
        creditsPanel.SetActive(true);                                                   //������ ������ ������ ��������
    }

    private void HideCreditsPanel()
    {
        creditsPanel.SetActive(false);                                                   //������ ������ ������ �� ��������
    }

    private void OptionsPanel()                                                         //���� ������ �������� ������� - ������ �� �������� � ��������
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

    private void CreditsPanel()                                                         //���� ������ ������ ������� - ������ �� �������� � ��������
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
