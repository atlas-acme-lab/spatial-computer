using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransitionHandler : MonoBehaviour
{

    [SerializeField]
    private Image _canvas;

    Color _greyColor;

    private void Awake()
    {
        InitializeToScreen();
    }

    public void InitializeToScreen()
    {
        _greyColor = _canvas.color;
    }

    public void Transition(bool state)
    {
        StartCoroutine(TransitionLerp(state));
    }

    IEnumerator TransitionLerp(bool state)
    {
        float time = 0;


        yield return new WaitForSeconds(0.5f);


        Color _startCol = state? _greyColor : Color.clear;
        Color _endCol = state? Color.clear : _greyColor;


        while (time < 2f)
        {

            _canvas.color = Color.Lerp(_startCol, _endCol, time);

            time += Time.deltaTime;
            yield return null;
        }

        _canvas.color = _endCol;

    }

}
