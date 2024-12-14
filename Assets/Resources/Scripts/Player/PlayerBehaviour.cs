using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;
public class PlayerBehaviour : MonoBehaviour
{
    public float speed = 1f;
    public float communicationRange = 4f;
    private GameObject clickedObjectPublic;
    private GameManagerBehaviour gameManager;
    private IdentityBehaviour playerIdentity;
    private StringManagerBehaviour stringManager;
    private float timeSinceLastKeyInput = 0f;
    
    private TextMeshPro speakingText;

    
    // Movement region
    #region Movement
    public void moveUsingInput(){
        if (Input.GetKey("w")){
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        if (Input.GetKey("s")){
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
        if (Input.GetKey("a")){
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey("d")){
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
    }
    #endregion

    public GameObject getNearestCharacter(){
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        // Get rid of self from list of characters
        characters = characters.Where(c => c.GetComponent<IdentityBehaviour>().characterId != playerIdentity.characterId).ToArray();
        GameObject nearestCharacter = null;
        float nearestDistance = Mathf.Infinity;
        foreach (GameObject character in characters){
            float distance = Vector3.Distance(character.transform.position, transform.position);
            if (distance < nearestDistance){
                nearestDistance = distance;
                nearestCharacter = character;
            }
        }
        Debug.Log("Nearest character: " + nearestCharacter.name);
        return nearestCharacter;
    }

    private void askAboutCharacter(){
        speakingText.gameObject.SetActive(true);
         // Get the nearest character tagged "Character"
        GameObject character = getNearestCharacter();
        Debug.Log("Asking about character " + character.name);
        int characterId = gameManager.getCharacterIdFromGameObject(character);
        waitGetInput("Enter the name of the Noble you want to ask about: ");
        int targetId = gameManager.getRandomCharacterId();

        speakingText.text = stringManager.buildResponse(ConversationType.likingQuestions,
            gameManager.getCharacterFullName(characterId), gameManager.getCharacterFullName(playerIdentity.characterId), gameManager.getCharacterFullName(targetId), "low");

        Conversation conversation = new Conversation();
        conversation.speakerId = playerIdentity.characterId;
        conversation.responderId = characterId;
        conversation.type = ConversationType.likingAnswers;
        conversation.thirdPersonId = targetId;
        conversation.floatData = 0f;
        character.GetComponent<ConversationBehaviour>().processConversation(conversation, this.gameObject);

        HideTextAfterDelay(5f);

    }

    private void waitGetInput(string prompt){
        Debug.Log(prompt);
    }

    IEnumerator HideTextAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        speakingText.gameObject.SetActive(false);
    }

    private bool existsCharacterInRange(){
        GameObject closestCharacter = getNearestCharacter();
        float distance = Vector3.Distance(closestCharacter.transform.position, transform.position);
        if (distance < communicationRange){
            return true;
        }
        return false;
    }

    private void keyInputRouting(){
        // On keypress of "q" route to ask about character
        if (Input.GetKey("q") && timeSinceLastKeyInput > 2.5f){
            askAboutCharacter();
            timeSinceLastKeyInput = 0f;
        }
        if (Input.GetKey("k") && timeSinceLastKeyInput > 4.0f && existsCharacterInRange()){
            showFloatingDagger();
            gameManager.killCharacter(gameManager.getCharacterIdFromGameObject(getNearestCharacter()));
            timeSinceLastKeyInput = 0f;
        }
    }

    private void showFloatingDagger(){
        GameObject dagger = Instantiate(Resources.Load("Prefabs/DaggerIcon"), transform.position, Quaternion.identity) as GameObject;
        DaggerIconBehaviour daggerBehaviour = dagger.GetComponent<DaggerIconBehaviour>();
        daggerBehaviour.headGameObject = this.gameObject;
        Destroy(dagger, 4f);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        playerIdentity = GetComponent<IdentityBehaviour>();
        stringManager = GameObject.Find("StringManager").GetComponent<StringManagerBehaviour>();
        speakingText = GameObject.Find("PlayerSpeakingText").GetComponent<TextMeshPro>();
        playerIdentity.isPlayer = true;
        speakingText.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // Set rotation to up
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        moveUsingInput();
        keyInputRouting();
        timeSinceLastKeyInput += Time.deltaTime;
    }
}
