using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UFO : MonoBehaviour
{
    //������ ���������� ���

    [SerializeField] private Rigidbody leftTurboRB;                                         //������ �� RigidBody ����� �������
    [SerializeField] private Rigidbody rightTurboRB;                                        //������ �� RigidBody ������ �������

    [SerializeField] private int humansToCatch = 0;                                         //���������� ������� ����� ���������� �������

    [SerializeField] private float speed = 15f;                                             //�������� ���
    [SerializeField] private float powerReduceForTurn = 0.8f;                               //���� ���������� ���� ����� ������� ��� ��������
    [SerializeField] private float turboParticleMultiplier = 1.5f;                          //�������� ���������� ���� ������� ������ �� ������ ��� ���������
    [SerializeField] private float maxHighPos = 35f;                                        //�������� ������������ ������

    [SerializeField] private SceneLoader sceneLoader;                                       //������ �� ������ ���������� ������ SceneLoader

    [SerializeField] private Slider leftSlider;                                             //������ �� ������� ����� �������
    [SerializeField] private Slider rightSlider;                                            //������ �� ������� ������ �������
    [SerializeField] private Slider LoadingBar;                                             //������ �� ������� �������� ��������� ������
    [SerializeField] private Slider highSlider;                                             //������ �� ������� ������

    [SerializeField] private Text leftText;                                                 //������ �� ����� ���� ����� �������
    [SerializeField] private Text rightText;                                                //������ �� ����� ���� ������ �������
    [SerializeField] private Text highText;                                                 //������ �� ����� ������ ���
    [SerializeField] private Text humanText;                                                //������ �� ����� ������� ����� �������

    [SerializeField] private ParticleSystem leftTurboParticle;                              //������ �� ������� ������ ����� �������
    [SerializeField] private ParticleSystem rightTurboParticle;                             //������ �� ������� ������ ������ �������
    [SerializeField] private ParticleSystem finishPosParticle;                              //������ �� ������� ������ ��������� ���� ����� ������

    [SerializeField] private GameObject explosion;                                          //������ �� �����
    [SerializeField] private GameObject[] objs;                                             //������ �������� �� ������� ����� ����������� ��� ��� ������������

    [SerializeField] private Transform speedDot;                                            //������ �� ����� ����������� ����������� �������� ���

    [SerializeField] private AudioSource engineSound;                                       //���� ����������
    [SerializeField] private AudioSource explosionSound;                                    //���� ������
    [SerializeField] private AudioSource[] hitsSound;                                       //������ ������ ������
    [SerializeField] private AudioSource[] screamsSound;                                    //������ ������ �����

    private bool isLoadingNextLevel = false;                                                //���������� ����������� ��������� �� ��������� �������
    private float delay = 2f;                                                               //�������� �������� ���������� ������
    private Coroutine startCoroutine;                                                       //�������� �������� ���������� ������

    public static int humansCaught = 0;                                                     //���-�� ��������� �����

    public static bool isDead = false;                                                      //���������� ����� ��� ���



    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();                                      //������� ������ � ����� SceneLoader � ���������� � ����������

        leftSlider.maxValue = speed;                                                        //������������� ������������ �������� ��� ��������� ������, ������ � �������� ����. ������
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

        humanText.text = $"{humansCaught} / {humansToCatch}";                               //������ ����� � ����������� �� ��������� � ����������� ��� ������ �����

        if (humansCaught >= humansToCatch)
        {
            finishPosParticle.Play();                                                       //���� ������� ���������� �����, �� �������� ��������� ��������� ���� ����� ������������
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isDead)                                                                        //���� ���������� � ���-�� � �� ����� �� ������� � �������
        {
            int i = UnityEngine.Random.Range(0, hitsSound.Length);                          //��������� ������� �������� ���� ������������
            hitsSound[i].Play();

            if (collision.gameObject.CompareTag("Friend"))                                  //���� ��������� � ����� Friend 
            {
                if (humansCaught >= humansToCatch)                                          //� ���� ������ ���������� �����, ��
                {
                    startCoroutine = StartCoroutine(sceneLoader.NextLevel(delay));          //��������� �������� �������� ���� ������
                    isLoadingNextLevel = true;                                              
                }
                else
                {
                    Animator humanTextAnimator = humanText.GetComponent<Animator>();        //������������ ����� �����, ���� ������ �� ���������� ��� �������� �� ����. �������
                    humanTextAnimator.SetTrigger("NotReady");
                }
            }

            if (collision.gameObject.CompareTag("Enemy"))                                   //���� ���������� � ������ - �������
                Die();

            if (collision.gameObject.CompareTag("Finish"))                                  //���� ��������� � ����� Finish, �� ��������� �������� ������
            {
                startCoroutine = StartCoroutine(sceneLoader.Win(delay));
                isLoadingNextLevel = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Friend") && humansCaught >= humansToCatch)     //���� ������ ���������� ����� � ������� �� ���� Friend, �� ������������� �������� �������� ����. ������
        {
            if (startCoroutine != null)
                StopCoroutine(startCoroutine);

            isLoadingNextLevel = false;
        }
    }

    private void LeftAndRightTurboController()
    {
        if (gameObject.transform.position.y <= maxHighPos)                                  //���� ������ ��� ������ ������������, �� ������� � �������
        {
            Vector3 minForce = Vector3.up * speed * powerReduceForTurn;                     //������������� ����������� � ������������ �������� ���� ������
            Vector3 maxForce = Vector3.up * speed;

            Vector3 leftForce = Vector3.zero;                                               //������������� ���� �������� 0
            Vector3 rightForce = Vector3.zero;

            if (Input.GetKey(KeyCode.LeftArrow))                                            //��� ������� ����� ������� �������� ������ ������� �� ����, � ����� �� ���.
            {
                leftForce = minForce;
                rightForce = maxForce;
                EngineVolumeUp();                                                           //�������� ���� ������
            }
            else if (Input.GetKey(KeyCode.RightArrow))                                      //��� ������� ������ ������� �������� ����� ������� �� ����, � ������ �� ���.
            {
                leftForce = maxForce;
                rightForce = minForce;
                EngineVolumeUp();                                                           //�������� ���� ������
            }
            else if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))          //��� ������� ������� ����� ��� ������ ��� ������� �������� �� ��������
            {   
                leftForce = maxForce;
                rightForce = maxForce;
                EngineVolumeUp();                                                           //�������� ���� ������
            }
            else
            {
                EngineVolumeDown();                                                         //���� ������ �� ������ ��������� ���� ������
            }

            if (!isDead)                                                                    //���� �� �����, �� ������� � �������
            {
                leftTurboRB.AddRelativeForce(leftForce);                                      //������� ����� ������� �� �������� ������ ���� ����� �������
                rightTurboRB.AddRelativeForce(rightForce);                                    //������� ������ ������� �� �������� ������ ���� ������ �������

                leftSlider.value = Mathf.Lerp(leftSlider.value, leftForce.y, 0.15f);                //������ ���������� �������� �������� ����� �������
                rightSlider.value = Mathf.Lerp(rightSlider.value, rightForce.y, 0.15f);             //������ ���������� �������� �������� ������ �������

                leftText.text = (leftForce.y * 100) / maxForce.y + "%";                             //������ �������� ������ ����� ������� � %
                rightText.text = (rightForce.y * 100) / maxForce.y + "%";                           //������ �������� ������ ������ ������� � %

                var mainLeft = leftTurboParticle.main;                                              //����������� ���������� �������� ������� ������ ������
                var mainRight = rightTurboParticle.main;

                if (leftForce.y >= 0)                                                                   //���� ���� �������������� � ��������, �� ����������� �������� ������ � ������� ������ ������
                    mainLeft.startSpeed = (leftForce.y * turboParticleMultiplier) / maxForce.y;

                if (rightForce.y >= 0)
                    mainRight.startSpeed = (rightForce.y * turboParticleMultiplier) / maxForce.y;
            }
            else
            {                                                            //���� �����, �� �������� ��������� � ������ �� ����� ����������
                leftSlider.value = 0f;
                rightSlider.value = 0f;

                leftText.text = "0%";
                rightText.text = "0%";
            }
        }
    }

    private void EngineVolumeUp()
    {
        engineSound.volume = Mathf.Lerp(engineSound.volume, 0.5f, 0.5f);                        //����������� ���� ������
    }

    private void EngineVolumeDown()                                                             //��������� ���� ������
    {
        engineSound.volume = Mathf.Lerp(engineSound.volume, 0.25f, 0.5f); ;
    }

    private void SpeedDotRotation()
    {
        if (!isDead)                                                                                            //���� �� �����
        {
            Vector3 angles = new Vector3(0, 0, gameObject.transform.eulerAngles.z);                             //����������� ������ ������� �������� ������� �������, �� � �������� ������������ x � y

            if (gameObject.transform.eulerAngles.z < 270f && gameObject.transform.eulerAngles.z > 180f)         //���� ���� ������ �� ��������, �� �������� �������������� � 270 ��� 90 ��������. � ����������� �� ���� � ������ ���� �����
                angles = new Vector3(0, 0, 270f);
            else if (gameObject.transform.eulerAngles.z > 90f && gameObject.transform.eulerAngles.z <= 180f)
                angles = new Vector3(0, 0, 90f);

            speedDot.transform.eulerAngles = angles;                                                        //����������� �������� ����� ������ ������� ����
        }
    }

    private void HighSliderChange()
    {
        highSlider.value = gameObject.transform.position.y;                                                 //�������� �������� ������ ����������� ������ ���
        highText.text = Math.Round(highSlider.value, 1) + "m";                                              //��������� �� 1 ����� ����� ������� � ����������� m
    }

    private void LoadingBarValueChanging()
    {
        if (isLoadingNextLevel && LoadingBar.value < delay)                                                 //���� ��������� ���� �������, �� ������ ������ �������� ������� ��������
            LoadingBar.value += delay / (delay * 50);
        else if (!isLoadingNextLevel && LoadingBar.value > 0)                                               //���� �� ���������, �� ������ �������� ��������� �������� ��������
            LoadingBar.value -= delay / (delay / 10 * 50);
    }

    private void Die()
    {
        explosionSound.Play();                                                                              //�������� ���� ������
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);                         //������� �����

        foreach (GameObject obj in objs)                                                                    //������� ������� �� ������� ��������� RigidBody � BoxCollider (���� �� �� ����)
        {
            if (obj.GetComponent<Rigidbody>() == null)
                obj.AddComponent<Rigidbody>();

            if (obj.GetComponent<BoxCollider>() == null)
                obj.AddComponent<BoxCollider>();

            if (obj.GetComponent<FixedJoint>() != null)                                                     //��������� FixedJoint �� ��������
            {
                FixedJoint fix = obj.GetComponent<FixedJoint>();
                fix.breakForce = 1f;
            }
            obj.transform.SetParent(null);                                                                  //������ ������� �� ��������� � �������
        }
        Camera.main.gameObject.AddComponent<CameraShake>();                                                 //��������� � ������ ������ ������ ������
        isDead = true;                                                                                      //���������� ����� ������ � true
        StartCoroutine(sceneLoader.ResetScene(delay));                                                      //��������� �������� ����������� ������
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Human"))          //���� ������� �������� �������, �� ������� � �������
        {
            for (int i = 0; i < screamsSound.Length; i++)                       //���� ���� ������ ��� ������, �� ������� �� �������
            {
                if (screamsSound[i].isPlaying)
                {
                    return;
                }
            }
            int j = UnityEngine.Random.Range(0, screamsSound.Length);           //���� �� ������, �� ����������� ��������� ���� �� ������� ������
            screamsSound[j].Play();
        }
    }

}
