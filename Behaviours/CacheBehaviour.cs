using UnityEngine;
using System.Collections;


public class CacheBehaviour : BaseBehaviour
{
	protected     Animator animator;
	protected new AudioSource audio;
	protected new Camera camera;
	protected new Collider2D collider2D;
	protected new Rigidbody2D rigidbody2D;
	protected new Renderer renderer;
	protected     SpriteRenderer spriteRenderer;
	protected new Transform transform;

	protected void CacheComponents()
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
