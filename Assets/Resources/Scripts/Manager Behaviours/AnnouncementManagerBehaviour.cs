using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class AnnouncementManagerBehaviour : MonoBehaviour
{
    public TextMeshProUGUI announcement;
    
    private List<string> announcements = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        announcement.text = "Game Started";
        
    }

    private void cullAnnouncements(){
        // Keep only the most recent 5 announcements
        if (announcements.Count > 5){
            announcements.RemoveRange(0, announcements.Count - 5);
        }
    }

    public void makeAnnouncement(string message){
        announcements.Add(message);
        cullAnnouncements();
        announcement.text = "";
        for (int i = 0; i < announcements.Count; i++){
            announcement.text += announcements[i] + "\n";
        }
        
    }

    public void reoutputAnnouncements(){
        announcement.text = "";
        for (int i = 0; i < announcements.Count; i++){
            announcement.text += announcements[i] + "\n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        cullAnnouncements();
        reoutputAnnouncements();
        
    }
}
