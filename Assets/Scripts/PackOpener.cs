using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardOpenAnimation : MonoBehaviour
{
    [SerializeField] Button Pack;
    [SerializeField] Button[] Cards;
    [SerializeField] RectTransform[] CardTargets; // Deðiþiklik burada

    public float moveSpeed = 800f; // UI'da daha yüksek olmalý

    private void Start()
    {
        Pack.onClick.AddListener(OpenAnimation);
    }

    private void OpenAnimation()
    {
        float duration = 0.6f;

        for (int i = 0; i < Cards.Length; i++)
        {
            int index = i;
            Cards[index].onClick.AddListener(() => OpenCards(Cards[index]));
            Cards[i].gameObject.SetActive(true);
            RectTransform cardRect = Cards[i].GetComponent<RectTransform>();
            StartCoroutine(MoveCard(cardRect, CardTargets[i].anchoredPosition, duration));
        }

        RectTransform PackRect = Pack.GetComponent<RectTransform>();
        StartCoroutine(MoveCard(PackRect, CardTargets[5].anchoredPosition, 1.0f));
    }


    IEnumerator MoveCard(RectTransform card, Vector2 targetPos, float duration)
    {
        Vector2 startPos = card.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            card.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        card.anchoredPosition = targetPos;
    }

    private void OpenCards(Button card)
    {
        StartCoroutine(FlipCard(card));
    }

    IEnumerator FlipCard(Button card)
    {
        RectTransform rect = card.GetComponent<RectTransform>();
        Image backImage = card.GetComponent<Image>();
        Transform frontTransform = card.transform.Find("Front");
        Image front = frontTransform.GetComponent<Image>();

        if (backImage == null || front == null)
        {
            Debug.LogWarning("Kartta image veya FrontContent eksik");
            yield break;
        }

        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float scale = Mathf.Lerp(1f, 0f, t);
            rect.localScale = new Vector3(scale, 1f, 1f);
            yield return null;
        }

        backImage.enabled = false;
        front.gameObject.SetActive(true);

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float scale = Mathf.Lerp(0f, 1f, t);
            rect.localScale = new Vector3(scale, 1f, 1f);
            yield return null;
        }

        rect.localScale = Vector3.one;
        card.interactable = false;
    }

}
