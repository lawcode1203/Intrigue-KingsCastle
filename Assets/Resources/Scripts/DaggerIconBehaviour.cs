using UnityEngine;

public class DaggerIconBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject headGameObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (headGameObject == null){
            Destroy(this.gameObject);
        }
        else{
            transform.position = headGameObject.transform.position;
        }
        
    }
}
