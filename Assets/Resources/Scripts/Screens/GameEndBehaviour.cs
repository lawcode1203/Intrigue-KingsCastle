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
        beingEnded();
        
    }

    private void RemoveAllOtherScreens(){
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects) {
            if (obj.CompareTag("StartScreen")) {
                Debug.Log("Destroying inactive or active StartScreen: " + obj.name);

                // Destroy the GameObject
                Destroy(obj);
            }
        }
    }

    public void beingEnded(){
        Debug.Log("BeingEnded");
        // format the text
        float time = timeManager.getTime();
        string endText = this.endText.Replace("{0}", timeManager.getTime().ToString());
        endText = endText.Replace("{1}", (time/60.0f).ToString());
        endTextObject.text = endText;

        RemoveAllOtherScreens();

    }

    public void checkIfPlayerStillExists(){
        GameObject playerCharacter = GameObject.Find("PlayerCharacter");
        if(playerCharacter == null){
            beingEnded();
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
