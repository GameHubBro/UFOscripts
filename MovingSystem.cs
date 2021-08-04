using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSystem : MonoBehaviour
{
    //������ ���������� ��������� �������������� ����������

    [SerializeField] private Transform[] points;                    //������ ����� ����� �������� �������� ������
    [SerializeField] private Transform obj;                         //������ �� ���������� ������

    [SerializeField] private bool cycle = false;                    //���������� ������������� ��������
    [SerializeField] private bool isLookingToPoint = false;         //���������� �� ������� �������� �� ����� 

    [SerializeField] private float speed;                           //�������� �������� �������

    private Transform targetPos;                                    //���������� ����� ���� ���������� ��������� �������
    private int currentPoint;                                       //���������� ������� �����
    private bool isMovingForward = true;                            //���������� ���������� �������� �� ������ ������ ��� �����
    

    private void Awake()
    {
        currentPoint = 0;                                           //����������� ������� ����� �������� 0
        targetPos = points[currentPoint];                           //����������� ������� ���� ����� ��������� ������� ������� ������ �����
    }

    private void Update()
    {
            if (obj != null)                                        //������� � ������� ���� ���� ������
            {
                if (obj.position == targetPos.position)             //���� ������ ������� �� ����� � ������� ���
                {
                    if (!cycle)                                     //�� ���� � �����, ��
                    {
                        if (isMovingForward)                        //���� �������� ������, �� ��������� ����� - ��������� 
                        {
                            currentPoint++;                         
                        }
                        else                                            //���� �������� ������, �� ��������� ����� - ���������� 
                        {
                            currentPoint--;
                        }
                    }
                    else                                            //���� �� � �����, ��
                    {
                        if (currentPoint >= points.Length - 1)      //���� ������� ����� ���������, �� ��������� ���������� ������
                        {
                            currentPoint = 0;
                        }
                        else                                        //���� ������� ����� �� ���������, �� ��������� ���������� ���������
                        {
                            currentPoint++;
                        }
                    }
                    targetPos = points[currentPoint];               //����������� ����� ���� ���������� ��������� ������� - ����� ��������� �����
                }

               
                    obj.position = Vector3.MoveTowards(obj.position, targetPos.position, speed * Time.deltaTime);               //������� ������ �� ����������� � �����
                
            
                if (isLookingToPoint)                       //���� ������� �� ����, �� ������ �������������� �� ����
                {
                    obj.LookAt(targetPos);
                }

                ChangeDirection();                          
            }
    }

    private void ChangeDirection()
    {
            if (currentPoint <= 0)                                  //���� ����� �� ���������, �� ���� ������
            {
                isMovingForward = true;
            }
            else if (currentPoint >= points.Length - 1)             //���� ����� ���������, �� ���������������
            {
                isMovingForward = false;
            }
    }


}
