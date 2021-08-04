using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliController : MonoBehaviour
{   //скрипт управлени€ вертолетом

    [SerializeField] private GameObject bullet;                                     //ссылка на объект ракета
    [SerializeField] private GameObject explosion;                                  //ссылка на взрыв вертолета (при касании ЌЋќ)
    [SerializeField] private Transform heliArm;                                     //ссылка на пропеллер вертолета
    [SerializeField] private float delayToShoot = 2f;                               //задержка между выстрелами
    [SerializeField] private float speed = 100f;                                    //скорость выстрела
    [SerializeField] private float armSpeed = 5f;                                   //скорость пропеллера
    [SerializeField] private AudioSource bulletStartSound;                          //ссылка на звук пуска ракеты

    private float delayToDestroyBullet = 8f;                                        //врем€ по истечении которого ракета уничтожитс€
    private Coroutine shootingCoroutine;                                            //курутина стрельбы

    private void Start()
    {
        shootingCoroutine = StartCoroutine(Shooting(delayToShoot));                 //запускаем курутину стрельбы
    }

    private void Update()
    {
        if (heliArm != null)
        {
            heliArm.Rotate(new Vector3(0, armSpeed * Time.deltaTime, 0));           //крутим пропеллер
        }
    }

    IEnumerator Shooting(float delayToShoot)
    {
        while (true)                                                                                                                //бесконечный цикл стрельбы
        {
            GameObject bul = Instantiate(bullet, gameObject.transform.position, Quaternion.Euler(new Vector3(0, -90, 0)));          //создаем ракету
            bulletStartSound.Play();                                                                                                //включаем звук запуска ракеты
            Rigidbody rbBul = bul.GetComponent<Rigidbody>();                                                                        //записываем в переменную RigidBody ракеты
            rbBul.AddForce(Vector3.forward * speed * Time.fixedDeltaTime);                                                          //добавл€ем RigidBody силу полета
            Destroy(bul, delayToDestroyBullet);                                                                                     //уничтожаем пулю через заданное врем€
            yield return new WaitForSeconds(delayToShoot);                                                                          //ждем врем€ до следующего пуска
        }
    }

    private void OnCollisionEnter(Collision collision)                                                                              
    {
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);                                                 //если вертолет с чем то сталкиваетс€, то создаем взрыв
        StopCoroutine(shootingCoroutine);                                                                                           //останавливаем курутину стрельбы
        Destroy(gameObject);                                                                                                        //уничтожаем объект
    }
}
