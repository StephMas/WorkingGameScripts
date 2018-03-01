/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionTeleport.cs"
 * 
 *	This action moves an object to a specified GameObject's position.
 *	Markers are helpful in this regard.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

    [System.Serializable]
    public class ActionExactStop : Action
    {
        public int obToMoveParameterID = -1;
        public int obToMoveID = 0;

        public int choice;
        public string[] names = new string[] { "Exact On", "Exact Off" };
        public int[] choiceNumbers = { 0, 1 };

        public bool isPlayer;
        public GameObject obToMove;


        public ActionExactStop()
        {
            this.isDisplayed = true;
            category = ActionCategory.Custom;
            title = "Change Motion Control";
            description = "Changes a characters motion control engine. To be used prior to moving a character to a point and again after moving the character to that point.";
        }

        override public void AssignValues(List<ActionParameter> parameters)
        {
            obToMove = AssignFile(parameters, obToMoveParameterID, obToMoveID, obToMove);

            if (isPlayer && KickStarter.player)
            {
                obToMove = KickStarter.player.gameObject;
            }
        }

        override public float Run()
        {
            
            if (choice == 0)
            {

                obToMove.GetComponent<Char>().motionControl = MotionControl.Automatic;

            }

            else if (choice == 1)
            {

                obToMove.GetComponent<Char>().motionControl = MotionControl.JustTurning;
            }

            return 0f;
        }

#if UNITY_EDITOR

        override public void ShowGUI(List<ActionParameter> parameters)
        {

            isPlayer = EditorGUILayout.Toggle("Is Player?", isPlayer);
            if (!isPlayer)
            {
                obToMoveParameterID = Action.ChooseParameterGUI("Object to move:", parameters, obToMoveParameterID, ParameterType.GameObject);
                if (obToMoveParameterID >= 0)
                {
                    obToMoveID = 0;
                    obToMove = null;
                }
                else
                {
                    obToMove = (GameObject)EditorGUILayout.ObjectField("Object to move:", obToMove, typeof(GameObject), true);

                    obToMoveID = FieldToID(obToMove, obToMoveID);
                    obToMove = IDToField(obToMove, obToMoveID, false);
                }
            }        

            choice = EditorGUILayout.IntPopup("Action: ", choice, names, choiceNumbers);

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
                else if (obToMove.GetComponent<Player>() == null && !isPlayer)
                {
                    AddSaveScript<RememberTransform>(obToMove);
                }
            }

            if (!isPlayer)
            {
                AssignConstantID(obToMove, obToMoveID, obToMoveParameterID);
            }

        }


        override public string SetLabel()
        {
            string labelAdd = "";

            return labelAdd;
        }

#endif
    }
}
