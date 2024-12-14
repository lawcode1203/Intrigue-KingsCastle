using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class Conversation{
    public int speakerId;
    public int responderId;
    public ConversationType type;
    public int thirdPersonId;
    public float floatData;
}

public class ConversationBehaviour : MonoBehaviour
{
    public float conversationDistance = 2f;
    public float conversationDelay = 8f;
    private StringManagerBehaviour stringManager;
    private IdentityBehaviour identityBehaviour;
    private GameManagerBehaviour gameManager;
    private IntrigueBehaviour intrigueBehaviour;
    private VariableBehaviour variableBehaviour;
    private ConspiracyBehaviour conspiracyBehaviour;
    public TextMeshPro conversationText;
    public bool inConversation = false;
    private float timeSinceLastConversation = 0f;

    void Start()
    {
        stringManager = GameObject.Find("StringManager").GetComponent<StringManagerBehaviour>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        // IdentityBehaviour is attached to the same GameObject as ConversationManager
        identityBehaviour = GetComponent<IdentityBehaviour>();
        intrigueBehaviour = GetComponent<IntrigueBehaviour>();
        variableBehaviour = GetComponent<VariableBehaviour>();
        conspiracyBehaviour = GetComponent<ConspiracyBehaviour>();
        conversationText.gameObject.SetActive(false);
        
    }

