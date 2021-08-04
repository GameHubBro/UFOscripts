using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    //скрипт управления сценами

    [SerializeField] private GameObject menuPanel;                                  //ссылка на панель меню
    [SerializeField] private Button resumeButton;                                   //ссылка на кнопку "возобновить игру"
    [SerializeField] private Animator winTextAnimator;                              //ссылка на аниматор победного текста

    public IEnumerator NextLevel(float delay)                                       //метод загрузки следующего уровня
    {
        yield return new WaitForSeconds(delay);                                     //задержка
        UFO.humansCaught = 0;                                                       //обнуляем счетчик пойманных людей
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       //загружаем следующую сцену
    }

    public IEnumerator ResetScene(float delay)                                      //метод перезапуска сцены
    {
        yield return new WaitForSeconds(delay);                                     //задержка
        UFO.isDead = false;                                                         //присваиваем значение переменной "не мертв"
        UFO.humansCaught = 0;                                                       //обнуляем счетчик пойманных людей
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);           //загружаем текущую сцену
    }

    public IEnumerator Win(float delay)                                             //метод после победы
    {
        yield return new WaitForSeconds(delay);                                     //задержка
        UFO.humansCaught = 0;                                                       //обнуляем счетчик пойманных людей
        menuPanel.SetActive(true);                                                  //меню настроек делаем активным
        resumeButton.gameObject.SetActive(false);                                   //скрываем кнопку "возобновить игру"
        winTextAnimator.SetBool("isWon", true);                                     //включаем появление текста "Вы выиграли"
    }
}
