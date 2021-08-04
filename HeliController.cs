using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliController : MonoBehaviour
{   //������ ���������� ����������

    [SerializeField] private GameObject bullet;                                     //������ �� ������ ������
    [SerializeField] private GameObject explosion;                                  //������ �� ����� ��������� (��� ������� ���)
    [SerializeField] private Transform heliArm;                                     //������ �� ��������� ���������
    [SerializeField] private float delayToShoot = 2f;                               //�������� ����� ����������
    [SerializeField] private float speed = 100f;                                    //�������� ��������
    [SerializeField] private float armSpeed = 5f;                                   //�������� ����������
    [SerializeField] private AudioSource bulletStartSound;                          //������ �� ���� ����� ������

    private float delayToDestroyBullet = 8f;                                        //����� �� ��������� �������� ������ �����������
    private Coroutine shootingCoroutine;                                            //�������� ��������

    private void Start()
    {
        shootingCoroutine = StartCoroutine(Shooting(delayToShoot));                 //��������� �������� ��������
    }

    private void Update()
    {
        if (heliArm != null)
        {
            heliArm.Rotate(new Vector3(0, armSpeed * Time.deltaTime, 0));           //������ ���������
        }
    }

    IEnumerator Shooting(float delayToShoot)
    {
        while (true)                                                                                                                //����������� ���� ��������
        {
            GameObject bul = Instantiate(bullet, gameObject.transform.position, Quaternion.Euler(new Vector3(0, -90, 0)));          //������� ������
            bulletStartSound.Play();                                                                                                //�������� ���� ������� ������
            Rigidbody rbBul = bul.GetComponent<Rigidbody>();                                                                        //���������� � ���������� RigidBody ������
            rbBul.AddForce(Vector3.forward * speed * Time.fixedDeltaTime);                                                          //��������� RigidBody ���� ������
            Destroy(bul, delayToDestroyBullet);                                                                                     //���������� ���� ����� �������� �����
            yield return new WaitForSeconds(delayToShoot);                                                                          //���� ����� �� ���������� �����
        }
    }

    private void OnCollisionEnter(Collision collision)                                                                              
    {
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);                                                 //���� �������� � ��� �� ������������, �� ������� �����
        StopCoroutine(shootingCoroutine);                                                                                           //������������� �������� ��������
        Destroy(gameObject);                                                                                                        //���������� ������
    }
}
