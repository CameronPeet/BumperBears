using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EffectBase : MonoBehaviour
{
	[SerializeField]
	protected AnimationSettings settings;

	// [SerializeField]
	protected RectTransform rect;

	protected Vector2 originalPosition;

	private void Awake ()
	{
		rect = GetComponent<RectTransform> ();
		originalPosition = (Vector2) rect.anchoredPosition;
	}

	void Start ()
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