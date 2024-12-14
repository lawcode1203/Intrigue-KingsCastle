using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Collections;
public class Character{
    public string name;
    public int characterId;
    public int positionId;
    public bool isMale;
}

public class GameManagerBehaviour : MonoBehaviour
{
    public int numCharacters = 5;
    public AnnouncementManagerBehaviour announcementManager;
    public PositionManagerBehaviour positionManager;
    private StartScreenBehaviour sScreenBehaviour;
    public List<Character> characters = new List<Character>();
    public bool hasGameStarted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sScreenBehaviour = GameObject.Find("GameStartScreen").GetComponent<StartScreenBehaviour>();

        announcementManager = GameObject.Find("AnnouncementManager").GetComponent<AnnouncementManagerBehaviour>();
        positionManager = GameObject.Find("PositionManager").GetComponent<PositionManagerBehaviour>();
    }

    public Vector3 getRandomSpawnPosition(){
        // Get all GameObjects tagged "SpawnLocation"
        GameObject[] spawnLocations = GameObject.FindGameObjectsWithTag("SpawnLocation");
        // Get a random spawn location
        GameObject spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
        return spawnLocation.transform.position;
    }


    public void createUserPlayer(string characterName){
        Character playerCharacter = new Character();
        playerCharacter.name = characterName;
        playerCharacter.characterId = 0;
        playerCharacter.positionId = positionManager.getNumPositions() - 1;
        playerCharacter.isMale = true;
        characters.Add(playerCharacter);
        
        // Load the "Prefabs/PlayerCharacter" prefab
        GameObject playerCharacterObject = Instantiate(Resources.Load("Prefabs/PlayerCharacter"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    }

    public void startGame(){
        createUserPlayer(sScreenBehaviour.playerName);
        spawnStartingCharacters();
        StartCoroutine(waitFourSecondsAndSetHasGameStarted());
    }

    IEnumerator waitFourSecondsAndSetHasGameStarted(){
        yield return new WaitForSeconds(4f);
        hasGameStarted = true;
    }

    public int getRandomCharacterId(){
        return Random.Range(0, numCharacters);
    }

    public int getNumPlayers(){
        return numCharacters;
    }

    public string getCharacterFullName(int characterId){
        Character character = characters[characterId];
        string prefix = positionManager.getGenderVersion(character.positionId, character.isMale);
        return prefix + " " + character.name;
    }

    public void notifyNaturalCharacterDeath(int characterId){
        string name = getCharacterFullName(characterId);
        announcementManager.makeAnnouncement(name + " has died of natural causes.");

        // Get all gameObjects tagged with "character"
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        for (int i = 0; i < characters.Length; i++){
            characters[i].GetComponent<IdentityBehaviour>().wipeIntrigue(characterId);
        }

        spawnCharacter(characterId);
    }

    public void spawnCharacter(int characterId){
        if (characterId >= characters.Count){
            Character character = new Character();
            bool isMale = Random.Range(0, 2) == 0;
            character.name = getRandomCharacterName(isMale);
            character.characterId = characterId;
            character.positionId = Random.Range(0, positionManager.getNumPositions());
            character.isMale = isMale;
            characters.Add(character);
        } else {
            Character character = characters[characterId];
            bool isMale = character.isMale;
            character.name = getRandomCharacterName(isMale);
            character.positionId = Random.Range(0, positionManager.getNumPositions());
            character.isMale = isMale;
            characters[characterId] = character;
        }

        createAndAssignCharacter(characters[characterId]);
        
    }

    public string getRandomCharacterName(bool isMale){
        // Open "characterNames.txt" and read it line by line, adding each line to the list
        List<string> lines = new List<string>();
        System.IO.StreamReader fileReader = new System.IO.StreamReader("Assets/Resources/characterNamesMale.txt");
        if (isMale){
            fileReader = new System.IO.StreamReader("Assets/Resources/characterNamesMale.txt");
        } else {
            fileReader = new System.IO.StreamReader("Assets/Resources/characterNamesFemale.txt");
        }
        while (!fileReader.EndOfStream){
            lines.Add(fileReader.ReadLine());
        }
        fileReader.Close();
        // Return a random line from the list
        return lines[Random.Range(0, lines.Count)];
    }

    private void createAndAssignCharacter(Character character){
        // Spawn a new GameObject using a prefab
        Vector3 spawnPoint = getRandomSpawnPosition();
        GameObject characterObject = Instantiate(Resources.Load("Prefabs/DemoCharacter"), spawnPoint, Quaternion.identity) as GameObject;
        characterObject.name = character.name;
        characterObject.tag = "Character";

        // Get the IdentityBehaviour from the GameObject
        IdentityBehaviour identityBehaviour = characterObject.GetComponent<IdentityBehaviour>();
        identityBehaviour.characterId = character.characterId;
        identityBehaviour.characterName = character.name;
        identityBehaviour.positionId = character.positionId;
        identityBehaviour.isMale = character.isMale;

        // Set image as the sprite of the correct gender. Male is: pixil-male, female is: pixil-female
        SpriteRenderer spriteRenderer = characterObject.GetComponent<SpriteRenderer>();
        if (character.isMale){
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/pixil-male");
        } else {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/pixil-female");
        }

        announcementManager.makeAnnouncement(getCharacterFullName(character.characterId) + " has joined the court.");
    }
    
    public void spawnStartingCharacters(){
        for (int i = 1; i < numCharacters; i++){
            spawnCharacter(i);
        }
        
    }

    public GameObject getGameObjectFromCharacterId(int characterId){
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject character in characters){
            IdentityBehaviour identityBehaviour = character.GetComponent<IdentityBehaviour>();
            if (identityBehaviour.characterId == characterId){
                return character;
            }
        }
        return null;
    }

    public int getCharacterIdFromGameObject(GameObject character){
        IdentityBehaviour identityBehaviour = character.GetComponent<IdentityBehaviour>();
        return identityBehaviour.characterId;
    }

    public int getCharacterIdFromName(string gameObjectName){
        for (int i = 0; i < characters.Count; i++){
            if (characters[i].name == gameObjectName){
                return i;
            }
        }
        Debug.Log("Hit incorrect character name: " + gameObjectName);
        Debug.Log("getCharacterIdFromName failed");
        return -1;
    }

    public void killCharacter(int characterId){
        GameObject character = getGameObjectFromCharacterId(characterId);
        announcementManager.makeAnnouncement(getCharacterFullName(characterId) + " has been assassinated.");
        AdvanceCharactersAndRespawn(character);
        Destroy(character);
    }

    public void AdvanceCharactersAndRespawn(GameObject character){
        // Get character details
        int characterId = getCharacterIdFromGameObject(character);
        int characterPosition = characters[characterId].positionId;

        // Find a random character with a lower position
        GameObject newCharacter = getGameObjectFromCharacterId(Random.Range(0, characters.Count));
        if (newCharacter != null && newCharacter.GetComponent<IdentityBehaviour>().positionId < characterPosition)
        {
            int newCharacterId = getCharacterIdFromGameObject(newCharacter);

            // Update positions
            newCharacter.GetComponent<IdentityBehaviour>().positionId = characterPosition;
            characters[newCharacterId].positionId = characterPosition;
        }

        // Respawn the original character
        spawnCharacter(characterId);
    }

    public void checkIfPlayerStillExists(){
        GameObject[] allCharacters = GameObject.FindGameObjectsWithTag("Character");
        bool playerCharacterExists = false;

        foreach (GameObject character in allCharacters){
            IdentityBehaviour identityBehaviour = character.GetComponent<IdentityBehaviour>();
            if (identityBehaviour.isPlayer){
                playerCharacterExists = true;
            }
        }

        if (!playerCharacterExists && hasGameStarted){
            Debug.Log("Game End called, when it should not be called");
            gameEnd();
        }
    }

    public void gameEnd(){
        Debug.Log("Game has ended");
        for (int i = 0; i < characters.Count; i++){
            GameObject character = getGameObjectFromCharacterId(i);
            if (character != null){
                Destroy(character);
            }
        }
        // Set game end to active
        if (hasGameStarted){
            Instantiate(Resources.Load("Prefabs/GameEndScreen"), new Vector3(0, 0, 0), Quaternion.identity);
            hasGameStarted = false;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        checkIfPlayerStillExists();
    }
}
