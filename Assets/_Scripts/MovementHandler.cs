using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler
{
    public Vector3 turnPos;
    public bool turnRight;
    public bool turnUp;
    public bool turnDown;
    public bool turnLeft;

    public MovementHandler(Vector3 turnPos, bool right, bool up, bool down, bool left)
    {
        this.turnPos = turnPos;
        turnRight = right;
        turnUp = up;
        turnDown = down;
        turnLeft = left;
    }

}
