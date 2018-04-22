using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Animator _textBlinkAnimator;

    private bool _isStarting;

    private void Start()
    {
        SoundManager.Instance.PlayBgm(Sound.BGM);
    }

    private void Update()
    {
        if (Input.anyKey)
            StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        if (_isStarting)
            yield break;

        SoundManager.Instance.Play(Sound.StartGame);

        _isStarting = true;

        _textBlinkAnimator.speed = 5;

        yield return new WaitForSeconds(0.65f);

        SoundManager.Instance.SetBgmVolume(0.5f);

        SceneLoader.Instance.LoadNextAsync();
    }
}
