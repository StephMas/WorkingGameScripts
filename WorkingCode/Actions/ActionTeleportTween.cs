/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionTeleportTweening.cs"
 * 
 *	This action moves an object to a specified GameObject's position with a tween.
 *	Markers are helpful in this regard.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

    [System.Serializable]
    public class ActionTeleportTween : Action
    {

        public int obToMoveParameterID = -1;
        public int obToMoveID = 0;
        public int markerParameterID = -1;
        public int markerID = 0;

        public GameObject obToMove;
        public Marker teleporter;

        public float moveTime;



        public ActionTeleportTween()
        {
            this.isDisplayed = true;
            category = ActionCategory.Object;
            title = "Teleport Tween";
            description = "Moves a GameObject to a Marker with a tween. Better than transform but can not copy the Marker's rotation.";
        }


        override public void AssignValues(List<ActionParameter> parameters)
        {
            obToMove = AssignFile(parameters, obToMoveParameterID, obToMoveID, obToMove);
            teleporter = AssignFile<Marker>(parameters, markerParameterID, markerID, teleporter);

        }


        override public float Run()
        {
            if (teleporter && obToMove)
            {
                moveTime += 10 * Time.deltaTime;
                //The marker position
                Vector3 position = teleporter.transform.position;
                Vector3 rotation = teleporter.transform.rotation.eulerAngles;
                               

                obToMove.transform.DOLocalMoveX(position.x, moveTime, false);
                obToMove.transform.DOLocalMoveZ(position.z, moveTime, false);

            }

            return 0f;
        }


#if UNITY_EDITOR

        override public void ShowGUI(List<ActionParameter> parameters)
        {
            obToMove = (GameObject)EditorGUILayout.ObjectField("Object to move:", obToMove, typeof(GameObject), true);

            obToMoveID = FieldToID(obToMove, obToMoveID);
            obToMove = IDToField(obToMove, obToMoveID, false);

            markerParameterID = Action.ChooseParameterGUI("Teleport to:", parameters, markerParameterID, ParameterType.GameObject);
            if (markerParameterID >= 0)
            {
                markerID = 0;
                teleporter = null;
            }
            else
            {
                teleporter = (Marker)EditorGUILayout.ObjectField("Teleport to:", teleporter, typeof(Marker), true);

                markerID = FieldToID<Marker>(teleporter, markerID);
                teleporter = IDToField<Marker>(teleporter, markerID, false);
            }

            moveTime = EditorGUILayout.Slider("Move Time:", moveTime, 0, 10f);


            AfterRunningOption();
        }


        override public void AssignConstantIDs(bool saveScriptsToo)
        {
            if (saveScriptsToo && obToMove != null)
            {
                if (obToMove.GetComponent<NPC>())
                {
                    AddSaveScript<RememberNPC>(obToMove);
                }
                else if (obToMove.GetComponent<Player>() == null)
                {
                    AddSaveScript<RememberTransform>(obToMove);
                }
            }
            AssignConstantID<Marker>(teleporter, markerID, markerParameterID);
        }


        override public string SetLabel()
        {
            string labelAdd = "";

            if (teleporter)
            {
                if (obToMove)
                {
                    labelAdd = " (" + obToMove.name + " to " + teleporter.name + ")";
                }

            }

            return labelAdd;
        }

#endif
    }

}