using System.Collections;
using UnityEngine;

public interface IGameStateReadOnly
{
	bool LevelLoading           { get; }
}

public interface IGameStateFullAccess
{
	bool LevelLoading           { get; set; }
}
