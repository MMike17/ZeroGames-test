using UnityEngine;

/// <summary>Indicator to show where the player destination was set</summary>
public class TargetIndicator : BaseBehaviour
{
	[Header("Settings")]
	public float startFadeDistance;
	public float endFadeDistance;

	[Header("Scene references")]
	public SpriteRenderer groundRenderer;
	public Transform diamondIndicator;

	Transform mainCamera, player;
	SpriteRenderer diamondRenderer;

	public void Init(Transform camera, Transform player)
	{
		mainCamera = camera;
		this.player = player;

		diamondRenderer = diamondIndicator.GetComponent<SpriteRenderer>();

		InitInternal();
	}

	void Update()
	{
		ManageOrientation();
		ManageFade();
	}

	void ManageOrientation()
	{
		diamondIndicator.LookAt(mainCamera);
		diamondIndicator.rotation = Quaternion.Euler(0, diamondIndicator.eulerAngles.y, 90);
	}

	void ManageFade()
	{
		float currentAlpha = 1;
		float playerDistance = Vector3.Distance(transform.position, player.position);

		// should fade
		if(playerDistance < startFadeDistance)
		{
			// should be fully faded
			if(playerDistance < endFadeDistance)
				currentAlpha = 0;
			else
				currentAlpha = (playerDistance - endFadeDistance) / (startFadeDistance - endFadeDistance);
		}

		SetSpriteAlpha(groundRenderer, currentAlpha);
		SetSpriteAlpha(diamondRenderer, currentAlpha);
	}

	void SetSpriteAlpha(SpriteRenderer renderer, float alpha)
	{
		UnityEngine.Color color = renderer.color;
		color.a = alpha;
		renderer.color = color;
	}
}