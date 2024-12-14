using UnityEngine;

public class VariableBehaviour : MonoBehaviour
{
    public float baseLiking;// = Random.Range(-0.5f, 0.5f);
    public float positionMultiplier;// = Random.Range(-1.5f, 1.5f);
    public float likingMultiplier;// = Random.Range(-1.5f, 1.5f);
    public float conspiracyThreshold;// = Random.Range(0.0f, 1.0f);
    public float conspiracyMultiplier;// = Random.Range(-1.5f, 1.5f);
    public float likingBias;// = Random.Range(-0.5f, 0.5f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseLiking = Random.Range(-0.5f, 0.5f);
        positionMultiplier = Random.Range(-1.5f, 1.5f);
        likingMultiplier = Random.Range(-1.5f, 1.5f);
        conspiracyThreshold = Random.Range(0.0f, 1.0f);
        conspiracyMultiplier = Random.Range(-1.5f, 1.5f);
        likingBias = Random.Range(-0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
