using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    [SerializeField] private ParticleSystem lowBodyTunnel;                  //ссылка на систему частиц тоннеля поднимающего людей
    [SerializeField] private float upForce = 15f;                           //переменная сила подъёма людей


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HumanCatcher"))
        {
            Destroy(gameObject);                                            //если человек коснулся HumanCather, который на НЛО, то объект удаляется
            if (lowBodyTunnel.isPlaying)
            {
                lowBodyTunnel.Stop();                                       //если тоннель работал, то его останавливаем
            }
            UFO.humansCaught++;                                             //к переменной скрипт UFO прибавляем 1 (означает сколько людей было поймано тарелкой)
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !UFO.isDead)              
        {
            if (lowBodyTunnel.isStopped)
            {
                lowBodyTunnel.Play();                                       //если человек касается триггера Player, то включаем тоннель (если не был включен)
            }                                                               //и поднимаем человека по направлению к тарелке
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, other.gameObject.transform.position, upForce * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !UFO.isDead)                      //если выходим из триггера Player и тоннель был включен, то останавливаем его
        {
            if (lowBodyTunnel.isPlaying)
                lowBodyTunnel.Stop();
        }
    }
}
