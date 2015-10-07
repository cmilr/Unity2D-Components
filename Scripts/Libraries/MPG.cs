//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S             www.matcha.industries

using UnityEngine;
using System.Collections;
using System;

namespace Matcha.ProcGen
{

public class MPG : CacheBehaviour
{
    // return various room coordinates for procedural generation
    public static int TopLeftX(ProcRoom room)
    {
        return room.originX;
    }

    public static int TopLeftY(ProcRoom room)
    {
        return room.originY;
    }

    public static int TopRightX(ProcRoom room)
    {
        return room.originX + room.width - 1;
    }

    public static int TopRightY(ProcRoom room)
    {
        return room.originY;
    }

    public static int BottomLeftX(ProcRoom room)
    {
        return room.originX;
    }

    public static int BottomLeftY(ProcRoom room)
    {
        return room.originY + room.height - 1;
    }

    public static int BottomRightX(ProcRoom room)
    {
        return room.originX + room.width - 1;
    }

    public static int BottomRightY(ProcRoom room)
    {
        return room.originY + room.height - 1;
    }
}
}