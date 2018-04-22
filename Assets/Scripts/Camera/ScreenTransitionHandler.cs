using DG.Tweening;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenTransitionHandler : MonoSingleton<ScreenTransitionHandler>
{
    [SerializeField] private Color _dieingTransitionColor;
    [SerializeField] private Material _prototypeTransitionMaterial;

    public Material TransitionMaterial { get; private set; }

    public float SceneTransitionDuration = 0.45f;

    private void Awake()
    {
        DefineSingleton(this);

        // Create a duplicate material so we don't modify the global material shader values
        TransitionMaterial = Instantiate(_prototypeTransitionMaterial);
    }

    private void Start()
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.ScreenTransitionHandler = this;
    }

    public void IsDieing()
    {
        TransitionMaterial.SetColor("_TransitionColor", _dieingTransitionColor);
    }

    public void AnimateIn()
    {
        DOVirtual.Float(1f, 0f, SceneTransitionDuration,
            value => TransitionMaterial.SetFloat("_Cutoff", value));
    }

    public void AnimateOut()
    {
        DOVirtual.Float(0f, 1f, SceneTransitionDuration,
            value => TransitionMaterial.SetFloat("_Cutoff", value));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(TransitionMaterial != null)
            Graphics.Blit(source, destination, TransitionMaterial);
    }
}
