using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Player player;
    public Text coinText;
    public Image[] hearts;
    public Sprite isLife, nonLife;
    public GameObject PauseScreen;
    public GameObject WinScreen;
    public GameObject LoseScreen;
    public Soundeffector soundeffector;

    public void ReloadLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Update() 
    {
        coinText.text = player.GetCoins().ToString();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (player.GetHp() > i)
            hearts[i].sprite = isLife;
            else 
            hearts[i].sprite = nonLife;
        }
    }

    public void PauseOn()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        PauseScreen.SetActive(true);
    }

    public void PauseOff()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        PauseScreen.SetActive(false);
    }

    public void Win()
    {
        soundeffector.PlayWinSound();
        Time.timeScale = 0f;
        player.enabled = false;
        WinScreen.SetActive(true);

        if(!PlayerPrefs.HasKey("Lvl") || PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex)
        PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);

        if (PlayerPrefs.HasKey("coins"))
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + player.GetCoins());
        else
        PlayerPrefs.SetInt("coins", player.GetCoins());
        print (PlayerPrefs.GetInt("coins"));
    }

    public void Lose()
    {
        soundeffector.PlayLoseSound();
        Time.timeScale = 0f;
        player.enabled = false;
        LoseScreen.SetActive(true);
    }

    public void MenuLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene("MainMenuAlien");
    }

    public void NextLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
