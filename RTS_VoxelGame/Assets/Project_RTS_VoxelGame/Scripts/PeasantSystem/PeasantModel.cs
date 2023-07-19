using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace NekraliusDevelopmentStudio
{
    public class PeasantModel : MonoBehaviour, ISelectableInteractable
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //PeasantModel - (0.2)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Dependencies -
        private NavMeshAgent agent => GetComponent<NavMeshAgent>();
        private Animator anim => GetComponent<Animator>();
        #endregion

        #region - Navigation System -
        public bool waitForDestination;
        private Vector3 newDestination;
        #endregion

        public UnityAction OnInteractEvent;
        public UnityAction OnInteractEndEvent;

        public bool isWalking = false;

        public void GoTo(Vector3 position, bool expressOrder)
        {
            if (expressOrder) agent.SetDestination(position);
            else waitForDestination = true;
            newDestination = position;
        }

        public void Update()
        {
            if (waitForDestination)
            {
                if (agent.pathPending)
                {
                    isWalking = false;
                    return;
                }
                else
                {
                    agent.SetDestination(newDestination);
                    waitForDestination = false;
                    isWalking = true;
                }
            }
            AnimationUpdates();
        }
        private void AnimationUpdates()
        {
            anim.SetBool("IsWalking", isWalking);
        }

        public void Interact()
        {
            throw new System.NotImplementedException();
        }

        public void OnInteract()
        {
            OnInteractEvent.Invoke();
        }

        public void OnInteractionEnd()
        {
            OnInteractEndEvent.Invoke();
        }
    }
}