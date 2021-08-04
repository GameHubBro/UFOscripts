using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //скрипт управления пулей

    [SerializeField] private GameObject explosionSmall;             //вызрыв при столкновении
    private float delayToDestroy = 3f;                              //задержка перед уничтожением взрыва

    private void OnCollisionEnter(Collision collision)
    {
        GameObject explode = Instantiate(explosionSmall, gameObject.transform.position, Quaternion.identity);            //если сталкивается с чем либо, то создаем взрыв
        Destroy(gameObject);                                                                                             //уничтожаем ракету
        Destroy(explode, delayToDestroy);                                                                                //уничтожаем взрыв после задержки
    }
}
