using UnityEngine;

public class IntrigueBehaviour : MonoBehaviour
{
    public float [,] likingMatrix;
    public float [] powerLevels;

    private VariableBehaviour variableBehaviour;
    private PositionManagerBehaviour positionManagerBehaviour;
    private GameManagerBehaviour gameManagerBehaviour;

    private int characterIdentity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManagerBehaviour = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        positionManagerBehaviour = GameObject.Find("PositionManager").GetComponent<PositionManagerBehaviour>();

        // VariableBehaviour is attached to the same GameObject as IntrigueBehaviour
        // Get the VariableBehaviour from self
        variableBehaviour = GetComponent<VariableBehaviour>();
        
        int numPlayers = gameManagerBehaviour.getNumPlayers();
        likingMatrix = new float[numPlayers, numPlayers];
        for (int i = 0; i < numPlayers; i++){
            for (int j = 0; j < numPlayers; j++){
                likingMatrix[i,j] = variableBehaviour.baseLiking + Random.Range(-0.5f, 0.5f);
            } 
        }

        powerLevels = new float[numPlayers];
        for (int i = 0; i < numPlayers; i++){
            powerLevels[i] = positionManagerBehaviour.getCharacterPositionPower(i) * variableBehaviour.positionMultiplier;
        }
        
        scalePowerLevels();
    }

    private void scalePowerLevels(){
        float sum = 0;
        for (int i = 0; i < powerLevels.Length; i++){
            sum += powerLevels[i];
        }
        for (int i = 0; i < powerLevels.Length; i++){
            powerLevels[i] = powerLevels[i] / sum;
        }
    }

    private void setLikingMatrix(int i, int j, float amount){
        likingMatrix[i,j] = amount;
    }

    private void modifyLikingMatrix(int i, int j, float amount){
        likingMatrix[i,j] += amount;
    }

    private void recalculatePowerLevels(){
        
        for (int i = 0; i < powerLevels.Length; i++){
            powerLevels[i] = calcWeightedPower(i) * variableBehaviour.likingMultiplier + positionManagerBehaviour.getCharacterPositionPower(i) * variableBehaviour.positionMultiplier;
        }
        scalePowerLevels();
    }

    public float getLiking(int i, int j){
        return likingMatrix[i,j];
    }

    private float calcWeightedPower(int i){
        float sum = 0;
        for (int j = 0; j < likingMatrix.GetLength(1); j++){
            sum += likingMatrix[j,i] * powerLevels[j];
        }
        return sum;
    }

    public void wipeIntrigue(int characterId){
        for (int i = 0; i < likingMatrix.GetLength(1); i++){
            likingMatrix[i,characterId] = variableBehaviour.baseLiking;
            likingMatrix[characterId,i] = variableBehaviour.baseLiking;
        }
        recalculatePowerLevels();
    }

    public void updateLiking(int speakerId, int thirdPersonId, float liking){
        modifyLikingMatrix(speakerId, thirdPersonId, liking * variableBehaviour.likingMultiplier);
        recalculatePowerLevels();
    }

    public void setIdentity(int characterId){
        characterIdentity = characterId;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
