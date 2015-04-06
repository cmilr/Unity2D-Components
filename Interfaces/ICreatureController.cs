using UnityEngine;
using System.Collections;

public interface ICreatureController {

    void MoveRight();
    void MoveLeft();
    void Jump();
    void Defend();
    void Attack();
}
