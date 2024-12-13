using UnityEngine;
using TMPro;
public class IdentityBehaviour : MonoBehaviour
{
    public string characterName;
    public int characterId;
    public int positionId;
    public bool isMale;
    public float age;
    public float birthmoment;
    public float timeToNextBirthday;
    public float currentTime;
    public TextMeshPro headFloatText;
    public bool isPlayer = false;

    private GameManagerBehaviour gameManager;
    private TimeManagerBehaviour timeManager;
    private IntrigueBehaviour intrigueBehaviour;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManagerBehaviour>();
        
        // Get self from intrigueBehaviour - If this is the player, intrigueBehaviour will be null
        if (!isPlayer){
            intrigueBehaviour = GetComponent<IntrigueBehaviour>();
            intrigueBehaviour.setIdentity(characterId);
        }
        headFloatText = GetComponentInChildren<TextMeshPro>();
        headFloatText.text = gameManager.getCharacterFullName(characterId);
    }

    private void timeControl(){
        age = timeManager.getTime() - birthmoment;
        float realTime = timeManager.getTime();
        float dt = realTime - currentTime;
        currentTime = realTime;
        timeToNextBirthday = timeToNextBirthday - dt;
    }

    private bool checkIfDeathFromNature(){
        if (timeToNextBirthday <= 0){
            timeToNextBirthday = 31536000.0f;
            return probCheckNaturalDeath(age);
        } else {
            return false;
        }
    }

    private bool probCheckNaturalDeath(float age){
        // Convert age to years
        int ageInYears = (int)(age / 31536000);

        if (ageInYears >= 100){
            return true;
        }

        // Calculate the probability of death based on age
        float deathProbability = 0.1f * ageInYears + 0.1f;

        // Generate a random number between 0 and 1
        float random = Random.Range(0f, 1f);

        // Check if the random number is less than the death probability
        if (random < deathProbability){
            return true;
        } else {
            return false;
        }
    }

    public void wipeIntrigue(int characterId){
        intrigueBehaviour.wipeIntrigue(characterId);
    }

    // Update is called once per frame
    void Update()
    {
        timeControl();
        if (checkIfDeathFromNature()){
            gameManager.notifyNaturalCharacterDeath(characterId);
            Destroy(gameObject);
        }
        if (headFloatText == null){
            headFloatText = GetComponentInChildren<TextMeshPro>();
        }
        headFloatText.text = gameManager.getCharacterFullName(characterId);
        //Debug.Log("Head float text should say: " + gameManager.getCharacterFullName(characterId));
    }
}
