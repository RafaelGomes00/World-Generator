using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GizmosDrawer : MonoBehaviour
{
    public abstract void Draw(params object[] info);

    protected abstract void OnDrawGizmos();
}
