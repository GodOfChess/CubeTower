using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButton : MonoBehaviour
{
    public Sprite musicOn, musicOff;

    public void Start()
    {
        //Если у игрока включена преднастройка без музыки, то меняем иконку кнопки Music на выключенную
        if (PlayerPrefs.GetString("music") == "No" && gameObject.name == "Music")
        {
            GetComponent<Image>().sprite = musicOff;
        }
    }

    public void RestartGame()
    {
        //Подгружаем заново ту же самую сцену, при нажатии на кнопку рестарта
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadInstagram()
    {
        //Передаем Url-адрес, при нажатии на кнопку инсты открывает его
        Application.OpenURL("https://www.instagram.com/lie___to___fly/");
    }

    public void LoadShop()
    {
        //Подгружаем сцену магазина, при нажатии на кнопку магазина
        SceneManager.LoadScene("Shop");
    }

    public void CloseShop()
    {
        //Подгружаем сцену игры, при выходе из магазина
        SceneManager.LoadScene("Main");
    }

    public void MusicWork()
    {   //музыка выключена
        if(PlayerPrefs.GetString("music") == "No")
        {
            //Берем музыку, включаем ее, меняем спрайт, меняем предпочтение пользователя
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<Image>().sprite = musicOn;
        }
        // музыка включена
        else
        {
            GetComponent<AudioSource>().Stop();
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
}
