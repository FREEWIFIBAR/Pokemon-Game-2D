using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialog
{
    [SerializeField] private List<String> lines;

    public List<String> Lines => lines;
}