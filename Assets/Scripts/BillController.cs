using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BillController : MonoBehaviour
{

    public enum SymbolType
    {
        Food,
        Money,
        Bone,
    }

    public class StatVector
    {
        public int MoneyStat;
        public int ReputationStat;
        public int StealthStat;
    }
    
    public SymbolType[] symbols;
    
    //How many symbols should be generated
    public int numSymbols;
    
    //Random factor, numSymbols +/- rand(symbolDistribution) will be generated
    public int numSymbolDistribution;
    
    
    void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    private void GenerateSymbolPrefabs()
    {
        foreach (SymbolType symbol in symbols)
        {
            
        }
    }

    private StatVector CalculateOutcome()
    {
        StatVector returnVector = new StatVector();
        foreach (SymbolType symbol in symbols)
        {
            
        }

        return returnVector;
    }

    public void PassBill()
    {
        StatVector returnedStatVector = CalculateOutcome();
        Unitialize();
    }

    public void VetoBill()
    {
        Unitialize();
    }

    private void Unitialize()
    {
        
    }
}
