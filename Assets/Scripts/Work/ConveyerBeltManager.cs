using UnityEngine;
using System.Collections;

public class ConveyerBeltManager : MonoBehaviour
{
    public static ConveyerBeltManager Instance{get; private set;}
    void Awake()
    {
        if(Instance==null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public Material targetMaterial;
    public float offsetIncrementAmount = 0.05f;
    public float updateInterval = 0.05f;
    public float duration = 5f;

    private Coroutine offsetCoroutine;

    public void RunConveyerBelt(float amount)
    {
        offsetIncrementAmount = amount;
        if (offsetCoroutine != null)
        {
            StopCoroutine(offsetCoroutine);
        }
        offsetCoroutine = StartCoroutine(IncreaseYOffsetOverTime());
    }

    private IEnumerator IncreaseYOffsetOverTime()
    {
        if (targetMaterial == null)
        {
            Debug.LogWarning("Target material is not assigned!");
            yield break;
        }

        float elapsedTime = 0f;
        Vector2 initialOffset = targetMaterial.mainTextureOffset;
        float targetYOffset = initialOffset.y + offsetIncrementAmount;

        while (elapsedTime < duration)
        {
            elapsedTime += updateInterval;
            float t = Mathf.Clamp01(elapsedTime / duration);
            
            Vector2 newOffset = targetMaterial.mainTextureOffset;
            newOffset.y = Mathf.Lerp(initialOffset.y, targetYOffset, t);
            targetMaterial.mainTextureOffset = newOffset;

            yield return new WaitForSeconds(updateInterval);
        }

        // Ensure we reach the exact target offset
        Vector2 finalOffset = targetMaterial.mainTextureOffset;
        finalOffset.y = targetYOffset;
        targetMaterial.mainTextureOffset = finalOffset;
    }
}