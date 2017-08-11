using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewMenu : Menu
{
    public override void OnActivation()
    {
        manager.camPan.MoveTo(transform);
    }

    protected override void moveDown()
    {
        if (bottomMenu != null)
        {
            bottomMenu.activate();
            deactivate();
        }
    }

    public override void OnDeactivation()
    {
    }
}