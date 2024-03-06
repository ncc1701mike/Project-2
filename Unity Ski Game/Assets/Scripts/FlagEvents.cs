using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlagEventArgs : EventArgs
    {
        public string FlagType { get; set; }
        
        public FlagEventArgs(string flagType)
        {
            FlagType = flagType;
        }
    }


public class FlagEvents : MonoBehaviour
{
    public delegate void flagAction(FlagEventArgs e);
    public static event flagAction OnFlagReached;

    public static void FlagReached(string flagType)
    {
        FlagEventArgs args = new FlagEventArgs(flagType);
        OnFlagReached?.Invoke(args);
    }
    
}



