using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Scaler : EffectBase
{
	[SerializeField]
	private Vector2 ScaleOffset = Vector2.one;

	public override void Animate ()
	{
		rect.DOScale (originalScale + ScaleOffset, settings.easeDuration).SetEase (settings.easeType).SetLoops (settings.loop ? -1 : 0, settings.loopType);
	}
}