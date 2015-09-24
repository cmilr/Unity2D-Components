//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S             www.matcha.industries

using UnityEngine;
using System.Collections;
using System;

namespace Matcha.Lib
{

public class MLib : CacheBehaviour
{
	// ignore layer collisions *by name* instead of by layer ID
	public static void IgnoreLayerCollision2D(string layer1, string layer2, bool status)
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(layer1), LayerMask.NameToLayer(layer2), status);
	}

	// same as above but ignores layer collisions between *this gameObject* and other layer
	public static void IgnoreLayerCollision2DWith(GameObject gameObject, string layer2, bool status)
	{
		Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(layer2), status);
	}

	// allows comparison of two floats within a given margin of error
	public static bool FloatEqual(float num1, float num2, float threshold = .0001f)
	{
	    return Math.Abs(num1 - num2) < threshold;
	}

	// allows comparison of two doubles within a given margin of error
	public static bool DoubleEqual(double num1, double num2, double threshold = .0001f)
	{
	    return Math.Abs(num1 - num2) < threshold;
	}

    // allows comparison of two floats within a given range
    public static bool FloatWithinRange(float num1, float num2, float range = .02f)
    {
        return Math.Abs(num1 - num2) < range;
    }

    // allows comparison of two doubles within a given range
    public static bool DoubleWithinRange(double num1, double num2, double range = .02f)
    {
        return Math.Abs(num1 - num2) < range;
    }

	// HexToColor was written by Danny Lawrence, and appears here unmodified.
	// It is reproduced under a Creative Common license — http://creativecommons.org/licenses/by-sa/3.0/
	public static Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r, g, b, 255);
	}

	// returns an int constant describing which side the GameObject has been hit on
	public static int HorizSideThatWasHit(GameObject currentGo, Collider2D coll)
	{
		Vector3 relativePosition = currentGo.transform.InverseTransformPoint(coll.transform.position);

		// if scale is positive: ie, facing right
		if (currentGo.transform.lossyScale.x == 1)
		{
			if (relativePosition.x > 0)
				return RIGHT;
			else
				return LEFT;
		}
		// if scale is negative: ie, facing left
		else if (currentGo.transform.lossyScale.x == -1)
		{
			if (relativePosition.x < 0)
				return RIGHT;
			else
				return LEFT;
		}
		else
		{
			return ERROR;
		}
	}

	// same as above, but returns the vertical side that was hit
	public static int VertSideThatWasHit(GameObject currentGo, Collider2D coll)
	{
		Vector3 relativePosition = currentGo.transform.InverseTransformPoint(coll.transform.position);

		// if scale is positive: ie, upright
		if (currentGo.transform.lossyScale.y == 1)
		{
			if (relativePosition.y > 0)
				return TOP;
			else
				return BOTTOM;
		}
		// if scale is negative: ie, upside down
		else if (currentGo.transform.lossyScale.y == -1)
		{
			if (relativePosition.y < 0)
				return TOP;
			else
				return BOTTOM;
		}
		else
		{
			return ERROR;
		}
	}

	// returns Vector2 coordinates for lobbing a projectile
	public static Vector2 LobProjectile(Weapon weapon, Transform origin, Transform target)
	{
	    float distance;
	    float yDifference;
	    float angleToPoint;
	    float distanceFactor;
	    float distanceCompensation;
	    float speedCompensation;
	    float angleCorrection;
	    float speed;

        // the below formula is accurate given the following settings: gravity of .5,
        // angular drag of .05, mass of 1, and projectile speeds between 8 and 20

        distance = target.position.x - origin.position.x;
        yDifference = target.position.y - origin.position.y;
        angleToPoint = (float)Math.Atan2(target.position.y - origin.position.y, target.position.x - origin.position.x);
        speed = weapon.speed;

        // compensate for various projectile speeds
        // in terms of accuracy, the data set below supports speeds between 8 and 20
        if (speed < 8)
            speedCompensation = 3.2f;
        else if (speed >= 8 && speed < 9)
            speedCompensation = 2.2f;
        else if (speed >= 9 && speed < 10)
            speedCompensation = 1.85f;
        else if (speed >= 10 && speed < 11)
            speedCompensation = 1.50f;
        else if (speed >= 11 && speed < 12)
            speedCompensation = 1.25f;
        else if (speed >= 12 && speed < 13)
            speedCompensation = 1f;
        else if (speed >= 13 && speed < 14)
            speedCompensation = .85f;
        else if (speed >= 14 && speed < 15)
            speedCompensation = .75f;
        else if (speed >= 15 && speed < 16)
            speedCompensation = .65f;
        else if (speed >= 16 && speed < 17)
            speedCompensation = .55f;
        else if (speed >= 17 && speed < 18)
            speedCompensation = .5f;
        else if (speed >= 18 && speed < 19)
            speedCompensation = .45f;
        else if (speed >= 19 && speed < 20)
            speedCompensation = .4f;
        else
            speedCompensation = .35f;

        // compensate for both positive and negative distances between origin and target
        distanceFactor = .034f * speedCompensation;

        if (yDifference >= -2f)
            distanceCompensation = .001f * speedCompensation;
        else
            distanceCompensation = .00065f * speedCompensation;

        distanceFactor += yDifference * distanceCompensation;
        angleCorrection = (float)(3.14*0.18) * (distance * distanceFactor);

        return new Vector2((float)Math.Cos(angleToPoint+angleCorrection) * speed,
                           (float)Math.Sin(angleToPoint+angleCorrection) * speed);
    }

    public static float NextGaussian() {
        float v1, v2, s;
        do {
            v1 = 2.0f * UnityEngine.Random.Range(0f,1f) - 1.0f;
            v2 = 2.0f * UnityEngine.Random.Range(0f,1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }

    public static float NextGaussian(float mean, float standard_deviation)
    {
        return mean + NextGaussian() * standard_deviation;
    }

    public static float NextGaussian (float mean, float standard_deviation, float min, float max) {
        float x;
        do {
            x = NextGaussian(mean, standard_deviation);
        } while (x < min || x > max);
        return x;
    }
}
}