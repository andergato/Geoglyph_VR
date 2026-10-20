using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCheckController : MonoBehaviour
{
    public List<GameObject> items;
    public List<string> conditions;
    
    private Stack<Tuple<GameObject,string>> itemStack;
    public CheckForItem check; 
    // Start is called before the first frame update
    void Start()
    {
        for (int i = items.Count; i >= 0; i--)
        {
            itemStack.Push(Tuple.Create(items[i], conditions[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
