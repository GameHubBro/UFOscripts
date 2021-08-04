using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //������ ���������� �� ������ ������ �� ����� ������

    private Transform camTransform;                                         //������ �� ��������� �������
    private float shakeDuration = 1.5f;                                     //������������ ������
    private float shakeAmount = 0.3f;                                       //���� ������
    private float decreseShaking = 2f;                                      //�������� ���������� ������ �� ��������
    private Vector3 originPos;                                              //������ �� ����������� ������� ������

    private void Start()
    {
        camTransform = GetComponent<Transform>();                           //���������� � ���������� ��������� �������
        originPos = camTransform.position;                                  //���������� � ���������� ��������� ������� ������
    }

    private void Update()
    {
        if (shakeDuration > 0)                                                                          //���� ����� ������ ������ ����, ��
        {   
            camTransform.position = originPos + Random.insideUnitSphere * shakeAmount;                  //������� � ������ ������� ����� � �������� shakeAmount � ����������� ������ ��������� ������� ������ ���� �����
            shakeDuration -= Time.deltaTime * decreseShaking;                                           //��������� ����� ������ �� ����� ����� ������������ �����
        }
        else
        {
            shakeDuration = 0;                                                                          //���� ����� ������ �� ������ 0, �� ����������� ����� ������� ������� 0 � 
            camTransform.position = originPos;                                                          //����������� ������ ��������� �������
        }
    }

}
