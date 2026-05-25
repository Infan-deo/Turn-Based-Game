using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using Sirenix.OdinInspector;

public class CircleFadeTransition : Singleton<CircleFadeTransition>
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private float transitionDuration = 1f;

    private VisualElement blackout;

    private const float CircleSize = 3000f;

    IEnumerator Start()
    {
        var root = uiDocument.rootVisualElement;

        blackout = root.Q<VisualElement>("blackout-circle");

        yield return new WaitForSeconds(0.5f);

        NormalFadeOut();
    }

    // =========================
    // Circle Transition
    // =========================

    [Button]
    public void CircleFadeIn()
    {
        StopAllCoroutines();

        PrepareCircle();

        StartCoroutine(AnimateCircle(0, CircleSize, false));
    }

    [Button]
    public void CircleFadeOut()
    {
        StopAllCoroutines();

        PrepareCircle();

        StartCoroutine(AnimateCircle(CircleSize, 0, true));
    }

    private IEnumerator AnimateCircle(float startSize, float endSize, bool disableAfter)
    {
        float time = 0;

        while (time < transitionDuration)
        {
            time += Time.deltaTime;

            float t = Mathf.SmoothStep(0, 1, time / transitionDuration);

            float size = Mathf.Lerp(startSize, endSize, t);

            blackout.style.width = size;
            blackout.style.height = size;

            blackout.style.marginLeft = -size / 2;
            blackout.style.marginTop = -size / 2;

            yield return null;
        }

        if (disableAfter)
        {
            blackout.style.display = DisplayStyle.None;
        }
    }

    // =========================
    // Normal Fade
    // =========================

    [Button]
    public void NormalFadeIn()
    {
        StopAllCoroutines();

        PrepareFullscreen();

        StartCoroutine(FadeRoutine(0, 1, false));
    }

    [Button]
    public void NormalFadeOut()
    {
        StopAllCoroutines();

        PrepareFullscreen();

        StartCoroutine(FadeRoutine(1, 0, true));
    }

    private IEnumerator FadeRoutine(float start, float end, bool disableAfter)
    {
        float time = 0;

        while (time < transitionDuration)
        {
            time += Time.deltaTime;

            float t = Mathf.SmoothStep(0, 1, time / transitionDuration);

            float alpha = Mathf.Lerp(start, end, t);

            blackout.style.opacity = alpha;

            yield return null;
        }

        blackout.style.opacity = end;

        if (disableAfter)
        {
            blackout.style.display = DisplayStyle.None;
        }
    }

    // =========================
    // Helpers
    // =========================

   private void PrepareFullscreen()
{
    blackout.style.display = DisplayStyle.Flex;

    blackout.style.opacity = 1;

    // FULLSCREEN
    blackout.style.left = 0;
    blackout.style.top = 0;
    blackout.style.right = 0;
    blackout.style.bottom = 0;

    // REMOVE CIRCLE SIZE
    blackout.style.width = StyleKeyword.Auto;
    blackout.style.height = StyleKeyword.Auto;

    // REMOVE CENTERING
    blackout.style.marginLeft = 0;
    blackout.style.marginTop = 0;

    // REMOVE ROUND SHAPE
    blackout.style.borderTopLeftRadius = 0;
    blackout.style.borderTopRightRadius = 0;
    blackout.style.borderBottomLeftRadius = 0;
    blackout.style.borderBottomRightRadius = 0;
}

   private void PrepareCircle()
{
    blackout.style.display = DisplayStyle.Flex;

    blackout.style.opacity = 1;

    // CENTER
    blackout.style.left = new Length(50, LengthUnit.Percent);
    blackout.style.top = new Length(50, LengthUnit.Percent);

    blackout.style.right = StyleKeyword.Null;
    blackout.style.bottom = StyleKeyword.Null;

    // MAKE CIRCLE
    blackout.style.borderTopLeftRadius = 9999;
    blackout.style.borderTopRightRadius = 9999;
    blackout.style.borderBottomLeftRadius = 9999;
    blackout.style.borderBottomRightRadius = 9999;
}
}