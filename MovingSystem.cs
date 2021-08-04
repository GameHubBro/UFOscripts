using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSystem : MonoBehaviour
{
    //скрипт управлени€ движением второстепенных персонажей

    [SerializeField] private Transform[] points;                    //массив точек между которыми движетс€ объект
    [SerializeField] private Transform obj;                         //ссылка на движущийс€ объект

    [SerializeField] private bool cycle = false;                    //переменна€ зацикленности маршрута
    [SerializeField] private bool isLookingToPoint = false;         //необходимо ли объекту смотреть на точку 

    [SerializeField] private float speed;                           //скорость движени€ объекта

    private Transform targetPos;                                    //переменна€ точки куда необходимо двигатьс€ объекту
    private int currentPoint;                                       //переменна€ текущей точки
    private bool isMovingForward = true;                            //переменна€ означающа€ движетс€ ли объект вперед или назад
    

    private void Awake()
    {
        currentPoint = 0;                                           //присваиваем текущей точке значение 0
        targetPos = points[currentPoint];                           //присваиваем позиции куда нужно двигатьс€ объекту позицию первой точки
    }

    private void Update()
    {
            if (obj != null)                                        //заходим в условие если есть объект
            {
                if (obj.position == targetPos.position)             //если объект доходит до точку в которую шел
                {
                    if (!cycle)                                     //то если в цикле, то
                    {
                        if (isMovingForward)                        //если движетс€ вперед, то следующа€ точка - следующа€ 
                        {
                            currentPoint++;                         
                        }
                        else                                            //если движетс€ вперед, то следующа€ точка - предыдуща€ 
                        {
                            currentPoint--;
                        }
                    }
                    else                                            //если не в цикле, то
                    {
                        if (currentPoint >= points.Length - 1)      //если текуща€ точка последн€€, то следующей становитс€ перва€
                        {
                            currentPoint = 0;
                        }
                        else                                        //если текуща€ точка не последн€€, то следующей становитс€ следующа€
                        {
                            currentPoint++;
                        }
                    }
                    targetPos = points[currentPoint];               //присваиваем месту куда необходимо двигатьс€ объекту - место следующей точки
                }

               
                    obj.position = Vector3.MoveTowards(obj.position, targetPos.position, speed * Time.deltaTime);               //двигаем объект по направлению к точке
                
            
                if (isLookingToPoint)                       //если смотрим на цель, то объект поворачиваетс€ на цель
                {
                    obj.LookAt(targetPos);
                }

                ChangeDirection();                          
            }
    }

    private void ChangeDirection()
    {
            if (currentPoint <= 0)                                  //если точка не последн€€, то идем вперед
            {
                isMovingForward = true;
            }
            else if (currentPoint >= points.Length - 1)             //если точка последн€€, то разворачиваемс€
            {
                isMovingForward = false;
            }
    }


}
