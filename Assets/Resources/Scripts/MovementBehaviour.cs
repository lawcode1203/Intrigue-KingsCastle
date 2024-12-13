using UnityEngine;

public enum RotationOptions{
    Up,
    Down,
    Left,
    Right
}

public class MovementBehaviour : MonoBehaviour
{
    // Characters just move around aimlessly, just avoiding walls and each other.
    // They tend to keep moving in a straight line, turning with a small probability.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private ConversationBehaviour conversationBehaviour;
    private ConspiracyBehaviour conspiracyBehaviour;
    public RotationOptions rotationOption = RotationOptions.Up;
    public float speed;
    public float turnChance;
    private GameManagerBehaviour gameManager;
    private void moveAround(){
        moveStraight();
        if (Random.Range(0f, 1f) < turnChance){
            turn();
        }
    }

    private void moveStraight(){
        if (!conversationBehaviour.getInConversation()){
            if (rotationOption == RotationOptions.Up){
                transform.Translate(0f, speed * Time.deltaTime, 0f);
            }
            else if (rotationOption == RotationOptions.Down){
                transform.Translate(0f, -speed * Time.deltaTime, 0f);
            }
            else if (rotationOption == RotationOptions.Left){
                transform.Translate(-speed * Time.deltaTime, 0f, 0f);
            }
            else if (rotationOption == RotationOptions.Right){
                transform.Translate(speed * Time.deltaTime, 0f, 0f);
            }
        }
    }

    private void turn(){
        // Rotate some random multiple of 90 degrees
        rotationOption = (RotationOptions)Random.Range(0, 4);
    }

    // OnCollisionEnter causes the character to turn
    private void OnCollisionEnter2D(Collision2D collision){
        //Debug.Log("Collided with " + collision.gameObject.name);
        turn();
        teleportSlightly(collision);
    }

    private void teleportSlightly(Collision2D collision){
        // Teleport slightly away from the collision object
        Vector3 direction = transform.position - collision.transform.position;
        direction.Normalize();
        transform.position += direction * 0.1f;
    }

    private void fixRotation(){
        // Set rotation (real rotation this time) to up
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    void Start()
    {
        conversationBehaviour = GetComponent<ConversationBehaviour>();
        conspiracyBehaviour = GetComponent<ConspiracyBehaviour>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
    }

    // If the distance from this character to the conspiracy target is low, kill the target
    void checkAssassination(){
        if (conspiracyBehaviour.getIsInConspiracy()){
            if (Vector3.Distance(transform.position, gameManager.getGameObjectFromCharacterId(conspiracyBehaviour.conspiracy.targetid).transform.position) < 4f){
                Debug.Log("Close enough for an assassination");
                gameManager.killCharacter(conspiracyBehaviour.conspiracy.targetid);
                conspiracyBehaviour.isInConspiracy = false;
                //conspiracyBehaviour.conspiracy = null;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        moveAround();
        fixRotation();
        checkAssassination();
    }
}
