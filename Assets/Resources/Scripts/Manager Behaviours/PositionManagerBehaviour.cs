using UnityEngine;
using System;
using System.Collections.Generic;

public class PositionManagerBehaviour : MonoBehaviour
{
    public List<string> positions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        positions = new List<string>();

        positions.Add("King/Queen");
        positions.Add("Prince/Princess");
        positions.Add("Chancellor/Chancellor");
        positions.Add("Duke/Duchess");
        positions.Add("Marquess/Marchioness");
        positions.Add("Count/Countess");
        positions.Add("High Chamberlain/High Chambermaid");
        positions.Add("Viscount/Viscountess");
        positions.Add("Steward/Stewardess");
        positions.Add("Baron/Baroness");
        positions.Add("Bishop/Priestess");
        positions.Add("Constable/Constableess");
        positions.Add("Treasurer/Treasurer");
        positions.Add("Counselor/Counselor");
        positions.Add("Lord/Lady");
        positions.Add("Ambassador/Ambassador");
        positions.Add("Knight/Dame");
        positions.Add("Jester/Poetess");
        positions.Add("Servant/Servant");
        positions.Add("Commoner/Commoner");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getGenderVersion(int positionId, bool isMale){
        string version = positions[positionId];
        string version1 = version.Substring(0, version.IndexOf('/'));
        string version2 = version.Substring(version.IndexOf('/') + 1);
        if (isMale){
            return version1;
        } else {
            return version2;
        } 
    }
    
    public int getNumPositions(){
        return positions.Count;
    }

    public float getCharacterPositionPower(int positionId){
        return positions.Count - positionId;
    }
}
