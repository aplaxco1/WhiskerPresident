using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BillController : MonoBehaviour
{

    public GameObject foodSymbolPrefab;
    public GameObject moneySymbolPrefab;
    public GameObject boneSymbolPrefab;
    public GameObject negatorSymbolPrefab;
    public GameObject doublerSymbolPrefab;

    public enum SymbolType
    {
        Food = '1', //Symbol1
        Money = '2', //Symbol2
        Bone = '3', //Symbol3
        Negator = 'N',
        Doubler = 'D', //Multiplier
        
        // Generate symbol 1 or 2
        ValueA = 'A',
        
        // Generate symbol 1 or 2 or 3
        ValueB = 'B',
        
        // Generate modifier
        Modifier = 'M',
    }

    public class StatVector
    {
        public int RedStat;
        public int GreenStat;
        public int BlueStat;

        public string StringConversion()
        {
            string ret = "";
            ret += "Red: " + RedStat + "\n";
            ret += "Green: " + GreenStat + "\n";
            ret += "Blue: " + BlueStat + "\n";
            return ret;
        }
    }
    
    public List<SymbolType> symbols;
    
    // //How many symbols should be generated
    // public int numSymbols;
    //
    // //Random factor, numSymbols +/- rand(symbolDistribution) will be generated
    // public int numSymbolDistribution;

    public int symbolsPerLine;
    public float symbolHorizontalDist;
    public float symbolVerticalDist;

    public float initialSymbolXCoord;
    public float initialSymbolYCoord;
    public float initialSymbolZCoord;

    private Transform symbolParent;

    // Designer input string (What sequence type to generate)
    // Grabbed from BillContentsManager
    private string templateSequence;
    
    // Script output string (What has been generated exactly)
    public string generatedSequence;
    
    
    void Awake()
    {
        symbolParent = transform.Find("SymbolParent");
    }

    // Update is called once per frame
    void Update()
    {
        // ! Temporary pass/veto controls
        // if (Input.GetKeyDown(KeyCode.Y))
        // {
        //     PassBill();
        // }
        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     VetoBill();
        // }
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     Reinitialize();
        // }
    }

    public void InitializeBill()
    {
        templateSequence = BillContentsManager.Instance.templateSequence;
        symbols = new List<SymbolType>();
        GenerateSymbolList();
        GenerateSymbolPrefabs();
    }

    public void UninitializeBill()
    {
        BillContentsManager.Instance.SaveBill(gameObject);
        Destroy(gameObject);
    }

    // Use partial random system to generate symbol list
    private void GenerateSymbolList()
    {
        // if (templateSequence == "")
        // {
        //     GenerateRandomSymbols();
        //     return;
        // }
        
        foreach(SymbolType a in templateSequence)
        {
            switch (a)
            { 
                case SymbolType.ValueA:
                    SequenceValueA();
                    break;
                case SymbolType.ValueB:
                    SequenceValueB();
                    break;
                case SymbolType.Modifier:
                    SequenceModifier();
                    break;
                
                case SymbolType.Food:
                    symbols.Add(SymbolType.Food);
                    break;
                case SymbolType.Money:
                    symbols.Add(SymbolType.Money);
                    break;
                case SymbolType.Bone:
                    symbols.Add(SymbolType.Bone);
                    break;
                case SymbolType.Negator:
                    symbols.Add(SymbolType.Negator);
                    break;
                case SymbolType.Doubler:
                    symbols.Add(SymbolType.Doubler);
                    break;
                
                default:
                    print("WARNING: INVALID SEQUENCE TEMPLATE LETTER");
                    break;
            }
        }
    }

    private void SequenceValueA()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            symbols.Add(SymbolType.Food);
        } 
        else if (rand == 1)
        {
            symbols.Add(SymbolType.Money);
        }
    }
    
    private void SequenceValueB()
    {
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            symbols.Add(SymbolType.Food);
        } 
        else if (rand == 1)
        {
            symbols.Add(SymbolType.Money);
        }
        else if (rand == 2)
        {
            symbols.Add(SymbolType.Bone);
        }
    }

    private void SequenceModifier()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            symbols.Add(SymbolType.Doubler);
        }
        else if (rand == 1)
        {
            symbols.Add(SymbolType.Negator);
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
        int symbolCount = -1;
        float xCoord = initialSymbolXCoord;
        float yCoord = initialSymbolYCoord;
        float zCoord = initialSymbolZCoord;
        foreach (SymbolType symbol in symbols)
        {
            GameObject symbolToInstantiate;
            switch (symbol)
            {
                case SymbolType.Bone:
                    symbolToInstantiate = boneSymbolPrefab;
                    break;
                case SymbolType.Food:
                    symbolToInstantiate = foodSymbolPrefab;
                    break;
                case SymbolType.Money:
                    symbolToInstantiate = moneySymbolPrefab;
                    break;
                case SymbolType.Negator:
                    symbolToInstantiate = negatorSymbolPrefab;
                    break;
                case SymbolType.Doubler:
                    symbolToInstantiate = doublerSymbolPrefab;
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
                yCoord += symbolVerticalDist; // CHANGE THIS BACK TO -= IF AUTUMN WAS STUPID
                xCoord = initialSymbolXCoord;
            }
            xCoord -= symbolHorizontalDist;
            Vector3 pos = new Vector3(xCoord, zCoord, yCoord);
            
            GameObject instantiatedSymbol = Instantiate(symbolToInstantiate, symbolParent, false);
            instantiatedSymbol.transform.position += pos;

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
                case SymbolType.Bone:
                    returnVector.BlueStat += scoreInterval * multiplier;
                    multiplier = 1;
                    break;
                case SymbolType.Food:
                    returnVector.RedStat += scoreInterval * multiplier;
                    multiplier = 1;
                    break;
                case SymbolType.Money:
                    returnVector.GreenStat += scoreInterval * multiplier;
                    multiplier = 1;
                    break;
                case SymbolType.Negator:
                    multiplier *= -1;
                    break;
                case SymbolType.Doubler:
                    multiplier *= 2;
                    break;
                default:
                    print("WARNING: INVALID SYMBOL FOR BILL CALCULATION");
                    break;
            }
        }

        return returnVector;
    }

    // Bill approved, add stat vector to total stats
    public void PassBill()
    {
        StatVector returnedStatVector = CalculateOutcome();
        StatManager.Instance.AdjustStats(returnedStatVector);
        string statOutput = returnedStatVector.StringConversion();
        //print("BILL PASSED WITH STATS: " + statOutput);
        //GameObject.Find("Main Camera/Result Text").GetComponent<TMP_Text>().text = statOutput;
    }

    // Bill is denied, does not effect stats
    public void VetoBill()
    {
        //GameObject.Find("Main Camera/Result Text").GetComponent<TMP_Text>().text = "BILL VETOED";
    }

    // Reset the bill
    private void Reinitialize()
    {
        foreach (Transform child in symbolParent) {
            Destroy(child.gameObject);
        }
        InitializeBill();
    }

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
}
