#if UnityEditor
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathLogger
{
    [MenuItem("Assets/Log Asset Path")]
    private static void LogAssetAtPath()
    {
        var selected = Selection.activeObject;
        Debug.Log(AssetDatabase.GetAssetPath(selected));
    }
}
#endif