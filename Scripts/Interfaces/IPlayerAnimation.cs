using UnityEngine;
using System.Collections;

public interface IPlayerAnimation {

    void PlayIdleAnimation(float xOffset, float yOffset);
    void PlayRunAnimation(float xOffset, float yOffset);
    void PlayJumpAnimation(float xOffset, float yOffset);
    void PlayAttackAnimation(float xOffset, float yOffset);
}
