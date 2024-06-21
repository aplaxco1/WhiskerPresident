using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BillController : MonoBehaviour
{

    [Tooltip("R = Generate Resource, \n M = Generate Modifier, \n I = Generate Immediacy")]
    [SerializeField] private string templateSequence;

    [SerializeField] private GameObject foodSymbolPrefab;
    [SerializeField] private GameObject technologySymbolPrefab;
    [SerializeField] private GameObject infrastructureSymbolPrefab;

    [SerializeField] private GameObject positiveSymbolPrefab;
    [SerializeField] private GameObject negativeSymbolPrefab;

    // THESE SYMBOLS ARE TEMPORARY
    // DELETE THEM ONCE WE HAVE A PROPER IMPLEMENTATION
    [SerializeField] private GameObject nowSymbolPrefab;
    [SerializeField] private GameObject laterSymbolPrefab;

    [SerializeField] private bool uniqueResourcesOnly;
    private int lastSymbol = -1;
    //private [SerializeField] GameObject immediateSymbolPrefab;

    // One bill is classified as: One Resource, Positive or Negative, Immediate or Delayed
    public enum SymbolType
    {
        // New Symbol Types
        // Symbols for representing randomized categories
        RandomResource = 'R',
        RandomModifier = 'M',
        RandomImmediacy = 'I',
        // specific food symbols
        Food = '1',
        Technology = '2',
        Infrastructure = '3',
        // specific positivity symbols
        Positive = 'p',
        Negative = 'n',
        // specific immediacy symbols
        Immediate = 'i',
        Delayed = 'd',
    }

    public List<SymbolType> symbols;
    
    public int symbolsPerLine;
    public float symbolHorizontalSpacing;
    public float symbolVerticalSpacing;

    public float initialSymbolXCoord;
    public float initialSymbolYCoord;
    public float initialSymbolZCoord;

    public float initialSymbolScale;

    private Transform symbolParent;

    // Designer input string (What sequence type to generate)
    // Grabbed from BillContentsManager
    //private string templateSequence;
    
    // Script output string (What has been generated exactly)
    public string generatedSequence;
    
    public bool hasBeenPlacedDown = false;
    
    void Awake()
    {
        symbolParent = transform.Find("SymbolParent");
    }

    public void InitializeBill()
    {
        //templateSequence = BillContentsManager.Instance.templateSequence;   // Take our set symbol pattern generation
        symbols = new List<SymbolType>();   // initialize empty list "symbols"
        GenerateSymbolList();   // fill out "symbols" list
        GenerateSymbolPrefabs();    // generate the actual visual prefabs
    }

    public void UninitializeBill()
    {
        //Debug.Log("ok inside uninitializebill");
        BillContentsManager.Instance.SaveBill(gameObject);
        //Debug.Log("bill saved");
        Destroy(gameObject);
        //Debug.Log("bill destroyed");
    }

    // Use partial random system to generate symbol list
    private void GenerateSymbolList()
    {
        foreach(SymbolType a in templateSequence)
        {
            switch (a)
            {
                case SymbolType.RandomResource:
                    RandomizeResourceSymbol();
                    break;
                case SymbolType.RandomModifier:
                    RandomizePositivitySymbol();
                    break;
                case SymbolType.RandomImmediacy:
                    RandomizeImmediacySymbol();
                    break;
                
                case SymbolType.Food:
                    symbols.Add(SymbolType.Food);
                    break;
                case SymbolType.Technology:
                    symbols.Add(SymbolType.Technology);
                    break;
                case SymbolType.Infrastructure:
                    symbols.Add(SymbolType.Infrastructure);
                    break;

                case SymbolType.Positive:
                    symbols.Add(SymbolType.Positive);
                    break;
                case SymbolType.Negative:
                    symbols.Add(SymbolType.Negative);
                    break;
                
                default:
                    Debug.LogWarning("WARNING: INVALID SEQUENCE TEMPLATE LETTER");
                    break;
            }
        }
    }

    //THESE ARE HELPER FUNCITONS TO DECIDE RANDOMIZED SYMBOLS
    private void RandomizeResourceSymbol()
    {
        int rand;
        if (uniqueResourcesOnly && (lastSymbol >= 0))
        {
            if (lastSymbol == 0)
            {
                rand = Random.Range(1, 3);
            }
            else if (lastSymbol == 1)
            {
                rand = Random.Range(0, 2) * 2;
            }
            else if (lastSymbol == 2)
            {
                rand = Random.Range(0, 2);
            }
            else
            {
                // LAST SYMBOL IS INVALID
                Debug.LogWarning("INVALID LASTSYMBOL! Last Symbol is: " + lastSymbol + ". Defaulting to Random Symbol Generation");
                rand = Random.Range(0, 3);
            }
            lastSymbol = rand;
        }
        else
        {
            rand = Random.Range(0, 3);
            lastSymbol = rand;
        }

        if (rand == 0)
        {
            symbols.Add(SymbolType.Food);
        }
        else if (rand == 1)
        {
            symbols.Add(SymbolType.Technology);
        }
        else if(rand == 2)
        {
            symbols.Add(SymbolType.Infrastructure);
        }
    }

    private void RandomizePositivitySymbol()
    {
        int rand = Random.Range(0, 2);
        //print("Random Positivity: " + rand);
        if (rand == 1)
        {
            symbols.Add(SymbolType.Positive);
        }
        else
        {
            symbols.Add(SymbolType.Negative);
        }
    }
    private void RandomizeImmediacySymbol()
    {
        int rand = Random.Range(0, 2);
        //print("Random Immediacy: " + rand);
        if (rand == 1)
        {
            symbols.Add(SymbolType.Immediate);
        }
        else
        {
            symbols.Add(SymbolType.Delayed);
        }
    }

    // Ran only if not given a symbol sequence template
    // private void GenerateRandomSymbols()
    // {
    //     int symbolsToGen = numSymbols;
    //     symbolsToGen += Random.Range(-(numSymbolDistribution + 1), numSymbolDistribution);
    //     //print("symbolsToGen: " + symbolsToGen);
    //     
    //     //All symbol types as array
    //     SymbolType[] symbolTypes = Enum.GetValues(typeof(SymbolType)).Cast<SymbolType>().ToArray();
    //     for (int i = 0; i <= symbolsToGen; i++)
    //     {
    //         SymbolType randomSymbol = symbolTypes[Random.Range(0, symbolTypes.Length)];
    //         //print("randomSymbol: " + randomSymbol);
    //         symbols.Add(randomSymbol);
    //     }
    // }

    // Generate symbol prefabs onto the bill
    private void GenerateSymbolPrefabs()
    {
        /*
         * TODO:
         * Get New Prefabs for the new symbol types
        */
        int symbolCount = -1;
        float xCoord = initialSymbolXCoord;
        float yCoord = initialSymbolYCoord;
        float zCoord = initialSymbolZCoord;
        foreach (SymbolType symbol in symbols)
        {
            GameObject symbolToInstantiate;
            switch (symbol)
            {
                case SymbolType.Food:
                    symbolToInstantiate = foodSymbolPrefab;
                    break;
                case SymbolType.Technology:
                    symbolToInstantiate = technologySymbolPrefab;
                    break;
                case SymbolType.Infrastructure:
                    symbolToInstantiate = infrastructureSymbolPrefab;
                    break;

                case SymbolType.Positive:
                    symbolToInstantiate = positiveSymbolPrefab;
                    break;
                case SymbolType.Negative:
                    symbolToInstantiate = negativeSymbolPrefab;
                    break;

                case SymbolType.Immediate:
                    symbolToInstantiate = nowSymbolPrefab;
                    break;
                case SymbolType.Delayed:
                    symbolToInstantiate = laterSymbolPrefab;
                    break;
                default:
                    symbolToInstantiate = new GameObject();
                    break;
            }
            //print("symbolToInstantiate: " + symbolToInstantiate.name);
            symbolCount++;
            
            // Start instantiating from the left again and move to new line
            if (symbolCount >= symbolsPerLine)
            {
                symbolCount = 0;
                zCoord += symbolVerticalSpacing; // CHANGE THIS BACK TO -= IF AUTUMN WAS STUPID
                xCoord = initialSymbolXCoord;
            }
            xCoord -= symbolHorizontalSpacing;
            Vector3 pos = new Vector3(xCoord, yCoord, zCoord);
            
            GameObject instantiatedSymbol = Instantiate(symbolToInstantiate, symbolParent, false);
            // changing the scale of the symbols, subject to change
            Vector3 scale = new Vector3(symbolToInstantiate.transform.localScale.x * initialSymbolScale, symbolToInstantiate.transform.localScale.y * initialSymbolScale, symbolToInstantiate.transform.localScale.z * initialSymbolScale) ;
            instantiatedSymbol.transform.position += pos;
            instantiatedSymbol.transform.localScale = scale;

        }
    }

    // Will return the effects of the bill on stat bars
    public StatVector CalculateOutcome()
    {
        StatVector returnVector = new StatVector();
        int multiplier = 1;
        int scoreInterval = 5;
        foreach (SymbolType symbol in symbols)
        {
            switch (symbol)
            {
                //REDO
                case SymbolType.Food:
                    returnVector.StatA += scoreInterval * multiplier;
                    multiplier = 1;
                    break;
                case SymbolType.Technology:
                    returnVector.StatB += scoreInterval * multiplier;
                    multiplier = 1;
                    break;
                case SymbolType.Infrastructure:
                    returnVector.StatC += scoreInterval * multiplier;
                    multiplier = 1;
                    break;
                case SymbolType.Positive:
                    multiplier = 1;
                    break;
                case SymbolType.Negative:
                    multiplier = -2;
                    break;
                default:
                    Debug.LogWarning("WARNING: INVALID SYMBOL FOR BILL CALCULATION");
                    break;
            }
        }

        return returnVector;
    }

    // Bill approved, add stat vector to total stats
    public void PassBill()
    {
        StatVector returnedStatVector = CalculateOutcome();
        DayManager.Instance.AdjustStats(returnedStatVector);
        string statOutput = returnedStatVector.StringConversion();
        //print("BILL PASSED WITH STATS: " + statOutput);
        //GameObject.Find("Main Camera/Result Text").GetComponent<TMP_Text>().text = statOutput;
    }

    public void VetoBill() {
        StatVector returnedStatVector = CalculateOutcome();
        returnedStatVector.StatA *= -1;
        returnedStatVector.StatB *= -1;
        returnedStatVector.StatC *= -1;
        DayManager.Instance.AdjustStats(returnedStatVector);
        string statOutput = returnedStatVector.StringConversion();
    }

    //// Bill is denied, does not effect stats
    //public void VetoBill()
    //{
    //    //GameObject.Find("Main Camera/Result Text").GetComponent<TMP_Text>().text = "BILL VETOED";
    //}

    //// Reset the bill
    //private void Reinitialize()
    //{
    //    foreach (Transform child in symbolParent) {
    //        Destroy(child.gameObject);
    //    }
    //    InitializeBill();
    //}

    // Calculates whether bill evaluates to a pass or a veto (pass = return > 0, fail = return < 0, 0 = tie)
    public float evaluatePassVeto()
    {
        
        PawPrint[] prints = gameObject.GetComponentsInChildren<PawPrint>();
        //print("eval w/ prints length: " + prints.Length);
        float score = 0;
        float mostRecent = -1;
        foreach (PawPrint print in prints) {
            //score += (print.color.g - print.color.r)*print.color.a;
            if (print.renderQueue > mostRecent) {
                mostRecent = print.renderQueue;
                score = (print.color.g - print.color.r);
            }
        }

        //print("SCORE: " +  score);
        if (score > 0)
        {
            PassBill();
        }
        return score;
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is the one we want to detect collisions with.
        if (collision.gameObject.tag == "tray")
        {
            Debug.Log("Collision detected with the referenced GameObject!");
            // You can put any code here to handle the collision with the referenced GameObject.
        }
    }
}
