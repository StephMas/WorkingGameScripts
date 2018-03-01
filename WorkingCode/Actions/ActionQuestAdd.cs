/*
 *
 *	Custom - Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionQuestAdd.cs"
 * 
 *	This action is used to add a quest to the players quest log as defined in the easy quest manager.
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
    public class ActionQuestAdd : Action
    {

        public int parameterID = -1;
        public int questID;
        private int questNumber;

        public EasyQuestManager questManager;
        public int managerConstantID = 0;
        public int managerParameterID = -1;

        public bool addToFront = false;


        public ActionQuestAdd()
        {
            this.isDisplayed = true;
            category = ActionCategory.Quest;
            title = "Add";
            description = "Adds or removes a quest from the player's quest log.";
        }


        override public void AssignValues(List<ActionParameter> parameters)
        {
            questID = AssignInteger(parameters, parameterID, questID);
            questManager = AssignFile<EasyQuestManager>(parameters, managerParameterID, managerConstantID, questManager);
        }


        override public float Run()
        {
            //Add the quest to the quest log
            questManager.AcceptQuest(questID);
            

            return 0f;
        }


#if UNITY_EDITOR

        override public void ShowGUI(List<ActionParameter> parameters)
        {
            //First select the Quest Manager
            managerParameterID = Action.ChooseParameterGUI("Quest Manager:", parameters, managerParameterID, ParameterType.GameObject);

            if (managerParameterID >= 0)
            {
                managerConstantID = 0;
                questManager = null;
            }
            else
            {
                questManager = (EasyQuestManager)EditorGUILayout.ObjectField("Quest Manager:", questManager, typeof(EasyQuestManager), true);
                managerConstantID = FieldToID(questManager, managerConstantID);
                questManager = IDToField(questManager, managerConstantID, false);
            }

            //Then show the quest options
            if (questManager != null)
            {
                ShowQuestGUI(questManager);
            }
            else
            {
                EditorGUILayout.HelpBox("A Quest Manager must be chosen for more options to show.", MessageType.Warning);
            }

        }

        private void ShowQuestGUI(EasyQuestManager _questManager)
        {
            //Create a string list of the quests names (for the popup box)
            List<string> questList = new List<string>();

            int i = 0;

            if (_questManager.QuestContainerDatabase.EQList.Count > 0)
            {
                foreach (EasyQuestData _quest in _questManager.QuestContainerDatabase.EQList)
                {
                    questList.Add(_quest.QuestName);

                    //If the quest has been removed for some reason, make sure the variable is still valid
                    if (_quest.QuestID == questID)
                    {
                        questNumber = i;
                    }

                    i++;
                }

                if (questNumber == -1)
                {
                    Debug.Log("Previously chosen quest no longer exists!");
                    questNumber = 0;
                    questID = 0;
                }

                string label = "Quest to add:";

                questNumber = EditorGUILayout.Popup(label, questNumber, questList.ToArray());
                questID = _questManager.QuestContainerDatabase.EQList[questNumber].QuestID;

                addToFront = EditorGUILayout.Toggle("Add to front?", addToFront);
            }

            else
            {
                EditorGUILayout.HelpBox("No quests exist!", MessageType.Info);
                questID = -1;
                questNumber = -1;
            }

            AfterRunningOption();
        }


        override public string SetLabel()
        {
            string labelAdd = "";

            return labelAdd;
        }


#endif

    }

}