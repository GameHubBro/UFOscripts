using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UFO : MonoBehaviour
{
    //скрипт управления НЛО

    [SerializeField] private Rigidbody leftTurboRB;                                         //ссылка на RigidBody левой турбины
    [SerializeField] private Rigidbody rightTurboRB;                                        //ссылка на RigidBody правой турбины

    [SerializeField] private int humansToCatch = 0;                                         //переменная сколько людей необходимо поймать

    [SerializeField] private float speed = 15f;                                             //скорость НЛО
    [SerializeField] private float powerReduceForTurn = 0.8f;                               //сила уменьшения тяги одной турбины при повороте
    [SerializeField] private float turboParticleMultiplier = 1.5f;                          //значение увеличения силы системы частиц из турбин при ускорении
    [SerializeField] private float maxHighPos = 35f;                                        //значение максимальной высоты

    [SerializeField] private SceneLoader sceneLoader;                                       //ссылка на объект содержащий скрипт SceneLoader

    [SerializeField] private Slider leftSlider;                                             //ссылка на слайдер левой турбины
    [SerializeField] private Slider rightSlider;                                            //ссылка на слайдер правой турбины
    [SerializeField] private Slider LoadingBar;                                             //ссылка на слайдер загрузки следующег уровня
    [SerializeField] private Slider highSlider;                                             //ссылка на слайдер высоты

    [SerializeField] private Text leftText;                                                 //ссылка на текст силы левой турбины
    [SerializeField] private Text rightText;                                                //ссылка на текст силы правой турбины
    [SerializeField] private Text highText;                                                 //ссылка на текст высоты НЛО
    [SerializeField] private Text humanText;                                                //ссылка на текст сколько людей поймано

    [SerializeField] private ParticleSystem leftTurboParticle;                              //ссылка на систему частиц левой турбины
    [SerializeField] private ParticleSystem rightTurboParticle;                             //ссылка на систему частиц правой турбины
    [SerializeField] private ParticleSystem finishPosParticle;                              //ссылка на систему частиц платформы куда нужно лететь

    [SerializeField] private GameObject explosion;                                          //ссылка на взрыв
    [SerializeField] private GameObject[] objs;                                             //массив объектов на которые будет разлетаться НЛО при столкновении

    [SerializeField] private Transform speedDot;                                            //ссылка на точку указывающую направление вращения НЛО

    [SerializeField] private AudioSource engineSound;                                       //звук двигателей
    [SerializeField] private AudioSource explosionSound;                                    //звук взрыва
    [SerializeField] private AudioSource[] hitsSound;                                       //массив звуков ударов
    [SerializeField] private AudioSource[] screamsSound;                                    //массив криков людей

    private bool isLoadingNextLevel = false;                                                //переменная указывающая загружаем ли следующий уровень
    private float delay = 2f;                                                               //задержка загрузки следующего уровня
    private Coroutine startCoroutine;                                                       //курутина загрузки следующего уровня

    public static int humansCaught = 0;                                                     //кол-во пойманных людей

    public static bool isDead = false;                                                      //переменная мертв или нет



    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();                                      //находим объект в сцене SceneLoader и записываем в переменную

        leftSlider.maxValue = speed;                                                        //устанавливаем максимальные значения для слайдеров турбин, высоты и загрузки след. уровня
        rightSlider.maxValue = speed;
        highSlider.maxValue = maxHighPos;
        LoadingBar.maxValue = delay;
    }

    private void FixedUpdate()
    {
        LeftAndRightTurboController();
        SpeedDotRotation();
        HighSliderChange();
        LoadingBarValueChanging();

        humanText.text = $"{humansCaught} / {humansToCatch}";                               //меняем текст в зависимости от пойманных и необходимых для поимки людей

        if (humansCaught >= humansToCatch)
        {
            finishPosParticle.Play();                                                       //если поймали достаточно людей, то включаем подсветку платформы куда нужно приземлиться
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isDead)                                                                        //если столкнулся с чем-то и не мертв то заходим в условие
        {
            int i = UnityEngine.Random.Range(0, hitsSound.Length);                          //случайным образом выбираем звук столкновения
            hitsSound[i].Play();

            if (collision.gameObject.CompareTag("Friend"))                                  //если столкнуся с тегом Friend 
            {
                if (humansCaught >= humansToCatch)                                          //и если поймал достаточно людей, то
                {
                    startCoroutine = StartCoroutine(sceneLoader.NextLevel(delay));          //запускаем курутину загрузки след уровня
                    isLoadingNextLevel = true;                                              
                }
                else
                {
                    Animator humanTextAnimator = humanText.GetComponent<Animator>();        //подсвечиваем текст людей, если поймал не достаточно для перехода на след. уровень
                    humanTextAnimator.SetTrigger("NotReady");
                }
            }

            if (collision.gameObject.CompareTag("Enemy"))                                   //если столкнулся с врагом - умираем
                Die();

            if (collision.gameObject.CompareTag("Finish"))                                  //если стокнулся с тегом Finish, то запускаем курутину победы
            {
                startCoroutine = StartCoroutine(sceneLoader.Win(delay));
                isLoadingNextLevel = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Friend") && humansCaught >= humansToCatch)     //если поймал достаточно людей и уходишь от тега Friend, то останавливаем курутину загрузки след. уровня
        {
            if (startCoroutine != null)
                StopCoroutine(startCoroutine);

            isLoadingNextLevel = false;
        }
    }

    private void LeftAndRightTurboController()
    {
        if (gameObject.transform.position.y <= maxHighPos)                                  //если высота НЛО меньше максимальной, то заходим в условие
        {
            Vector3 minForce = Vector3.up * speed * powerReduceForTurn;                     //устанавливаем минимальное и максимальное значения силы турбин
            Vector3 maxForce = Vector3.up * speed;

            Vector3 leftForce = Vector3.zero;                                               //устанавливаем силу турбинам 0
            Vector3 rightForce = Vector3.zero;

            if (Input.GetKey(KeyCode.LeftArrow))                                            //при нажатии левой стрелке включаем правую турбину на макс, а левую на мин.
            {
                leftForce = minForce;
                rightForce = maxForce;
                EngineVolumeUp();                                                           //включаем звук турбин
            }
            else if (Input.GetKey(KeyCode.RightArrow))                                      //при нажатии правой стрелке включаем левую турбину на макс, а правую на мин.
            {
                leftForce = maxForce;
                rightForce = minForce;
                EngineVolumeUp();                                                           //включаем звук турбин
            }
            else if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))          //при нажатии стрелки вверх или пробел две турбины включаем на максимум
            {   
                leftForce = maxForce;
                rightForce = maxForce;
                EngineVolumeUp();                                                           //включаем звук турбин
            }
            else
            {
                EngineVolumeDown();                                                         //если ничего не нажато уменьшаем звук турбин
            }

            if (!isDead)                                                                    //если не мертв, то заходим в условие
            {
                leftTurboRB.AddRelativeForce(leftForce);                                      //толкаем левую турбина на величину равную силе левой турбины
                rightTurboRB.AddRelativeForce(rightForce);                                    //толкаем правую турбина на величину равную силе правой турбины

                leftSlider.value = Mathf.Lerp(leftSlider.value, leftForce.y, 0.15f);                //меняем постепенно значение слайдера левой турбины
                rightSlider.value = Mathf.Lerp(rightSlider.value, rightForce.y, 0.15f);             //меняем постепенно значение слайдера правой турбины

                leftText.text = (leftForce.y * 100) / maxForce.y + "%";                             //меняем значение текста левой турбины в %
                rightText.text = (rightForce.y * 100) / maxForce.y + "%";                           //меняем значение текста правой турбины в %

                var mainLeft = leftTurboParticle.main;                                              //присваиваем переменным значение системы частиц турбин
                var mainRight = rightTurboParticle.main;

                if (leftForce.y >= 0)                                                                   //если сила прикладывается к турбинам, то увеличиваем скорость частиц в системе частиц турбин
                    mainLeft.startSpeed = (leftForce.y * turboParticleMultiplier) / maxForce.y;

                if (rightForce.y >= 0)
                    mainRight.startSpeed = (rightForce.y * turboParticleMultiplier) / maxForce.y;
            }
            else
            {                                                            //если мертв, то значение слайдеров и текста по нулям выставляем
                leftSlider.value = 0f;
                rightSlider.value = 0f;

                leftText.text = "0%";
                rightText.text = "0%";
            }
        }
    }

    private void EngineVolumeUp()
    {
        engineSound.volume = Mathf.Lerp(engineSound.volume, 0.5f, 0.5f);                        //увеличиваем звук турбин
    }

    private void EngineVolumeDown()                                                             //уменьшаем звук турбин
    {
        engineSound.volume = Mathf.Lerp(engineSound.volume, 0.25f, 0.5f); ;
    }

    private void SpeedDotRotation()
    {
        if (!isDead)                                                                                            //если не мертв
        {
            Vector3 angles = new Vector3(0, 0, gameObject.transform.eulerAngles.z);                             //присваиваем новому вектору значение вектора объекта, но с нулевыми координатами x и y

            if (gameObject.transform.eulerAngles.z < 270f && gameObject.transform.eulerAngles.z > 180f)         //если угол уходит за полукруг, то значение приравнивается к 270 или 90 градусов. в зависимости от того к какому углу ближе
                angles = new Vector3(0, 0, 270f);
            else if (gameObject.transform.eulerAngles.z > 90f && gameObject.transform.eulerAngles.z <= 180f)
                angles = new Vector3(0, 0, 90f);

            speedDot.transform.eulerAngles = angles;                                                        //присваиваем повороту точки вектор данного угла
        }
    }

    private void HighSliderChange()
    {
        highSlider.value = gameObject.transform.position.y;                                                 //значению слайдера высоты присваиваем высоту НЛО
        highText.text = Math.Round(highSlider.value, 1) + "m";                                              //округляем до 1 числа после запятой и приписываем m
    }

    private void LoadingBarValueChanging()
    {
        if (isLoadingNextLevel && LoadingBar.value < delay)                                                 //если загружаем след уровень, то плавно меняем значение слайдер загрузки
            LoadingBar.value += delay / (delay * 50);
        else if (!isLoadingNextLevel && LoadingBar.value > 0)                                               //если не загружаем, то быстро обнуляем значенией слайдера загрузки
            LoadingBar.value -= delay / (delay / 10 * 50);
    }

    private void Die()
    {
        explosionSound.Play();                                                                              //включаем звук взрыва
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);                         //создаем взрыв

        foreach (GameObject obj in objs)                                                                    //каждому объекту из массива добавляем RigidBody и BoxCollider (если их не было)
        {
            if (obj.GetComponent<Rigidbody>() == null)
                obj.AddComponent<Rigidbody>();

            if (obj.GetComponent<BoxCollider>() == null)
                obj.AddComponent<BoxCollider>();

            if (obj.GetComponent<FixedJoint>() != null)                                                     //разрушаем FixedJoint на турбинах
            {
                FixedJoint fix = obj.GetComponent<FixedJoint>();
                fix.breakForce = 1f;
            }
            obj.transform.SetParent(null);                                                                  //делаем объекты не дочерними к объекту
        }
        Camera.main.gameObject.AddComponent<CameraShake>();                                                 //добавляем к камере скрипт тряски камеры
        isDead = true;                                                                                      //переменную мертв ставим в true
        StartCoroutine(sceneLoader.ResetScene(delay));                                                      //запускаем курутину перезапуска уровня
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Human"))          //если триггер коснулся человек, то заходим в условие
        {
            for (int i = 0; i < screamsSound.Length; i++)                       //если звук криков уже играет, то выходим из условия
            {
                if (screamsSound[i].isPlaying)
                {
                    return;
                }
            }
            int j = UnityEngine.Random.Range(0, screamsSound.Length);           //если не играет, то проигрываем случайный звук из массива криков
            screamsSound[j].Play();
        }
    }

}
