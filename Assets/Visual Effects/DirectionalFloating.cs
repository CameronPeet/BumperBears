using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DirectionalFloating : EffectBase
{
	[SerializeField]
	public Direction direction = Direction.Both;

	[SerializeField]
	private Vector2 MoveOffset = Vector2.zero;

	void Start ()
	{
		Animate();

	}

	public override void Animate ()
	{
		rect.DOAnchorPos (originalPosition + MoveOffset, settings.easeDuration).SetEase (settings.easeType).SetLoops (settings.loop ? -1 : 0, settings.loopType);
	}

	void OnValidate ()
	{
		switch (direction)
		{
			case Direction.Horizontal:
				MoveOffset.y = 0;
				break;
			case Direction.Vertical:
				MoveOffset.x = 0;
				break;
		}
	}
}