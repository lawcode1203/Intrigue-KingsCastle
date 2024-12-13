using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StartScreenBehaviour : MonoBehaviour
{
    public string playerName;
    public Button playButton;
    // Text input field
    public TMP_InputField nameInputField;
    private TScreenBehaviour tScreenBehaviour;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find "TutorialScreen"
        tScreenBehaviour = GameObject.Find("TutorialScreen").GetComponent<TScreenBehaviour>(); 
        // Hide button
        playButton.gameObject.SetActive(false);

        // Setup events
        nameInputField.onEndEdit.AddListener(OnInputFieldSubmit);
        playButton.onClick.AddListener(moveToGame);
        
    }

    // On input field submit, show button
    public void OnInputFieldSubmit(string input)
    {
        Debug.Log("Submitted name: " + input);
        // Set player name
        playerName = input;
        // Show button
        playButton.gameObject.SetActive(true);
    }

    public void moveToGame(){
        gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        nameInputField.gameObject.SetActive(false);
        tScreenBehaviour.loadUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
