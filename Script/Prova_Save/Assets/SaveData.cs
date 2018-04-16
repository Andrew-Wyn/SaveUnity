using UnityEngine;
using System;
using System.IO; // Required fro reading/writing to files.
using System.Collections.Generic; // Used for Lists

/// <summary>
/// The different types of powerups a player can have.
/// </summary>
public enum PowerUp
{
    Fireballs,
    DoubleJump
}

/// <summary>
/// Responsible for:
/// - Maintaining the stats for a player and their progress
/// - Writing this data to a file.
/// - Reading this data from a file.
/// </summary>



[Serializable]
public class Saved
{
    // We initialize all of the stats to be default values.
    public int coins;
    public int health;
    public int lives;
    public List<PowerUp> powerUps = new List<PowerUp>();
    public string lastLevel;
    public Vector3 position;
    public Quaternion rotation;

    public Saved() {}
    
}






public class SaveData : MonoBehaviour
{

    #region Defaults
    public const string DEFAULT_LEVEL = "level1";
    public const int DEFAULT_COINS = 0;
    public const int DEFAULT_HEALTH = 100;
    public const int DEFAULT_LIVES = 3;
    #endregion

    // We initialize all of the stats to be default values.
    public int coins = DEFAULT_COINS;
    public int health = DEFAULT_HEALTH;
    public int lives = DEFAULT_LIVES;
    public List<PowerUp> powerUps = new List<PowerUp>();
    public string lastLevel = DEFAULT_LEVEL;
    public Vector3 position;
    public Quaternion rotation;

    const bool DEBUG_ON = true;    

    //init
    public void Start()
    {
        addPower(PowerUp.Fireballs);
        addPower(PowerUp.DoubleJump);
        Saved saved = ReadFromFile("C:/Users/unity/Desktop/Prova/save1.json");
        setPosition(saved.position);
    }

    public void OnMouseEnter()
    {
        WriteToFile("C:/Users/unity/Desktop/Prova/save1.json");
    }


    //write to file, posso utilizzare questo metodo per leggere direttamente l'istanza della classe
    public void WriteToFile(string filePath)
    {
        getPosition();
        getRotation();

        // Convert the instance ('this') of this class to a JSON string with "pretty print" (nice indenting).
        string json = JsonUtility.ToJson(this, true);

        // Write that JSON string to the specified file.
        File.WriteAllText(filePath, json);

        // Tell us what we just wrote if DEBUG_ON is on.
        if (DEBUG_ON)
            Debug.LogFormat("WriteToFile({0}) -- data:\n{1}", filePath, json);
    }

    //read dal file
    public static Saved ReadFromFile(string filePath)
    {
        // If the file doesn't exist then just return the default object.
        if (!File.Exists(filePath))
        {
            Debug.LogErrorFormat("ReadFromFile({0}) -- file not found, returning new object", filePath);
            return new Saved();
        }
        else
        {
            // If the file does exist then read the entire file to a string.
            string contents = File.ReadAllText(filePath);

            // If debug is on then tell us the file we read and its contents.
            if (DEBUG_ON)
                Debug.LogFormat("ReadFromFile({0})\ncontents:\n{1}", filePath, contents);

            // If it happens that the file is somehow empty then tell us and return a new SaveData object.
            if (string.IsNullOrEmpty(contents))
            {
                Debug.LogErrorFormat("File: '{0}' is empty. Returning default SaveData");
                return new Saved();
            }

            // Otherwise we can just use JsonUtility to convert the string to a new SaveData object.
            return JsonUtility.FromJson<Saved>(contents);
        }
    }


    //add powerups
    public void addPower(PowerUp a)
    {
        powerUps.Add(a);
    }

    public bool IsDefault()
    {
        return (
            coins == DEFAULT_COINS &&
            health == DEFAULT_HEALTH &&
            lives == DEFAULT_LIVES &&
            lastLevel == DEFAULT_LEVEL &&
            powerUps.Count == 0);
    }


    //stringhify LOL
    public override string ToString()
    {
        string[] powerUpsStrings = new string[powerUps.Count];
        for (int i = 0; i < powerUps.Count; i++)
        {
            powerUpsStrings[i] = powerUps[i].ToString();
        }

        return string.Format(
            "coins: {0}\nhealth: {1}\nlives: {2}\npowerUps: {3}\nlastLevel: {4}",
            coins,
            health,
            lives,
            "[" + string.Join(",", powerUpsStrings) + "]",
            lastLevel
            );
    }

    //get position of target element
    public void getPosition()
    {
        position = GameObject.Find("prova").transform.position;
    }

    //get rotation of target element
    public void getRotation()
    {
        rotation = GameObject.Find("prova").transform.rotation;
    }



    //setter

    //set position of target element
    public void setPosition(Vector3 pos)
    {
        GameObject.Find("prova").transform.position = pos;
    }

    //set rotation of target element
    public void setRotation()
    {
        //GameObject.Find("prova").transform.rotation = ;
    }

}