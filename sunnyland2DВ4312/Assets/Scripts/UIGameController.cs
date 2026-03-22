using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameController : MonoBehaviour
{
    public Text scoreText;
    public Text lifesText;
    public GameObject pausePanel;
    public PlayerController player;

    private bool isPause;

    void Start()
    {
        isPause = false;
    }

    
    void Update()
    {
        if (player.HP <= 0) Restart();
        
        scoreText.text = $"{player.Score}";
        lifesText.text = $"{player.HP}";

        pausePanel.SetActive(isPause); //включить или выключить паузу
        
        if (isPause) Time.timeScale = 0;
        else Time.timeScale = 1;

 //инвертирование занчения, было true станет false и наоборот
        if (Input.GetKeyDown(KeyCode.Escape)) isPause = !isPause;
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Close()
    {
        isPause = false;
    }
}
