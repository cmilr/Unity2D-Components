//   _  _ ____ ___ ____ _  _ ____
//   |\/| |--|  |  |___ |--| |--|   This version of CacheBehaviour was modified by Cary Miller.
//   i  n  d  u  s  t  r  i  e  s   See below for original Copyright and Disclaimer info.

/*
 * Copyright (c) 2014, Nick Gravelyn.
 *
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 *    1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 *
 *    2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 *
 *    3. This notice may not be removed or altered from any source
 *    distribution.
 */

using UnityEngine;
using System;

/// <summary>
/// A safe, drop-in replacement for MonoBehaviour as your base class. Each property value is cached
/// and GetComponent will be called if the instance is null or is destroyed.
/// </summary>
public abstract class CacheBehaviour : BaseBehaviour
{
    [HideInInspector, NonSerialized]
    private Animation _animation;
    public new Animation animation { get { return _animation ? _animation : (_animation = GetComponent<Animation>()); } }

	[HideInInspector, NonSerialized]
    private Animator _animator;
    public Animator animator { get { return _animator ? _animator : (_animator = GetComponent<Animator>()); } }

    [HideInInspector, NonSerialized]
    private AudioSource _audio;
    public new AudioSource audio { get { return _audio ? _audio : (_audio = GetComponent<AudioSource>()); } }

    [HideInInspector, NonSerialized]
    private Camera _camera;
    public new Camera camera { get { return _camera ? _camera : (_camera = GetComponent<Camera>()); } }

    [HideInInspector, NonSerialized]
    private Collider _collider;
    public new Collider collider { get { return _collider ? _collider : (_collider = GetComponent<Collider>()); } }

    [HideInInspector, NonSerialized]
    private Collider2D _collider2D;
    public new Collider2D collider2D { get { return _collider2D ? _collider2D : (_collider2D = GetComponent<Collider2D>()); } }

    [HideInInspector, NonSerialized]
    private ConstantForce _constantForce;
    public new ConstantForce constantForce { get { return _constantForce ? _constantForce : (_constantForce = GetComponent<ConstantForce>()); } }

    [HideInInspector, NonSerialized]
    private GUIText _guiText;
    public new GUIText guiText { get { return _guiText ? _guiText : (_guiText = GetComponent<GUIText>()); } }

    [HideInInspector, NonSerialized]
    private GUITexture _guiTexture;
    public new GUITexture guiTexture { get { return _guiTexture ? _guiTexture : (_guiTexture = GetComponent<GUITexture>()); } }

    [HideInInspector, NonSerialized]
    private HingeJoint _hingeJoint;
    public new HingeJoint hingeJoint { get { return _hingeJoint ? _hingeJoint : (_hingeJoint = GetComponent<HingeJoint>()); } }

    [HideInInspector, NonSerialized]
    private Light _light;
    public new Light light { get { return _light ? _light : (_light = GetComponent<Light>()); } }

    [HideInInspector, NonSerialized]
    private NetworkView _networkView;
    public new NetworkView networkView { get { return _networkView ? _networkView : (_networkView = GetComponent<NetworkView>()); } }

    [HideInInspector, NonSerialized]
    private ParticleEmitter _particleEmitter;
    public new ParticleEmitter particleEmitter { get { return _particleEmitter ? _particleEmitter : (_particleEmitter = GetComponent<ParticleEmitter>()); } }

    [HideInInspector, NonSerialized]
    private ParticleSystem _particleSystem;
    public new ParticleSystem particleSystem { get { return _particleSystem ? _particleSystem : (_particleSystem = GetComponent<ParticleSystem>()); } }

    [HideInInspector, NonSerialized]
    private Renderer _renderer;
    public new Renderer renderer { get { return _renderer ? _renderer : (_renderer = GetComponent<Renderer>()); } }

    [HideInInspector, NonSerialized]
    private Rigidbody _rigidbody;
    public new Rigidbody rigidbody { get { return _rigidbody ? _rigidbody : (_rigidbody = GetComponent<Rigidbody>()); } }

    [HideInInspector, NonSerialized]
    private Rigidbody2D _rigidbody2D;
    public new Rigidbody2D rigidbody2D { get { return _rigidbody2D ? _rigidbody2D : (_rigidbody2D = GetComponent<Rigidbody2D>()); } }

	[HideInInspector, NonSerialized]
    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer spriteRenderer { get { return _spriteRenderer ? _spriteRenderer : (_spriteRenderer = GetComponent<SpriteRenderer>()); } }

    [HideInInspector, NonSerialized]
    private Transform _transform;
    public new Transform transform { get { return _transform ? _transform : (_transform = GetComponent<Transform>()); } }
}
