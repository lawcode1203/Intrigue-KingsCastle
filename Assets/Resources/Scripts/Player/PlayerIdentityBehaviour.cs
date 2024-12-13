using UnityEngine;

public class PlayerIdentityBehaviour : MonoBehaviour
{
    public int characterId = 0;
    private GameManagerBehaviour gameManager;

    void checkForSelfDeath(){
        // Go through all characters and their conspiracies, and see if self is the target of any of them
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject character in characters){
            ConspiracyBehaviour conspiracy = character.GetComponent<ConspiracyBehaviour>();
            if (conspiracy.isInConspiracy && conspiracy.conspiracy.targetid == 0){
                Debug.Log("Self is the target of a conspiracy by " + character.name);
                // Distance between self and conspiracy target is too close
                if (Vector3.Distance(transform.position, character.transform.position) < 4f){
                    gameManager.gameEnd();
                }
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
