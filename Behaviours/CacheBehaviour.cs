using UnityEngine;
using System;
using System.Collections;


public class CacheBehaviour : BaseBehaviour
{
	[HideInInspector, NonSerialized]
	protected new Animation animation;
	[HideInInspector, NonSerialized]
	protected new Animator animator;
	[HideInInspector, NonSerialized]
	protected new AudioSource audio;
	[HideInInspector, NonSerialized]
	protected new Camera camera;
	[HideInInspector, NonSerialized]
	protected new Collider2D collider2D;
	[HideInInspector, NonSerialized]
	protected new Light light;
	[HideInInspector, NonSerialized]
	protected new ParticleEmitter particleEmitter;
	[HideInInspector, NonSerialized]
	protected new ParticleSystem particleSystem;
	[HideInInspector, NonSerialized]
	protected new Rigidbody2D rigidbody2D;
	[HideInInspector, NonSerialized]
	protected new Renderer renderer;
	[HideInInspector, NonSerialized]
	protected new SpriteRenderer spriteRenderer;
	[HideInInspector, NonSerialized]
	protected new Transform transform;

	protected void CacheComponents()
	{
		transform = gameObject.transform;
		animation = gameObject.GetComponent<Animation>() as Animation;
		animator = gameObject.GetComponent<Animator>() as Animator;
		audio = gameObject.GetComponent<AudioSource>() as AudioSource;
		camera = gameObject.GetComponent<Camera>() as Camera;
		collider2D = gameObject.GetComponent<Collider2D>() as Collider2D;
		light = gameObject.GetComponent<Light>() as Light;
		particleEmitter = gameObject.GetComponent<ParticleEmitter>() as ParticleEmitter;
		particleSystem = gameObject.GetComponent<ParticleSystem>() as ParticleSystem;
		rigidbody2D = gameObject.GetComponent<Rigidbody2D>() as Rigidbody2D;
		renderer = gameObject.GetComponent<Renderer>() as Renderer;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
	}
}
