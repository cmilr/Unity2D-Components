using UnityEngine;

// if Standalone or Editor build, process Unity input manager.
// if mobile build, bypass.

public class InputAdapter : BaseBehaviour
{
	//private Player playerControls;
	private ICreatureController creature;

	void Start()
	{
		
	#if UNITY_EDITOR
		CacheReferences();
	#endif
			
	#if UNITY_STANDALONE
		CacheReferences();
	#endif
		
	}

	void Update()
	{
		
	#if UNITY_EDITOR
		PlatformSpecificUpdate();
	#endif
	
	#if UNITY_STANDALONE
		PlatformSpecificUpdate();
	#endif

	}

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
