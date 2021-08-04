using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    [SerializeField] private ParticleSystem lowBodyTunnel;                  //������ �� ������� ������ ������� ������������ �����
    [SerializeField] private float upForce = 15f;                           //���������� ���� ������� �����


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HumanCatcher"))
        {
            Destroy(gameObject);                                            //���� ������� �������� HumanCather, ������� �� ���, �� ������ ���������
            if (lowBodyTunnel.isPlaying)
            {
                lowBodyTunnel.Stop();                                       //���� ������� �������, �� ��� �������������
            }
            UFO.humansCaught++;                                             //� ���������� ������ UFO ���������� 1 (�������� ������� ����� ���� ������� ��������)
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !UFO.isDead)              
        {
            if (lowBodyTunnel.isStopped)
            {
                lowBodyTunnel.Play();                                       //���� ������� �������� �������� Player, �� �������� ������� (���� �� ��� �������)
            }                                                               //� ��������� �������� �� ����������� � �������
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, other.gameObject.transform.position, upForce * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !UFO.isDead)                      //���� ������� �� �������� Player � ������� ��� �������, �� ������������� ���
        {
            if (lowBodyTunnel.isPlaying)
                lowBodyTunnel.Stop();
        }
    }
}
