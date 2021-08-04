using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //������ ���������� �����

    [SerializeField] private GameObject explosionSmall;             //������ ��� ������������
    private float delayToDestroy = 3f;                              //�������� ����� ������������ ������

    private void OnCollisionEnter(Collision collision)
    {
        GameObject explode = Instantiate(explosionSmall, gameObject.transform.position, Quaternion.identity);            //���� ������������ � ��� ����, �� ������� �����
        Destroy(gameObject);                                                                                             //���������� ������
        Destroy(explode, delayToDestroy);                                                                                //���������� ����� ����� ��������
    }
}
