using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHUDElement<T>
{
    void UpdateHUD(T value);
}
