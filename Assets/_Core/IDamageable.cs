﻿using UnityEngine;
namespace RPG.Core
{
    

public interface IDamageable {
    void SubstractHealth(float damage);
        GameObject GetGameObject();
}
}
