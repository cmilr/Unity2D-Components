using UnityEngine;
using System.Collections;

public interface IGameStateReadOnly {

    bool LevelLoading           { get; }
}

public interface IGameStateFullAccess {

    bool LevelLoading           { get; set; }
}
