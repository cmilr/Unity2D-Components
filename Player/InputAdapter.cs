using UnityEngine;

// if Standalone or Editor build, process Unity input manager.
// if mobile build, bypass.

public class InputAdapter : BaseBehaviour
{
	//private Player playerControls;
	private ICreatureController creature;

	#if UNITY_EDITOR
	void Start()
	{
		CacheReferences();
	}

	void Update()
	{
		PlatformSpecificUpdate();
	}
	#endif

	#if !UNITY_EDITOR
	#if UNITY_STANDALONE
	void Start()
	{
		CacheReferences();
	}

	void Update()
	{
		PlatformSpecificUpdate();
	}
	#endif
	#endif

	void CacheReferences()
	{
		creature = GetComponent<ICreatureController>();
	}

	void PlatformSpecificUpdate()
	{
		float h;
		
		h = Input.GetAxisRaw("Horizontal");

		if (h > 0)
		{
			creature.MoveRight();
		}

		if (h < 0)
		{
			creature.MoveLeft();
		}

		if (Input.GetButtonDown("Jump"))
		{
			creature.Jump();
		}

		if (Input.GetButtonDown("Attack"))
		{
			creature.Attack();
		}

		if (Input.GetButtonDown("Switch"))
		{
			EventKit.Broadcast("switch weapon", Side.Right);
		}
	}
}
