using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct gnomeSpawnPoint{
        [SerializeField, Tooltip("the gnome")]public GameObject gnome;
        [SerializeField, Tooltip("the index at which they will spawn")]public int gnomeSpawnId;

    }
    [SerializeField, Tooltip("the mouse checker script")]private mouseChecker mouseCheck;
    [Header("health")]
    [SerializeField, Tooltip("the player's max health")]private int maxHealth = 3;
    [SerializeField, Tooltip("the player's current health")]private int currentHealth = 3;
    [Header("Time")]
    [SerializeField, Tooltip("the max time limit for the level in seconds")]private float maxTime = 300;
    [SerializeField, Tooltip("the amount of time left in the level")]private float levelTimer = 300;
    [Header("objects")]
    [SerializeField, Tooltip("a list of all of the objects to find")]private List<GameObject> hiddenObjects = new List<GameObject>();
    [SerializeField, Tooltip("a list of all previously found objects")]private List<GameObject> foundObjects = new List<GameObject>();

    private GameObject currentObject;
    private int currentObjectIndex;
    [Header("Gnomes")]
    [SerializeField, Tooltip("whether or not the gnomes show up at fixed intervals")]private bool fixedGnomePoints;
    [SerializeField, Tooltip("if fixedGnomePoints are active, use this list to set up when the gnomes spawn")]private List<gnomeSpawnPoint> fixedGnomeSpawns = new List<gnomeSpawnPoint>();
    [SerializeField, Tooltip("if fixedGnomePoints are active, this will be a list of all found gnomes")]private List<GameObject> foundGnomes = new List<GameObject>();
    private Dictionary<int, GameObject> gnomeSpawnMap = new Dictionary<int, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        if(!mouseCheck){
            mouseCheck = gameObject.GetComponentInChildren<mouseChecker>();
        }
        foundGnomes = new List<GameObject>();
        foundObjects = new List<GameObject>();
        currentHealth = maxHealth;
        levelTimer = maxTime;
        selectableObject[] selectableObjects = FindObjectsOfType<selectableObject>();
        foreach(selectableObject obj in selectableObjects){
            if(!hiddenObjects.Contains(obj.gameObject) && (!fixedGnomePoints || (fixedGnomePoints && obj.GetObjectType() == selectableObject.ObjectType.TOOL))){
                hiddenObjects.Add(obj.gameObject);
            }
            // else if(fixedGnomePoints && obj.GetObjectType == ObjectType.GNOME && !ContainsGnome(obj, fixedGnomeSpawns)){
            //     gnomeSpawnMap gnomeSpawn = new gnomeSpawnPoint();
            //     gnomeSpawn.gnome = 
            //     fixedGnomeSpawns.Add();
            // }
        }
        if(fixedGnomePoints){
            foreach(gnomeSpawnPoint gnomeSpawn in fixedGnomeSpawns){
                gnomeSpawnMap.Add(gnomeSpawn.gnomeSpawnId, gnomeSpawn.gnome);
            }
        }
        GetNextObject();
    }

    bool ContainsGnome(GameObject gnome, List<gnomeSpawnPoint> gnomeList){
        foreach(gnomeSpawnPoint gnomeSpawn in gnomeList){
            if(gnomeSpawn.gnome == gnome){
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CorrectObjectClicked(){
        Debug.Log("correct object clicked");
        GetNextObject();
    }

    public void GetNextObject(){
        if(currentObject){
            if(fixedGnomePoints && currentObject.GetComponent<selectableObject>().GetObjectType() == selectableObject.ObjectType.GNOME){
                foundGnomes.Add(currentObject);
            }
            else{
                foundObjects.Add(currentObject);
                hiddenObjects.Remove(currentObject);
            }
            currentObject.SetActive(false);
            currentObjectIndex++;
        }
        if(fixedGnomePoints){
            GameObject potentialGnome;
            if(gnomeSpawnMap.TryGetValue(currentObjectIndex, out potentialGnome)){
                currentObject = potentialGnome;
                return;
            }
        }
        if(hiddenObjects.Count < 1){
            WinLevel();
            return;
        }
        currentObject = hiddenObjects[Random.Range(0,hiddenObjects.Count - 1)];
        mouseCheck.UpdateGoalObj(currentObject.GetComponent<selectableObject>().getId());

    }

    public void InvalidClick(){
        Debug.Log("Clicked the wrong thing >:(");
        TakeDamage();
    }

    private void TakeDamage(){
        currentHealth--;
        if(currentHealth <= 0){
            GameOver();
        }
    }
    private void GameOver(){
        Debug.Log("Game over >>>>:(((((((");
    }
    private void WinLevel(){
        Debug.Log("You win :))))))))");
    }
}