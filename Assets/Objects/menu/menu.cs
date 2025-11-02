using UnityEngine;

public class menu : MonoBehaviour
{
    public GameObject menuInterface;
    public GameObject Score;
    public GameObject credit;
    public GameObject intro;
    public Transform player;
    
    void Awake()
    {

        
    }

    void Update()
    {
        
    }

    public void StartGame()
    {
        
        Vector3 spawnPosition = new Vector3(-10427f,-54f,0f);
        player.position = spawnPosition;
        menuInterface.SetActive(false);
        Score.SetActive(true);
        
        
    }

    public void StartCine()
    {
        
        menuInterface.SetActive(false);
        intro.SetActive(true);
        
        
    }

    public void CreditStart()
    {
        credit.SetActive(true);
        menuInterface.SetActive(false);
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartMenu()
    {
        credit.SetActive(false);
        menuInterface.SetActive(true);
        
    }
}
