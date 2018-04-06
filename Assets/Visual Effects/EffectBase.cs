using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EffectBase : MonoBehaviour
{	
	[Header("Animation Settings")]
	[SerializeField]
	protected AnimationSettings settings;

	protected RectTransform rect;

	protected Vector2 originalScale;
	protected Vector2 originalPosition;

	protected virtual void Awake ()
	{
		rect = GetComponent<RectTransform> ();
		originalPosition = (Vector2) rect.anchoredPosition;
		originalScale = (Vector2)rect.localScale;
	}

	protected virtual void Start ()
	{
		if (settings.animateFromStart)
		{
			Animate ();
		}
	}

	public virtual void Animate () { }
}

[System.Serializable]
public class AnimationSettings
{
	public Ease easeType;
	[Range (0.0f, 10.0f)]
	public float easeDuration = 1.0f;
	public bool animateFromStart = false;
	public bool loop = false;
	public LoopType loopType;
}

[System.Serializable]
public enum Direction
{
	Horizontal,
	Vertical,
	Both
};

[System.Serializable]
public class RectMenuItem
{
	public RectTransform rect;

	public UnityEngine.UI.Button button;

	public Vector2 originalPosition
	{
		get { return originalPosition; }
		set { originalPosition = value; }
	}

	public void Hide (bool right = true, bool instant = false)
	{
		rect.DOAnchorPosX (1280 * (right ? 1 : -1), instant ? 0 : 1).SetEase(Ease.OutQuad);
	}

	public void Show (bool instant = false)
	{
		rect.DOAnchorPosX (0, instant ? 0 : 1).SetEase(Ease.OutQuad);
	}
}