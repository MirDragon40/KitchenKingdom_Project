using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    CutScene,
    Go,
    Pause,
    Over,
    Ending
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State = GameState.Go;


}
