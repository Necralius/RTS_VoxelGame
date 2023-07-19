using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static NekraliusDevelopmentStudio.BuildingSystemUtility;

namespace NekraliusDevelopmentStudio
{
    public class SelectablePeasantResource : SelectableResourceModel
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Dependencies -
        private NavMeshAgent agent;
        private Animator anim => GetComponent<Animator>();
        #endregion

        #region - Navigation System -
        public bool waitForDestination;
        private Vector3 newDestination;
        #endregion

        private Vector3 lastPos;

        private LineRenderer pathRenderer;

        public bool isWalking = false;

        MapData mapData => GridWorldGenerator.Instance.currentMap;

        public SelectableItemResource itemToGet;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            pathRenderer = GetComponent<LineRenderer>();

            pathRenderer.startWidth = 0.15f;
            pathRenderer.endWidth = 0.15f;
            pathRenderer.positionCount = 0;

            newDestination = transform.position;
        }

        private void GoTo(Vector3 position)
        {
            bool expressOrder = !InputManager.Instance.leftShift;
            if (InputManager.Instance.leftShift) waitForDestination = true;
            else agent.SetDestination(position);
            newDestination = position;
        }
        public void StartGoToAction()
        {
            ModeManager.Instance.SetMode(ModeType.PeasantGoToMode);
        }

        private void DrawPath()
        {
            pathRenderer.positionCount = agent.path.corners.Length;
            pathRenderer.SetPosition(0, transform.position);

            if (agent.path.corners.Length < 2) return;

            for (int i = 1; i < agent.path.corners.Length; i++)
            {
                Vector3 pointPosition = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y, agent.path.corners[i].z);
                pathRenderer.SetPosition(i, pointPosition);
            }
        }

        private bool CanGoTo(BlockCell cell, Vector3Int GridPos)
        {
            ObjectGridData floorData = BuildingSystem.Instance.floorData;
            ObjectGridData structureData = BuildingSystem.Instance.structureData;
            return floorData.CanPlaceObjectAt(GridPos, new Vector2Int(1, 1)) && structureData.CanPlaceObjectAt(GridPos, new Vector2Int(1, 1)) && !cell.cellOcupied;
        }

        public void MineResource()
        {



        }

        public void Update()
        {
            if (ModeManager.Instance.IsOnState(ModeType.PeasantGoToMode))
            {
                Vector3 mousePos = InputManager.Instance.GetSelectedMapPosition();
                Vector3Int gridPosition = BuildingSystem.Instance.buildingGrid.WorldToCell(mousePos);

                if (lastPos != gridPosition)
                {
                    agent.SetDestination(gridPosition);
                    agent.isStopped = true;

                    if (CanGoTo(mapData.mapCompleteData[gridPosition.x, gridPosition.y], gridPosition)) pathRenderer.material.color = Color.white;
                    else pathRenderer.material.color = Color.red;
                }
                else lastPos = gridPosition;

                if (Input.GetMouseButtonDown(1))
                {
                    GoTo(gridPosition);
                    agent.isStopped = false;
                    ModeManager.Instance.ResetMode();
                }     
            }

            isWalking = !(agent.velocity == Vector3.zero);

            if (agent.hasPath) DrawPath();

            pathRenderer.enabled = isSelected;

            if (waitForDestination)
            {
                if (agent.destination == newDestination) return;
                if (agent.pathPending) return;
                else
                {
                    agent.SetDestination(newDestination);
                    waitForDestination = false;
                }
            }

            anim.SetBool("IsWalking", isWalking);
        }
    }
}