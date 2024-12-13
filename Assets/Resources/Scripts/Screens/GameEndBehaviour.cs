using UnityEngine;
using TMPro;
public class GameEndBehaviour : MonoBehaviour
{
    public string endText = "Intrigue - King's Court\nYou have been assassinated! However, you survived for {0} seconds, earning you a score of {1}! Great work!\n\n\nGAME OVER";
    public TextMeshPro endTextObject;
    public TimeManagerBehaviour timeManager;
    public GameManagerBehaviour gameManager;
    private Renderer renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        endTextObject = GetComponentInChildren<TextMeshPro>();
        // Hide self
        endTextObject.text = endText;
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManagerBehaviour>();
        
    }

    public void beingEnded(){
        renderer.enabled = true;
        // format the text
        float time = timeManager.getTime();
        string endText = this.endText.Replace("{0}", timeManager.getTime().ToString());
        endText = endText.Replace("{1}", (time/60.0f).ToString());
        endTextObject.text = endText;
    }

    public void checkIfPlayerStillExists(){
        GameObject playerCharacter = gameManager.getGameObjectFromCharacterId(0);
        if (playerCharacter == null){
            Debug.Log("Being ended");
            beingEnded();
        }
    }
    // Update is called once per frame
    void Update()
    {
        float time = timeManager.getTime();
        string endText = this.endText.Replace("{0}", timeManager.getTime().ToString());
        endText = endText.Replace("{1}", (time/60.0f).ToString());
        endTextObject.text = endText;
    }
}
