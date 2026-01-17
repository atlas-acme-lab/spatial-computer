using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Switches to a specified scene.
/// </summary>
public class SwitchScene : MonoBehaviour
{

    [SerializeField]
    private int _buildIndex = 1;
    
    public void LoadScene()
    {
        SceneManager.LoadScene(_buildIndex, LoadSceneMode.Single);
    }

}
