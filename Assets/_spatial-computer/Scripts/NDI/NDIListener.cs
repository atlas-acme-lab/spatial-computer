using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Klak.Ndi;
using System.Linq;

public class NDIListener : MonoBehaviour
{

    [SerializeField]
    NdiResources _resources = null;
    
    [SerializeField]
    GameObject _activeSpeakerObject;
    
    [SerializeField]
    RenderTexture _activeSpeakerTexture;
    
    [SerializeField]
    string _speakerString = "";
    
    NdiReceiver _speakerReceiver;

    
    [SerializeField]
    GameObject _activeScreenObject;
    
    [SerializeField]
    RenderTexture _activeScreenTexture;
    
    [SerializeField]
    string _screenString = "";
    
    NdiReceiver _screenReceiver;


    List<string> _sourceNames;
    float _checkTime = 2f;

    // Start is called before the first frame update
    void Start()
    {                
        StartCoroutine(CheckInterval(_checkTime));
        StartCoroutine(StartNDIStream());
    }

    IEnumerator CheckInterval(float timer)
    {
        _sourceNames = NdiFinder.sourceNames.ToList();
        Debug.Log("Active Sources: " + _sourceNames.Count.ToString());
        for(int i = 0; i< _sourceNames.Count; i++)
        {
            Debug.Log("Source " + i.ToString() + ": " + _sourceNames[i]);
        }
        yield return new WaitForSeconds(timer);
        StartCoroutine(CheckInterval(_checkTime));
    }

    IEnumerator StartNDIStream()
    {
        yield return new WaitForSeconds(5f);
        StartNDI();
    }

    public void StartNDI()
    {
        _speakerReceiver = new NdiReceiver();
        _speakerReceiver = _activeSpeakerObject.AddComponent<NdiReceiver>();
        _speakerReceiver.ndiName = _speakerString;
        _speakerReceiver.SetResources(_resources);
        _speakerReceiver.targetTexture = _activeSpeakerTexture;

        _screenReceiver = new NdiReceiver();
        _screenReceiver = _activeScreenObject.AddComponent<NdiReceiver>();
        _screenReceiver.ndiName = _screenString;
        _screenReceiver.SetResources(_resources);
        _screenReceiver.targetTexture = _activeScreenTexture;     
    }

}
