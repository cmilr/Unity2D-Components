#define DEBUG_LEVEL_LOG
#define DEBUG_LEVEL_WARN
#define DEBUG_LEVEL_ERROR


using UnityEngine;
using System.Collections;


// setting the conditional to the platform of choice will only compile the method for that platform
// alternatively, use the #defines at the top of this file
public class D
{
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_WARN" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_ERROR" )]
	public static void log( object format, params object[] paramList )
	{
		if( format is string )
			Debug.Log( string.Format( format as string, paramList ) );
		else
			Debug.Log( format );
	}
	
	
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_WARN" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_ERROR" )]
	public static void warn( object format, params object[] paramList )
	{
		if( format is string )
			Debug.LogWarning( string.Format( format as string, paramList ) );
		else
			Debug.LogWarning( format );
	}
	
	
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_ERROR" )]
	public static void error( object format, params object[] paramList )
	{
		if( format is string )
			Debug.LogError( string.Format( format as string, paramList ) );
		else
			Debug.LogError( format );
	}
	
	
	[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
	public static void Assert( bool condition )
	{
		Assert( condition, string.Empty, true );
	}

	
	[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
	public static void Assert( bool condition, string assertString )
	{
		Assert( condition, assertString, false );
	}

	
	[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
	[System.Diagnostics.Conditional( "DEBUG_LEVEL_LOG" )]
	public static void Assert( bool condition, string assertString, bool pauseOnFail )
	{
		if( !condition )
		{
			Debug.LogError( "ASSERT FAILED:   " + assertString );
			
			if( pauseOnFail )
				Debug.Break();
		}
	}

}
