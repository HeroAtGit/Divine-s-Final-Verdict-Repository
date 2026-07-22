using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Dialouge
{
    public string[] names;
    [TextArea(3, 10)]
    public string[] sentences;
    public Image[] images;
}
