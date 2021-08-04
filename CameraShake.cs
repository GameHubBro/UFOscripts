using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //скрипт отвечающий за тряску камеры во время взрыва

    private Transform camTransform;                                         //ссылка на трансформ объекта
    private float shakeDuration = 1.5f;                                     //длительность тряски
    private float shakeAmount = 0.3f;                                       //сила тряски
    private float decreseShaking = 2f;                                      //величина уменьшения тряски со временем
    private Vector3 originPos;                                              //ссылка на изначальную позицию камеры

    private void Start()
    {
        camTransform = GetComponent<Transform>();                           //записываем в переменную трансформ объекта
        originPos = camTransform.position;                                  //записываем в переменную начальную позицию камеры
    }

    private void Update()
    {
        if (shakeDuration > 0)                                                                          //если время тряски больше нуля, то
        {   
            camTransform.position = originPos + Random.insideUnitSphere * shakeAmount;                  //создаем в центре объекта сферу с радиусом shakeAmount и присваиваем камере случайную позицию внутри этой сферы
            shakeDuration -= Time.deltaTime * decreseShaking;                                           //уменьшаем время тряски на время между прорисовками кадра
        }
        else
        {
            shakeDuration = 0;                                                                          //если время тряски не больше 0, то присваиваем этому времени значени 0 и 
            camTransform.position = originPos;                                                          //присваиваем камере начальную позицию
        }
    }

}
