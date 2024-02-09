using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public int MoneyStat;
        public int FoodStat;
        public int BoneStat;
    }
    
    public SymbolType[] symbols;
    
    //How many symbols should be generated
    public int numSymbols;
    
    //Random factor, numSymbols +/- rand(symbolDistribution) will be generated
    public int numSymbolDistribution;

    public int symbolsPerLine;
    public float symbolHorizontalDist;
    public float symbolVerticalDist;

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
        if (Input.GetKeyDown("Y"))
        {
            PassBill();
        }
        if (Input.GetKeyDown("N"))
        {
            VetoBill();
        }
    }

    public void InitializeBill()
    {
        GenerateSymbolList();
        GenerateSymbolPrefabs();
    }

    // Use partial random system to generate symbol list
    private void GenerateSymbolList()
    {
        int symbolsToGen = numSymbols;
        symbolsToGen += Random.Range(-numSymbolDistribution, numSymbolDistribution + 1);
        
        //All symbol types as array
        SymbolType[] symbolTypes = Enum.GetValues(typeof(SymbolType)).Cast<SymbolType>().ToArray();
        for (int i = 0; i <= symbolsToGen; i++)
        {
            SymbolType randomSymbol = symbolTypes[Random.Range(0, symbolTypes.Length)];
            symbols.Append(randomSymbol);
        }
    }

    // Generate symbol prefabs onto the bill
    private void GenerateSymbolPrefabs()
    {
        int symbolCount = 0;
        float xCoord = 0;
        float yCoord = 0;
        float zCoord = -1;
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
            symbolCount++;
            
            // Start instantiating from the left again and move to new line
            if (symbolCount >= symbolsPerLine)
            {
                symbolCount = 0;
                yCoord -= symbolVerticalDist;
                xCoord = 0;
            }
            xCoord += symbolHorizontalDist;
            Vector3 pos = new Vector3(xCoord, yCoord, zCoord);
            Quaternion rot = Quaternion.Euler(Vector3.zero);
            
            Instantiate(symbolToInstantiate, pos, rot, symbolParent);
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
        Unitialize();
        print("BILL PASSED WITH STATS: " + returnedStatVector);
    }

    // Bill is denied, does not effect stats
    public void VetoBill()
    {
        Unitialize();
        print("BILL VETOED");
    }

    // Remove the bill from the game
    private void Unitialize()
    {
        
    }
}
