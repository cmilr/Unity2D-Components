using UnityEngine;
using System.Collections;


public class CacheBehaviour : BaseBehaviour
{
	[HideInInspector]
	public     Animator animator;
	[HideInInspector]
	public new AudioSource audio;
	[HideInInspector]
	public new Camera camera;
	[HideInInspector]
	public new Collider2D collider2D;
	[HideInInspector]
	public new Rigidbody2D rigidbody2D;
	[HideInInspector]
	public new Renderer renderer;
	[HideInInspector]
	public     SpriteRenderer spriteRenderer;
	[HideInInspector]
	public new Transform transform;

	public void CacheComponents()
	{
		transform = gameObject.transform;
		animator = gameObject.GetComponent<Animator>() as Animator;
		audio = gameObject.GetComponent<AudioSource>() as AudioSource;
		camera = gameObject.GetComponent<Camera>() as Camera;
		collider2D = gameObject.GetComponent<Collider2D>() as Collider2D;
		rigidbody2D = gameObject.GetComponent<Rigidbody2D>() as Rigidbody2D;
		renderer = gameObject.GetComponent<Renderer>() as Renderer;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
	}
}