    public GameObject getNearestCharacterToSelf(){
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        // Remove self from list of characters
        characters = characters.Where(c => c.GetComponent<IdentityBehaviour>().characterId != identityBehaviour.characterId).ToArray();

        GameObject nearestCharacter = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject character in characters){
            float distance = Vector3.Distance(character.transform.position, transform.position);
            if (distance < minDistance){
                minDistance = distance;
                nearestCharacter = character;
            }
        }
        return nearestCharacter;
    }

    public GameObject getAvailableCharacter(){
        GameObject possibleCharacter = getNearestCharacterToSelf();
        if (possibleCharacter == null){
            return null;
        }
        if (Vector3.Distance(possibleCharacter.transform.position, transform.position) < conversationDistance){
            if (possibleCharacter.GetComponent<ConversationBehaviour>().getInConversation() == false){
                return possibleCharacter;
            }
        }
        return null;
    }

    public void tryStartConversation(){
        GameObject character = getAvailableCharacter();
        if (character != null && !inConversation && character.GetComponent<ConversationBehaviour>().getInConversation() == false && conspiracyBehaviour.getIsInConspiracy()){
            if (identityBehaviour.characterId > character.GetComponent<IdentityBehaviour>().characterId){
                timeSinceLastConversation = 0f;
                Conversation conversation = new Conversation();
                conversation.speakerId = identityBehaviour.characterId;
                conversation.responderId = character.GetComponent<IdentityBehaviour>().characterId;
                conversation.type = ConversationType.Conspiracy;
                conversation.thirdPersonId = conspiracyBehaviour.conspiracy.targetid; // Target of the conspiracy
                conversation.floatData = 0f;
                character.GetComponent<ConversationBehaviour>().processConversation(conversation, character);
            }
            
            
        }

        else if (character != null && !inConversation && character.GetComponent<ConversationBehaviour>().getInConversation() == false){
            // If my character id > the character id of the character I want to talk to
            if (identityBehaviour.characterId > character.GetComponent<IdentityBehaviour>().characterId){
                timeSinceLastConversation = 0f;
                Conversation conversation = new Conversation();
                conversation.speakerId = identityBehaviour.characterId;
                conversation.responderId = character.GetComponent<IdentityBehaviour>().characterId;
                conversation.type = ConversationType.likingQuestions;
                conversation.thirdPersonId = gameManager.getRandomCharacterId(); // Random third person
                conversation.floatData = 0f;
                character.GetComponent<ConversationBehaviour>().processConversation(conversation, character);
            }
            
        }
        
    }

    public bool getInConversation(){
        return inConversation;
    }

    public void processConversation(Conversation conversation, GameObject senderCharacter){
        //Debug.Log("Character: " + gameManager.getCharacterFullName(senderCharacter.GetComponent<IdentityBehaviour>().characterId) + " is now processing a conversation.");
        //Debug.Log(conversation.speakerId + " and " + conversation.responderId);
        
        
        
        
        inConversation = true;
        
        
        

        if (conversation.type == ConversationType.likingQuestions){
            int thirdPersonId = conversation.thirdPersonId;
            // Calculate how much this character likes the third person
            float liking = intrigueBehaviour.getLiking(conversation.speakerId, thirdPersonId) + variableBehaviour.likingBias;
            Conversation returnConversation = new Conversation();
            returnConversation.speakerId = conversation.responderId;
            returnConversation.responderId = conversation.speakerId;
            returnConversation.type = ConversationType.likingAnswers;
            returnConversation.thirdPersonId = thirdPersonId;
            returnConversation.floatData = liking;
            GameObject receiverCharacter = gameManager.getGameObjectFromCharacterId(conversation.speakerId);
            receiverCharacter.GetComponent<ConversationBehaviour>().processConversation(returnConversation, this.gameObject);

            string possibleConversationText = stringManager.getConversationText(conversation);
            Debug.Log(possibleConversationText);
            Debug.Log("Person: " + gameManager.getCharacterFullName(identityBehaviour.characterId) + " says: " + possibleConversationText);
            conversationText.text = possibleConversationText;
            conversationText.gameObject.SetActive(true);
            
        }

        if (conversation.type == ConversationType.likingAnswers){
            float liking = conversation.floatData;
            // Update intrigue behaviour
            intrigueBehaviour.updateLiking(conversation.speakerId, conversation.thirdPersonId, liking);
            string possibleConversationText = stringManager.getConversationText(conversation);
            Debug.Log(possibleConversationText);
            Debug.Log("Person: " + gameManager.getCharacterFullName(identityBehaviour.characterId) + " says: " + possibleConversationText);
            conversationText.text = possibleConversationText;
            conversationText.gameObject.SetActive(true);
            
        }

        if (conversation.type == ConversationType.Conspiracy){
            int thirdPersonId = conversation.thirdPersonId;
            // Calculate how much this character likes the third person
            float selfLiking = intrigueBehaviour.getLiking(conversation.responderId, conversation.thirdPersonId);
            Conspiracy conspiracy = new Conspiracy();
            conspiracy.targetid = conversation.thirdPersonId;
            bool joinConspiracy = conspiracyBehaviour.shouldJoinConspiracy(conversation.responderId, conspiracy);
            if (joinConspiracy){
                // Agree to join the conspiracy
                Conversation returnConversation = new Conversation();
                returnConversation.speakerId = conversation.responderId;
                returnConversation.responderId = conversation.speakerId;
                returnConversation.type = ConversationType.Affirmative;
                returnConversation.thirdPersonId = thirdPersonId;
                returnConversation.floatData = 0f;
                senderCharacter.GetComponent<ConversationBehaviour>().processConversation(returnConversation, this.gameObject);
                
                conspiracy.conspirators = new int[2];
                conspiracy.conspirators[0] = conversation.responderId;
                conspiracy.conspirators[1] = conversation.speakerId;
                conspiracyBehaviour.conspiracy = conspiracy;
            }   
            else{
                // Refuse to join the conspiracy
                Conversation returnConversation = new Conversation();
                returnConversation.speakerId = conversation.responderId;
                returnConversation.responderId = conversation.speakerId;
                returnConversation.type = ConversationType.Negative;
                returnConversation.thirdPersonId = thirdPersonId;
                returnConversation.floatData = 0f;
                senderCharacter.GetComponent<ConversationBehaviour>().processConversation(returnConversation, this.gameObject);
                
            }
            string possibleConversationText = stringManager.getConversationText(conversation);
            Debug.Log(possibleConversationText);
            Debug.Log("Person: " + gameManager.getCharacterFullName(identityBehaviour.characterId) + " says: " + possibleConversationText);
            conversationText.text = possibleConversationText;
            conversationText.gameObject.SetActive(true);
        }

        if (conversation.type == ConversationType.Affirmative){
            // The speaker has agreed to join the conspiracy
            conspiracyBehaviour.addConspirator(conversation.responderId);
            inConversation = false;
            string possibleConversationText = stringManager.getConversationText(conversation);
            Debug.Log(possibleConversationText);
            Debug.Log("Person: " + gameManager.getCharacterFullName(identityBehaviour.characterId) + " says: " + possibleConversationText);
            conversationText.text = possibleConversationText;
            conversationText.gameObject.SetActive(true);
        }

        if (conversation.type == ConversationType.Negative){
            // The speaker has refused to join the conspiracy
            inConversation = false;
            string possibleConversationText = stringManager.getConversationText(conversation);
            Debug.Log(possibleConversationText);
            Debug.Log("Person: " + gameManager.getCharacterFullName(identityBehaviour.characterId) + " says: " + possibleConversationText);
            conversationText.text = possibleConversationText;
            conversationText.gameObject.SetActive(true);
        }

        // Wait 5 seconds, then hide the text
        StartCoroutine(HideTextAfterDelay(5f)); // Replace 5f with your desired delay in seconds
    }

    public void conversationSearch(){
        timeSinceLastConversation += Time.deltaTime;
        if (timeSinceLastConversation > conversationDelay){
            tryStartConversation();
        }
    }

    IEnumerator HideTextAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        conversationText.gameObject.SetActive(false);
        inConversation = false;
    }


    void Update()
    {
        conversationSearch();
    }
    
}
