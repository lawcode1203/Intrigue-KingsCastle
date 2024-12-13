using UnityEngine;
using System.Collections.Generic;

public enum ConversationType{
    Affirmative,
    Negative,
    Conspiracy,
    YesBelief,
    NoBelief,
    likingQuestions,
    likingAnswers,
    conspiracyAccusation
}

public class StringManagerBehaviour : MonoBehaviour
{
    public string path = "Assets/Resources/Conversations/";
    string affirmativeFile = "affirmative.txt";
    string negativeFile = "negative.txt";
    string conspiracyFile = "conspiracy.txt";
    string yesBeliefFile = "yesBelief.txt";
    string noBeliefFile = "noBelief.txt";

    string likingQuestionsFile = "likingQuestions.txt";
    string likingAnswersFile = "likingAnswers.txt";
    string conspiracyAccusationFile = "conspiracyAccusation.txt";

    private GameManagerBehaviour gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        
    }

    public string getProperFile(ConversationType type){
        switch(type){
            case ConversationType.Affirmative:
                return affirmativeFile;
            case ConversationType.Negative:
                return negativeFile;
            case ConversationType.Conspiracy:
                return conspiracyFile;
            case ConversationType.YesBelief:
                return yesBeliefFile;
            case ConversationType.NoBelief:
                return noBeliefFile;
            case ConversationType.likingQuestions:
                return likingQuestionsFile;
            case ConversationType.likingAnswers:
                return likingAnswersFile;
            case ConversationType.conspiracyAccusation:
                return conspiracyAccusationFile;
            default:
                return "";
        }
    }

    public string getRandomLine(string file){
        List<string> lines = new List<string>();
        // Open the file and read it line by line, adding each line to the list
        System.IO.StreamReader fileReader = new System.IO.StreamReader(path + file);
        while (!fileReader.EndOfStream){
            lines.Add(fileReader.ReadLine());
        }
        fileReader.Close();
        // Return a random line from the list
        return lines[Random.Range(0, lines.Count)];
    }

    public string buildResponse(ConversationType type, string targetName, string speakerName, string personName, string confidence){
        string file = getProperFile(type);
        string line = getRandomLine(file);
        string message = string.Format(line, targetName, speakerName, personName, confidence);
        return message;
    }

    public string getConversationText(Conversation conversation){
        string confidence = "None";
        if (conversation.floatData < 0.0f){
            confidence = "low";
        } else{
            confidence = "high";
        }
        string response = buildResponse(conversation.type, gameManager.getCharacterFullName(conversation.speakerId), gameManager.getCharacterFullName(conversation.responderId), gameManager.getCharacterFullName(conversation.thirdPersonId), confidence);
        return response;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
