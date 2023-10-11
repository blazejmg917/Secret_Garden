using System.Collections;
using System.Collections.Generic;
using System.Security;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct gnomeSpawnPoint{
        [SerializeField, Tooltip("the gnome")]public GameObject gnome;
        [SerializeField, Tooltip("the index at which they will spawn")]public int gnomeSpawnId;

    }
    [SerializeField, Tooltip("the level count")]private int levelNumber = 3;
    [Header("other scripts")]
    [SerializeField, Tooltip("the timer script")]private TimerScript timer;
    [SerializeField, Tooltip("the item name display script")]private ItemDisplayScript itemDisplay;
    [SerializeField, Tooltip("the mouse checker script")]private mouseChecker mouseCheck;
    [SerializeField, Tooltip("the clicks left script")]private ClicksLeftScript clicksLeft;
    [SerializeField, Tooltip("items left script")]private ItemsLeftScript itemsLeft;
    [Header("UI")]
    [SerializeField, Tooltip("the pause canvas")]private GameObject pauseCanvas;
    [SerializeField, Tooltip("the game over canvas")]private GameObject gameOverCanvas;
    [SerializeField, Tooltip("the win canvas")]private GameObject winCanvas;
    [SerializeField, Tooltip("the leftmost win star")]private UnityEngine.UI.Image firstStar;
    [SerializeField, Tooltip("the middle win star")]private UnityEngine.UI.Image secondStar;
    [SerializeField, Tooltip("the rightmost win star")]private UnityEngine.UI.Image thirdStar;
    [SerializeField, Tooltip("how long the game waits on a post-game UI before returning to the main menu")]private float uiWaitTime = 3;
    [SerializeField, Tooltip("if the game is paused")]private bool isPaused = false;
    [Header("health")]
    [SerializeField, Tooltip("the player's max health")]private int maxHealth = 3;
    [SerializeField, Tooltip("the player's current health")]private int currentHealth = 3;
    [Header("Time")]
    [SerializeField, Tooltip("the max time limit for the level in seconds")]private float maxTime = 300;
    [SerializeField, Tooltip("the amount of time left in the level")]private float levelTimer = 300;
    [SerializeField, Tooltip("the time limit for three stars")]private float threeStarTime = 200;
    [SerializeField, Tooltip("the time limit for two stars")]private float twoStarTime = 100;
    [SerializeField, Tooltip("the time limit for one star")]private float oneStarTime = 0;
    [Header("objects")]
    [SerializeField, Tooltip("a list of all of the objects to find")]private List<GameObject> hiddenObjects = new List<GameObject>();
    [SerializeField, Tooltip("a list of all previously found objects")]private List<GameObject> foundObjects = new List<GameObject>();
    
    private GameObject currentObject;
    private int currentObjectIndex;
    private int maxTotalObjects;
    private bool gameOver = false;
    [Header("Gnomes")]
    [SerializeField, Tooltip("whether or not the gnomes show up at fixed intervals")]private bool fixedGnomePoints;
    [SerializeField, Tooltip("if fixedGnomePoints are active, use this list to set up when the gnomes spawn")]private List<gnomeSpawnPoint> fixedGnomeSpawns = new List<gnomeSpawnPoint>();
    [SerializeField, Tooltip("if fixedGnomePoints are active, this will be a list of all found gnomes")]private List<GameObject> foundGnomes = new List<GameObject>();
    private Dictionary<int, GameObject> gnomeSpawnMap = new Dictionary<int, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
        if(gameOverCanvas){
            gameOverCanvas.SetActive(false);
        }
        if(winCanvas){
            winCanvas.SetActive(false);
        }
        UnPause();
        if(!mouseCheck){
            mouseCheck = gameObject.GetComponentInChildren<mouseChecker>();
        }
        foundGnomes = new List<GameObject>();
        foundObjects = new List<GameObject>();
        currentHealth = maxHealth;
        levelTimer = maxTime;
        selectableObject[] selectableObjects = FindObjectsOfType<selectableObject>(true);
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
        if(clicksLeft){
            clicksLeft.Display(currentHealth);
        }
        maxTotalObjects = hiddenObjects.Count;
        GetNextObject();
    }

    public bool IsPaused(){
        return isPaused;
    }

    public void Pause(){
        isPaused = true;
        if(pauseCanvas){
            pauseCanvas.SetActive(true);
        }
    }

    public void UnPause(){
        isPaused = false;
        if(pauseCanvas){
            pauseCanvas.SetActive(false);
        }
    }

    public void TogglePaused(){
        if(isPaused){
            UnPause();
        }
        else{
            Pause();
        }

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
        if(gameOver || isPaused){
            return;
        }
        levelTimer -= Time.deltaTime;
        if(timer){
            timer.UpdateTimer(levelTimer);
        }
        if(levelTimer < 0 && !gameOver){
            GameOver();
        }
    }

    public void CorrectObjectClicked(){
        if(gameOver){
            return;
        }
        Debug.Log("correct object clicked");
        GetNextObject();
    }

    public void GetNextObject(){
        if(gameOver){
            return;
        }
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
        itemDisplay.Display(currentObject.GetComponent<selectableObject>().getId());
        itemsLeft.UpdateObjects(currentObjectIndex, maxTotalObjects);
    }

    public void InvalidClick(){
        Debug.Log("Clicked the wrong thing >:(");
        TakeDamage();
    }

    private void TakeDamage(){
        currentHealth--;
        clicksLeft.Display(currentHealth);
        if(currentHealth <= 0){
            GameOver();
        }
    }
    private void GameOver(){
        gameOver = true;
        Debug.Log("Game over >>>>:(((((((");
        if(gameOverCanvas){
            gameOverCanvas.SetActive(true);
        }
        Invoke("Quit",uiWaitTime);
    }
    private void WinLevel(){
        gameOver = true;
        Debug.Log("You win :))))))))");
        if(firstStar && levelTimer >= oneStarTime){
            firstStar.color = Color.white;
            Debug.Log("third star there " + levelTimer + ", " + twoStarTime);
        }
        else if(firstStar){
            firstStar.color = Color.black;
            Debug.Log("third star gone");
        }

        if(secondStar && levelTimer >= twoStarTime){
            secondStar.color = Color.white;
            Debug.Log("second star there " + levelTimer + ", " + twoStarTime);
        }
        else if(secondStar){
            secondStar.color = Color.black;
            Debug.Log("second star gone");
        }

        if(thirdStar && levelTimer >= threeStarTime){
            thirdStar.color = Color.white;
            Debug.Log("first star there");
        }
        else if(thirdStar){
            thirdStar.color = Color.black;
            Debug.Log("first star gone");
        }

        if(winCanvas){
            winCanvas.SetActive(true);
        }
        Invoke("Quit",uiWaitTime);

        if(PlayerPrefs.GetInt("UnlockedLevel", 1) < levelNumber){
            PlayerPrefs.SetInt("UnlockedLevel",levelNumber);
        }
    }

    public void Quit(){
        SceneManager.LoadScene(0);
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
