using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakePhysicalDamage(int damage);
}

public class Equiup : MonoBehaviour
{
    public virtual void OnAttackInput()
    {

    }
}
