using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoSingleton<SceneLoader>
{
    public ScreenTransitionHandler ScreenTransitionHandler { get; set; }

    public AsyncOperation CurrentAsyncOperation;

    public Scene ActiveScene
    {
        get { return SceneManager.GetActiveScene(); }
    }

    private void Awake()
    {
        DefineSingleton(this, true);
    }

    public void LoadNextAsync()
    {
        StartCoroutine(LoadSceneAsync(ActiveScene.buildIndex + 1));
    }

    public void ReloadCurrentSceneAsync()
    {
        StartCoroutine(LoadSceneAsync(ActiveScene.buildIndex));
    }

    //public void LoadSceneAsync(SceneAsset sceneAsset)
    //{
    //    var sceneBuildIndex = _library.GetBuildIndex(sceneAsset);
    //    if (sceneBuildIndex == -1)
    //        throw new Exception(string.Format("The scene that is trying to be loaded is not listed in the build settings: {0}", sceneAsset.name));

    //    StartCoroutine(LoadSceneAsync(sceneBuildIndex));
    //}

    public void LoadSceneAsync(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        var sceneBuildIndex = scene.buildIndex;
        if (sceneBuildIndex == -1)
            throw new Exception(string.Format("The scene that is trying to be loaded is not listed in the build settings: {0}", scene.name));

        StartCoroutine(LoadSceneAsync(sceneBuildIndex));
    }

    public void ShowLoadScreen()
    {
        SceneManager.LoadScene(SceneLiterals.LoadScreen, LoadSceneMode.Additive);
    }

    private IEnumerator LoadSceneAsync(int buildIndex)
    {
        if (ScreenTransitionHandler != null)
        {
            ScreenTransitionHandler.AnimateOut();
            yield return new WaitForSeconds(ScreenTransitionHandler.SceneTransitionDuration);
        }
        else
        {
            // Work around: needed to make the 'allowSceneActivation' flag work. (Last tested in: 2017.4.1f1)
            yield return null;
        }

        CurrentAsyncOperation = SceneManager.LoadSceneAsync(buildIndex);
        CurrentAsyncOperation.allowSceneActivation = false;

        // The 'allowSceneActivation' flag stalls the progress at 0.9 so that the scene will not load...
        while (CurrentAsyncOperation.progress < 0.9f)
        {
            yield return null;
        }

        CurrentAsyncOperation.completed += operation =>
        {
            // todo: find a better moment in time to do this, this can be quite heavy causing a severe fps drop in the process
            var sceneRootGameObjects = SceneManager.GetSceneByBuildIndex(buildIndex).GetRootGameObjects();
            foreach (var go in sceneRootGameObjects)
            {
                var screenTransitionHandler = go.GetComponent<ScreenTransitionHandler>();
                if (screenTransitionHandler != null)
                {
                    screenTransitionHandler.AnimateIn();
                    break;
                }
            }
        };

        CurrentAsyncOperation.allowSceneActivation = true;
    }
}
