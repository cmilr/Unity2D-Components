using UnityEngine;
using System.Collections;

public interface IPlayerAnimation {

    void PlayIdleAnimation();
    void PlayRunAnimation();
    void PlayJumpAnimation();
    void PlaySwingAnimation();
    void OffsetAnimationBy(float offset);
}
