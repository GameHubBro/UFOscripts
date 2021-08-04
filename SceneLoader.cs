using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    //������ ���������� �������

    [SerializeField] private GameObject menuPanel;                                  //������ �� ������ ����
    [SerializeField] private Button resumeButton;                                   //������ �� ������ "����������� ����"
    [SerializeField] private Animator winTextAnimator;                              //������ �� �������� ��������� ������

    public IEnumerator NextLevel(float delay)                                       //����� �������� ���������� ������
    {
        yield return new WaitForSeconds(delay);                                     //��������
        UFO.humansCaught = 0;                                                       //�������� ������� ��������� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       //��������� ��������� �����
    }

    public IEnumerator ResetScene(float delay)                                      //����� ����������� �����
    {
        yield return new WaitForSeconds(delay);                                     //��������
        UFO.isDead = false;                                                         //����������� �������� ���������� "�� �����"
        UFO.humansCaught = 0;                                                       //�������� ������� ��������� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);           //��������� ������� �����
    }

    public IEnumerator Win(float delay)                                             //����� ����� ������
    {
        yield return new WaitForSeconds(delay);                                     //��������
        UFO.humansCaught = 0;                                                       //�������� ������� ��������� �����
        menuPanel.SetActive(true);                                                  //���� �������� ������ ��������
        resumeButton.gameObject.SetActive(false);                                   //�������� ������ "����������� ����"
        winTextAnimator.SetBool("isWon", true);                                     //�������� ��������� ������ "�� ��������"
    }
}
