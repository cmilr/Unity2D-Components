using System;
using UnityEngine;

static class ExtensionMethods
{

	// I N T   &   F L O A T   E X T E N S I O N S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	static public int GetNearestMultiple(this int value, int multiple)
	{
		int rem = value % multiple;
		int result = value - rem;
		if (rem > (multiple / 2))
			result += multiple;

		return result;
	}


	// G A M E - O B J E C T   E X T E N S I O N S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	static public void SendEvent(this GameObject go, string eventType)
	{
		go.SendMessage(eventType);
	}

	static public void SendEvent<T>(this GameObject go, string eventType, T arg1)
	{
		go.SendMessage(eventType, arg1);
	}

	static public void SendEventUp(this GameObject go, string eventType)
	{
		go.SendMessageUpwards(eventType);
	}

	static public void SendEventUp<T>(this GameObject go, string eventType, T arg1)
	{
		go.SendMessageUpwards(eventType, arg1);
	}

	static public void SendEventDown(this GameObject go, string eventType)
	{
		go.BroadcastMessage(eventType);
	}

	static public void SendEventDown<T>(this GameObject go, string eventType, T arg1)
	{
		go.BroadcastMessage(eventType, arg1);
	}

	static public void SendEventToParent(this GameObject go, string eventType)
	{
		go.transform.parent.gameObject.SendMessage(eventType);
	}

	static public void SendEventToParent<T>(this GameObject go, string eventType, T arg1)
	{
		go.transform.parent.gameObject.SendMessage(eventType, arg1);
	}

	static public void SendEventToParentAndDown(this GameObject go, string eventType)
	{
		go.transform.parent.gameObject.BroadcastMessage(eventType);
	}

	static public void SendEventToParentAndDown<T>(this GameObject go, string eventType, T arg1)
	{
		go.transform.parent.gameObject.BroadcastMessage(eventType, arg1);
	}



	// T R A N S F O R M   E X T E N S I O N S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public static void SetPositionX(this Transform transform, float x)
	{
		transform.position = new Vector3(x, transform.position.y, transform.position.z);
	}

	public static void SetPositionY(this Transform transform, float y)
	{
		transform.position = new Vector3(transform.position.x, y, transform.position.z);
	}

	public static void SetPositionZ(this Transform transform, float z)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, z);
	}

	public static void SetPositionXY(this Transform transform, float x, float y)
	{
		transform.position = new Vector3(x, y, transform.position.z);
	}

	public static void SetPosition(this Transform transform, float x, float y, float z)
	{
		transform.position = new Vector3(x, y, z);
	}

	public static void SetLocalPositionX(this Transform transform, float x)
	{
		transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
	}

	public static void SetLocalPositionY(this Transform transform, float y)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
	}

	public static void SetLocalPositionZ(this Transform transform, float z)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
	}

	public static void SetLocalPositionXY(this Transform transform, float x, float y)
	{
		transform.localPosition = new Vector3(x, y, transform.localPosition.z);
	}

	public static void SetLocalPosition(this Transform transform, float x, float y, float z)
	{
		transform.localPosition = new Vector3(x, y, z);
	}

	public static void SetAbsLocalPositionX(this Transform transform, float x)
	{
		if (transform.lossyScale.x > 0f)
		{
			transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
		}
		else
		{
			transform.localPosition = new Vector3(-x, transform.localPosition.y, transform.localPosition.z);
		}
	}

	public static void SetLocalScaleX(this Transform transform, float x)
	{
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
	}

	public static void SetLocalScale(this Transform transform, float x, float y, float z)
	{
		transform.localScale = new Vector3(x, y, z);
	}

	public static void SetAbsLocalScaleX(this Transform transform, float x)
	{
		if (transform.lossyScale.x > 0f)
		{
			transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
		}
		else
		{
			transform.localScale = new Vector3(-x, transform.localScale.y, transform.localScale.z);
		}
	}


	// A N I M A T O R   E X T E N S I O N S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public static bool IsMidLoop(this Animator thisAnimator)
	{
		return (thisAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 < .80f);
	}

	public static int GetNameHash(this Animator thisAnimator)
	{
		return (thisAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash);
	}

	public static void PlayClip(this Animator thisAnimator, int clip, float speed)
	{
		thisAnimator.Play(clip);
		thisAnimator.SetFloat("VariableSpeed", speed);
	}


	// F L O A T I N G   P O I N T   E X T E N S I O N S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public static bool FloatEquals(this float num1, float num2, float threshold = .0001f)
	{
		return Math.Abs(num1 - num2) < threshold;
	}

	public static bool DoubleEquals(this double num1, double num2, double threshold = .0001f)
	{
		return Math.Abs(num1 - num2) < threshold;
	}


	// R I G I D B O D Y   E X T E N S I O N S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
	{
		Vector3 dir = (body.transform.position - explosionPosition);
		float wearoff = 1 - (dir.magnitude / explosionRadius);
		body.AddForce(dir.normalized * explosionForce * wearoff);
	}

	public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
	{
		Vector3 dir = (body.transform.position - explosionPosition);
		float wearoff = 1 - (dir.magnitude / explosionRadius);
		Vector3 baseForce = dir.normalized * explosionForce * wearoff;
		body.AddForce(baseForce);

		float upliftWearoff = 1 - upliftModifier / explosionRadius;
		Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
		body.AddForce(upliftForce);
	}
}
