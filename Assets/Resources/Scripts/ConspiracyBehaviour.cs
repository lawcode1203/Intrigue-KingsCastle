using UnityEngine;

public struct Conspiracy{
    public int targetid; // The id of the target
    public int[] conspirators; // The ids of all the conspirators
}

public class ConspiracyBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IntrigueBehaviour intrigueBehaviour;
    private VariableBehaviour variableBehaviour;
    private IdentityBehaviour identityBehaviour;
    private GameManagerBehaviour gameManager;
    public Conspiracy conspiracy;
    public bool isInConspiracy;

    void Start()
    {
        // Get the IntrigueBehaviour from self
        intrigueBehaviour = GetComponent<IntrigueBehaviour>();
        isInConspiracy = false;
        variableBehaviour = GetComponent<VariableBehaviour>();
        identityBehaviour = GetComponent<IdentityBehaviour>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        
    }

    // Update is called once per frame
    public int getTarget(int selfid){
        // Iterate through all players and find the one that selfid likes the least
        float min = intrigueBehaviour.likingMatrix[selfid,0];
        int minid = 0;
        for (int i = 1; i < intrigueBehaviour.likingMatrix.GetLength(1); i++){
            if (intrigueBehaviour.likingMatrix[selfid,i] < min){
                min = intrigueBehaviour.likingMatrix[selfid,i];
                minid = i;
            }
        }
        return minid;
    }

    public void addConspirator(int conspiratorid){
        conspiracy.conspirators = new int[conspiracy.conspirators.Length + 1];
        conspiracy.conspirators[conspiracy.conspirators.Length - 1] = conspiratorid;
    }

    public bool shouldJoinConspiracy(int selfid, Conspiracy conspiracy){
        int targetid = conspiracy.targetid;
        float liking = intrigueBehaviour.likingMatrix[selfid, targetid];

        if (liking < variableBehaviour.conspiracyThreshold && !isInConspiracy){
            return true;
        }
        return false;
    }

    public bool getIsInConspiracy(){
        return isInConspiracy;
    }

    void Update(){
        // There is a 1/10000 chance of starting a conspiracy
        if (Random.Range(0, 10000) == 0 && !isInConspiracy){
            int targetid = getTarget(identityBehaviour.characterId);
            isInConspiracy = true;
            conspiracy = new Conspiracy();
            conspiracy.targetid = targetid;
            conspiracy.conspirators = new int[1];
            conspiracy.conspirators[0] = identityBehaviour.characterId;
            //Debug.Log(gameManager.getCharacterFullName(identityBehaviour.characterId) + " started a conspiracy against " + gameManager.getCharacterFullName(targetid));
        }
    }
}
