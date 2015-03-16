// 2014 - Pixelnest Studio
using System;
using System.Collections;
using UnityEngine;
 
/// <summary>
/// Ready to use timers for coroutines
/// </summary>
public class Timer
{
	private bool repeat;
	private bool stop;
	private float duration;
	private Action callback;
	 
	public Timer(float duration, bool repeat, Action callback)
	{
		this.stop = false;
		this.repeat = repeat;
		this.duration = duration;
		this.callback = callback;
	}
	 
	/// <summary>
	/// Start in co-routine
	/// </summary>
	/// <returns></returns>
	public IEnumerator Start()
	{
		do
		{
			if (stop == false)
			{
				yield return new WaitForSeconds(duration);
			 
				if (callback != null)
				callback();
			}
		} while (repeat && !stop);
	}
	 
	/// <summary>
	/// Stop the timer (at next frame)
	/// </summary>
	public void Stop()
	{
		this.stop = true;
	}
	 
	/// <summary>
	/// Simple timer, no reference, wait and then execute something
	/// </summary>
	/// <param name="duration"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	public static IEnumerator Start(float duration, Action callback)
	{
		return Start(duration, false, callback);
	}
	 
	 
	/// <summary>
	/// Simple timer, no reference, wait and then execute something
	/// </summary>
	/// <param name="duration"></param>
	/// <param name="repeat"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	public static IEnumerator Start(float duration, bool repeat, Action callback)
	{
		Timer timer = new Timer(duration, repeat, callback);
		return timer.Start();
	}
	 
	public bool Repeat
	{
		get { return repeat; }
	}
}