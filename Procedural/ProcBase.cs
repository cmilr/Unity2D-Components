using UnityEngine;
using System.Collections;

public class ProcBase {

    public int originX  { get; set; }
    public int originY  { get; set; }
    public int width = 1;
    public int height = 1;

    public int TopLeftX()
    {
        return originX;
    }

    public int TopLeftY()
    {
        return originY;
    }

    public int TopRightX()
    {
        return originX + width - 1;
    }

    public int TopRightY()
    {
        return originY;
    }

    public int BottomLeftX()
    {
        return originX;
    }

    public int BottomLeftY()
    {
        return originY + height - 1;
    }

    public int BottomRightX()
    {
        return originX + width - 1;
    }

    public int BottomRightY()
    {
        return originY + height - 1;
    }
}
