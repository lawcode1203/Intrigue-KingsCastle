using UnityEngine;
using UnityEngine.UI;

public class TScreenBehaviour : MonoBehaviour
{
    public Button startButton;
    private GameManagerBehaviour gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void loadUp(){
        gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
    }
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        //Hide button and self on start
        startButton.gameObject.SetActive(false);
        gameObject.SetActive(false);

        // Setup events
        startButton.onClick.AddListener(moveToGame);   
    }

    void moveToGame(){
        gameObject.SetActive(false);
        gameManager.startGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
