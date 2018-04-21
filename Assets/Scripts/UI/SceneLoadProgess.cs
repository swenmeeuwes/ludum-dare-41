using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SceneLoadProgess : MonoBehaviour
{
    private Slider _slider;

    // Correction modifier because the 'allowSceneActivation' flag in SceneLoader
    // will stall the progress at 0.9 (this means that 0.9 = 100%)
    private const float _progressCorrection = 1f / 0.9f;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (SceneLoader.Instance != null && SceneLoader.Instance.CurrentAsyncOperation != null)
        {
            // todo: lerp progress for a smooth looking loading process            
            _slider.value = SceneLoader.Instance.CurrentAsyncOperation.progress * _progressCorrection;
        }
    }
}
