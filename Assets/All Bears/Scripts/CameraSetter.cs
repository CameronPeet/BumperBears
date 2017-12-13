using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetter : MonoBehaviour {

    public static void Set1of4(Camera camera, HoverCarControl control)
    {
        Rect rect = new Rect();
        rect.x = 0.0f;
        rect.width = 0.5f;
        rect.y = 0.0f;
        rect.height = 0.5f;

        camera.rect = rect;
        control.cameraRect = rect;
    }

    public static void Set2of4(Camera camera, HoverCarControl control)
    {
        Rect rect = new Rect();
        rect.x = 0.5f;
        rect.width = 1.0f;
        rect.y = 0.0f;
        rect.height = 0.5f;

        camera.rect = rect;
        control.cameraRect = rect;
    }

    public static void Set3of4(Camera camera, HoverCarControl control)
    {
        Rect rect = new Rect();
        rect.x = 0.0f;
        rect.width = 0.5f;
        rect.y = 0.5f;
        rect.height = 1.0f;

        camera.rect = rect;
        control.cameraRect = rect;
    }

    public static void Set4of4(Camera camera, HoverCarControl control)
    {
        Rect rect = new Rect();
        rect.x = 0.5f;
        rect.width = 1.0f;
        rect.y = 0.5f;
        rect.height = 1.0f;

        camera.rect = rect;
        control.cameraRect = rect;
    }


    public static void Set1of2(Camera camera, HoverCarControl control)
    {
        Rect rect = new Rect();
        rect.x = 0.0f;
        rect.width = 0.5f;
        rect.y = 0.0f;
        rect.height = 1.0f;

        camera.rect = rect;
        control.cameraRect = rect;
    }

    public static void Set2of2(Camera camera, HoverCarControl control)
    {
        Rect rect = new Rect();
        rect.x = 0.5f;
        rect.width = 1.0f;
        rect.y = 0.0f;
        rect.height = 1.0f;

        camera.rect = rect;
        control.cameraRect = rect;
    }
}
