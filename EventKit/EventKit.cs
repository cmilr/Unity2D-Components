// EventKit.cs is a very slightly modified version of the excellent Advanced C# Messenger by Ilya Suzdalnitski,
// which is itself based on Rod Hyde's "CSharpMessenger" and Magnus Wolffelt's "CSharpMessenger Extended".
//
// It's been updated to allow for more parameters in events, and its name and the names of some of its functions
// have been changed to better reflect the way I use it personally. Other than that, the script is mostly unchanged.
//
// Features:
// Prevents a MissingReferenceException because of a reference to a destroyed message handler.
// Option to log all messages
// Extensive error detection, preventing silent bugs
//
// Usage examples:
// 1. EventKit.Subscribe<GameObject>("prop collected", PropCollected);
//    EventKit.Broadcast<GameObject>("prop collected", prop);
// 2. EventKit.Subscribe<float>("speed changed", SpeedChanged);
//    EventKit.Broadcast<float>("speed changed", 0.5f);
//
// EventKit cleans up its evenTable automatically upon loading of a new level.
//
// Don't forget that the event that should survive the cleanup, should be marked with EventKit.MarkAsPermanent(string)

//#define LOG_ALL_MESSAGES
//#define LOG_ADD_LISTENER
//#define LOG_BROADCAST_MESSAGE
//#define REQUIRE_LISTENER

using System.Collections.Generic;
using System;
using UnityEngine;

static internal class EventKit
{
	#region Internal variables

	//Disable the unused variable warning
#pragma warning disable 0414
	//Ensures that the EventKitHelper will be created automatically upon start of the game.
	static private EventKitHelper messengerHelper = (new GameObject("EventKitHelper")).AddComponent< EventKitHelper >();
#pragma warning restore 0414

	static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	//Message handlers that should never be removed, regardless of calling Cleanup
	static public List< string > permanentMessages = new List< string > ();
	#endregion
	#region Helper methods
	//Marks a certain message as permanent.
	static public void MarkAsPermanent(string eventType)
	{
#if LOG_ALL_MESSAGES
		Debug.Log("EventKit MarkAsPermanent \t\"" + eventType + "\"");
#endif

		permanentMessages.Add(eventType);
	}


	static public void Cleanup()
	{
#if LOG_ALL_MESSAGES
		Debug.Log("EventKit Cleanup. Make sure that none of necessary listeners are removed.");
#endif

		List< string > messagesToRemove = new List<string>();

		foreach (KeyValuePair<string, Delegate> pair in eventTable) {
			bool wasFound = false;

			foreach (string message in permanentMessages) {
				if (pair.Key == message) {
					wasFound = true;
					break;
				}
			}

			if (!wasFound) {
				messagesToRemove.Add(pair.Key);
			}
		}

		// commented out to force all messages to be permanent
		// foreach (string message in messagesToRemove) {
		//    eventTable.Remove( message );
		// }
	}

	static public void PrintEventTable()
	{
		Debug.Log("\t\t\t=== EventKit PrintEventTable ===");

		foreach (KeyValuePair<string, Delegate> pair in eventTable) {
			Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
		}

		Debug.Log("\n");
	}
	#endregion

	#region Message logging and exception throwing
	static public void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
	{
#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
		Debug.Log("EventKit OnListenerAdding \t\"" + eventType + "\"\t{" + listenerBeingAdded.Target + " -> " + listenerBeingAdded.Method + "}");
#endif

		if (!eventTable.ContainsKey(eventType)) {
			eventTable.Add(eventType, null);
		}

		Delegate d = eventTable[eventType];

		if (d != null && d.GetType() != listenerBeingAdded.GetType()) {
			throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
		}
	}

	static public void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
	{
#if LOG_ALL_MESSAGES
		Debug.Log("EventKit OnListenerRemoving \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}");
#endif

		if (eventTable.ContainsKey(eventType)) {
			Delegate d = eventTable[eventType];

			if (d == null) {
				throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
			}
			else if (d.GetType() != listenerBeingRemoved.GetType())
			{
				throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
			}
		}
		else
		{
			throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
		}
	}

	static public void OnListenerRemoved(string eventType)
	{
		if (eventTable[eventType] == null) {
			eventTable.Remove(eventType);
		}
	}

	static public void OnBroadcasting(string eventType)
	{
#if REQUIRE_LISTENER

		if (!eventTable.ContainsKey(eventType)) {
			throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with EventKit.MarkAsPermanent.", eventType));
		}
#endif
	}

	static public BroadcastException CreateBroadcastSignatureException(string eventType)
	{
		return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
	}

	public class BroadcastException : Exception
	{
		public BroadcastException(string msg)
			: base(msg)
		{
		}
	}

	public class ListenerException : Exception
	{
		public ListenerException(string msg)
			: base(msg)
		{
		}
	}
	#endregion

