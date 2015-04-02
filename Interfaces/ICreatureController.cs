using UnityEngine;
using System.Collections;

public interface ICreatureController {

    void MoveRight(bool status);
    void MoveLeft(bool status);
    void Jump();
    void Attack(bool status);
    void Defend();
}
