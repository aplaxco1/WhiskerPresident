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
        Food,
        Money,
        Bone,
        Negator,
        Doubler,
    }

    public class StatVector
    {
        public int FoodStat;
        public int MoneyStat;
        public int BoneStat;

        public string StringConversion()
        {
            string ret = "";
            ret += "Red Stat (Food): " + FoodStat + "\n";
            ret += "Green Stat (Money): " + MoneyStat + "\n";
            ret += "Blue Stat (Bone): " + BoneStat + "\n";
            return ret;
        }
    }
    
    private List<SymbolType> symbols;
    
    //How many symbols should be generated
    public int numSymbols;
    
    //Random factor, numSymbols +/- rand(symbolDistribution) will be generated
    public int numSymbolDistribution;

    public int symbolsPerLine;
    public float symbolHorizontalDist;
    public float symbolVerticalDist;

    public float initialSymbolXCoord;
    public float initialSymbolYCoord;
    public float initialSymbolZCoord;
    

    private Transform symbolParent;
    
    
    void Awake()
    {
        symbolParent = transform.Find("SymbolParent");
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeBill();
        
    }

    // Update is called once per frame
    void Update()
    {
        // ! Temporary pass/veto controls
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PassBill();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            VetoBill();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reinitialize();
        }
    }

    public void InitializeBill()
    {
        symbols = new List<SymbolType>();
        GenerateSymbolList();
        GenerateSymbolPrefabs();
    }

    // Use partial random system to generate symbol list
    private void GenerateSymbolList()
    {
        int symbolsToGen = numSymbols;
        symbolsToGen += Random.Range(-(numSymbolDistribution + 1), numSymbolDistribution);
        //print("symbolsToGen: " + symbolsToGen);
        
        //All symbol types as array
        SymbolType[] symbolTypes = Enum.GetValues(typeof(SymbolType)).Cast<SymbolType>().ToArray();
        for (int i = 0; i <= symbolsToGen; i++)
        {
            SymbolType randomSymbol = symbolTypes[Random.Range(0, symbolTypes.Length)];
            //print("randomSymbol: " + randomSymbol);
            symbols.Add(randomSymbol);
        }
    }

    // Generate symbol prefabs onto the bill
    private void GenerateSymbolPrefabs()
    {
        //print("Symbols: " + symbols);
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
                yCoord -= symbolVerticalDist;
                xCoord = initialSymbolXCoord;
            }
            xCoord += symbolHorizontalDist;
            Vector3 pos = new Vector3(xCoord, zCoord, yCoord);
            Quaternion rot = Quaternion.Euler(Vector3.zero);
            //print(pos);
            
            GameObject instantiatedSymbol = Instantiate(symbolToInstantiate, symbolParent, false);
            instantiatedSymbol.transform.position += pos;

        }
    }

    // Will return the effects of the bill on stat bars
    private StatVector CalculateOutcome()
    {
        StatVector returnVector = new StatVector();
        int multiplier = 1;
        int scoreInterval = 5;
        foreach (SymbolType symbol in symbols)
        {
            switch (symbol)
            {
                case SymbolType.Bone:
                    returnVector.BoneStat += scoreInterval * multiplier;
                    multiplier = 1;
                    break;
                case SymbolType.Food:
                    returnVector.FoodStat += scoreInterval * multiplier;
                    multiplier = 1;
                    break;
                case SymbolType.Money:
                    returnVector.MoneyStat += scoreInterval * multiplier;
                    multiplier = 1;
                    break;
                case SymbolType.Negator:
                    multiplier *= -1;
                    break;
                case SymbolType.Doubler:
                    multiplier *= 2;
                    break;
                default:
                    break;
            }
        }

        return returnVector;
    }

    // Bill approved, add stat vector to total stats
    public void PassBill()
    {
        StatVector returnedStatVector = CalculateOutcome();
        string statOutput = returnedStatVector.StringConversion();
        print("BILL PASSED WITH STATS: " + statOutput);
        GameObject.Find("Main Camera/Result Text").GetComponent<TMP_Text>().text = statOutput;
    }

    // Bill is denied, does not effect stats
    public void VetoBill()
    {
        GameObject.Find("Main Camera/Result Text").GetComponent<TMP_Text>().text = "BILL VETOED";
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
        float score = 0;
        float mostRecent = 0;
        foreach (PawPrint print in prints) {
            //score += (print.color.g - print.color.r)*print.color.a;
            if (print.renderQueue > mostRecent) {
                mostRecent = print.renderQueue;
                score = (print.color.g - print.color.r);
            }
        }
        return score;
    }
}