	#region Subscribe
	//No parameters
	static public void Subscribe(string eventType, Callback handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback)eventTable[eventType] + handler;
	}

	//Single parameter
	static public void Subscribe<T>(string eventType, Callback<T> handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
	}

	//Two parameters
	static public void Subscribe<T, U>(string eventType, Callback<T, U> handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
	}

	//Three parameters
	static public void Subscribe<T, U, V>(string eventType, Callback<T, U, V> handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] + handler;
	}

	//Four parameters
	static public void Subscribe<T, U, V, W>(string eventType, Callback<T, U, V, W> handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback<T, U, V, W>)eventTable[eventType] + handler;
	}

	//Five parameters
	static public void Subscribe<T, U, V, W, X>(string eventType, Callback<T, U, V, W, X> handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback<T, U, V, W, X>)eventTable[eventType] + handler;
	}
	#endregion

	#region Unsubscribe
	//No parameters
	static public void Unsubscribe(string eventType, Callback handler)
	{
		OnListenerRemoving(eventType, handler);
		eventTable[eventType] = (Callback)eventTable[eventType] - handler;
		OnListenerRemoved(eventType);
	}

	//Single parameter
	static public void Unsubscribe<T>(string eventType, Callback<T> handler)
	{
		OnListenerRemoving(eventType, handler);
		eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;
		OnListenerRemoved(eventType);
	}

	//Two parameters
	static public void Unsubscribe<T, U>(string eventType, Callback<T, U> handler)
	{
		OnListenerRemoving(eventType, handler);
		eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;
		OnListenerRemoved(eventType);
	}

	//Three parameters
	static public void Unsubscribe<T, U, V>(string eventType, Callback<T, U, V> handler)
	{
		OnListenerRemoving(eventType, handler);
		eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] - handler;
		OnListenerRemoved(eventType);
	}

	//Four parameters
	static public void Unsubscribe<T, U, V, W>(string eventType, Callback<T, U, V, W> handler)
	{
		OnListenerRemoving(eventType, handler);
		eventTable[eventType] = (Callback<T, U, V, W>)eventTable[eventType] - handler;
		OnListenerRemoved(eventType);
	}

	//Five parameters
	static public void Unsubscribe<T, U, V, W, X>(string eventType, Callback<T, U, V, W, X> handler)
	{
		OnListenerRemoving(eventType, handler);
		eventTable[eventType] = (Callback<T, U, V, W, X>)eventTable[eventType] - handler;
		OnListenerRemoved(eventType);
	}
	#endregion

	#region Broadcast
	//No parameters
	static public void Broadcast(string eventType)
	{
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("EventKit\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
		OnBroadcasting(eventType);

		Delegate d;

		if (eventTable.TryGetValue(eventType, out d)) {
			Callback callback = d as Callback;

			if (callback != null) {
				callback();
			}
			else
			{
				throw CreateBroadcastSignatureException(eventType);
			}
		}
	}

	//Single parameter
	static public void Broadcast<T>(string eventType, T arg1)
	{
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("EventKit\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
		OnBroadcasting(eventType);

		Delegate d;

		if (eventTable.TryGetValue(eventType, out d)) {
			Callback<T> callback = d as Callback<T>;

			if (callback != null) {
				callback(arg1);
			}
			else
			{
				throw CreateBroadcastSignatureException(eventType);
			}
		}
	}

	//Two parameters
	static public void Broadcast<T, U>(string eventType, T arg1, U arg2)
	{
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("EventKit\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
		OnBroadcasting(eventType);

		Delegate d;

		if (eventTable.TryGetValue(eventType, out d)) {
			Callback<T, U> callback = d as Callback<T, U>;

			if (callback != null) {
				callback(arg1, arg2);
			}
			else
			{
				throw CreateBroadcastSignatureException(eventType);
			}
		}
	}

	//Three parameters
	static public void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
	{
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("EventKit\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
		OnBroadcasting(eventType);

		Delegate d;

		if (eventTable.TryGetValue(eventType, out d)) {
			Callback<T, U, V> callback = d as Callback<T, U, V>;

			if (callback != null) {
				callback(arg1, arg2, arg3);
			}
			else
			{
				throw CreateBroadcastSignatureException(eventType);
			}
		}
	}

	//Four parameters
	static public void Broadcast<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4)
	{
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("EventKit\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
		OnBroadcasting(eventType);

		Delegate d;

		if (eventTable.TryGetValue(eventType, out d)) {
			Callback<T, U, V, W> callback = d as Callback<T, U, V, W>;

			if (callback != null) {
				callback(arg1, arg2, arg3, arg4);
			}
			else
			{
				throw CreateBroadcastSignatureException(eventType);
			}
		}
	}

	//Five parameters
	static public void Broadcast<T, U, V, W, X>(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5)
	{
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("EventKit\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
		OnBroadcasting(eventType);

		Delegate d;

		if (eventTable.TryGetValue(eventType, out d)) {
			Callback<T, U, V, W, X> callback = d as Callback<T, U, V, W, X>;

			if (callback != null) {
				callback(arg1, arg2, arg3, arg4, arg5);
			}
			else
			{
				throw CreateBroadcastSignatureException(eventType);
			}
		}
	}
	#endregion
}

//This manager will ensure that the messenger's eventTable will be cleaned up upon loading of a new level.
public sealed class EventKitHelper : BaseBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	//Clean up eventTable every time a new level loads.
	public void OnLevelWasLoaded(int unused)
	{
		EventKit.Cleanup();
	}
}
