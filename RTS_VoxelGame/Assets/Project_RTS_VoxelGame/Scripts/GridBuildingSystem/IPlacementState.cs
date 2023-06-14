﻿using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public interface IPlacementState
    {
        void EndState();
        void OnAction(Vector3Int gridPosition);
        void UpdateState(Vector3Int gridPosition);
    }
}