using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMarker : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
            SceneLoader.Instance.ReloadCurrentSceneAsync();
    }
}
