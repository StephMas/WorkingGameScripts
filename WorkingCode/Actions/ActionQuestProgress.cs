/*
 *
 *	Custom - Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionQuestProgress.cs"
 * 
 *	This action is used to update quest progress as defined in the easy quest manager.
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
    public class ActionQuestProgress : Action
    {
        //The basic quest information
        public int questParameterID = -1;
        public int questID;
        private int questNumber;
        public EasyQuestManager questManager;
        public int managerConstantID = 0;
        public int managerParameterID = -1;

        //The UI information
        public QuestLogUI logUI;
        public int logUIID = 0;
        public int logUIParameterID = -1;


        //The inventory information, if the quest type is to collect
        public int invParameterID = -1;
        public int invID;
        private int invNumber;

        //The character to select, if the quest type is to talk to someone
        public NPC npcToTalkTo;
        public int npcID = 0;
        public int npcParameterID = -1;

        //The prefab of the enemy that you need to defeat
        public GameObject enemytoDefeat;
        public int enemyID = 0;
        public int enemyParamID = -1;


        //Other parameters
        public QuestType questType;
        public string objectName;

#if UNITY_EDITOR
        private InventoryManager inventoryManager;
        private SettingsManager settingsManager;
#endif


        public ActionQuestProgress()
        {
            this.isDisplayed = true;
            category = ActionCategory.Quest;
            title = "Check Progress";
            description = "Checks the progress of a player's quest log and updates it accordingly.";
        }


        override public void AssignValues(List<ActionParameter> parameters)
        {
            questID = AssignInteger(parameters, questParameterID, questID);
            invID = AssignInvItemID(parameters, invParameterID, invID);
            npcToTalkTo = AssignFile<NPC>(parameters, npcParameterID, npcID, npcToTalkTo);
            enemytoDefeat = AssignFile(parameters, enemyParamID, enemyID, enemytoDefeat);
            questManager = AssignFile<EasyQuestManager>(parameters, managerParameterID, managerConstantID, questManager);

            logUI = AssignFile<QuestLogUI>(parameters, logUIParameterID, logUIID, logUI);
        }


        override public float Run()
        {
            //Check the quest type and add the parameters needed
            if (questType == QuestType.Collect)
            {              
                questManager.CheckCollectObj(objectName);
                logUI.UpdateCurrentQuestInfo();

            }
            else if (questType == QuestType.Defeat)
            {
                enemytoDefeat.name = objectName;
                questManager.CheckEnemyObj(objectName);
                logUI.UpdateCurrentQuestInfo();
            }
            else if (questType == QuestType.TalkTo)
            {
                npcToTalkTo.speechLabel = objectName;
                questManager.CheckTalkObj(objectName);
                logUI.UpdateCurrentQuestInfo();
            }          
            
            return 0f;
        }


#if UNITY_EDITOR

        override public void ShowGUI(List<ActionParameter> parameters)
        {
            //Set up the proper UI window
            logUIParameterID = Action.ChooseParameterGUI("Quest Log UI:", parameters, logUIParameterID, ParameterType.GameObject);

            if (logUIParameterID >= 0)
            {
                logUIID = 0;
                logUI = null;
            }
            else
            {
                logUI = (QuestLogUI)EditorGUILayout.ObjectField("Quest Log UI:", logUI, typeof(QuestLogUI), true);
                logUIID = FieldToID(logUI, logUIID);
                logUI = IDToField(logUI, logUIID, false);
            }

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
                ShowQuestGUI(parameters, questManager);
            }
            else
            {
                EditorGUILayout.HelpBox("A Quest Manager must be chosen for more options to show.", MessageType.Warning);
            }

        }

        private void ShowQuestGUI(List<ActionParameter> _parameters, EasyQuestManager _questManager)
        {
            //First check the quest type to see what we're doing
            questType = (QuestType)EditorGUILayout.EnumPopup("Quest Type:", questType);

            if (questType == QuestType.Collect)
            {
                //Get a list of your inventory items and create a popup to choose from
                objectName = EditorGUILayout.TextField("Name of Object:", objectName);


            }
            else if (questType == QuestType.Defeat)
            {
                //Drag the enemy prefab here and get the name
                enemyParamID = Action.ChooseParameterGUI("Enemy:", _parameters, enemyID, ParameterType.GameObject);
                if (enemyParamID >= 0)
                {
                    enemyID = 0;
                    enemytoDefeat = null;
                }
                else
                {
                    enemytoDefeat = (GameObject)EditorGUILayout.ObjectField("Enemy:", enemytoDefeat, typeof(GameObject), true);

                    enemyID = FieldToID(enemytoDefeat, enemyID);
                    enemytoDefeat = IDToField(enemytoDefeat, enemyID, false);
                }

            }
            else if (questType == QuestType.TalkTo)
            {
                //Drag the character prefab here and get the name
                npcParameterID = Action.ChooseParameterGUI("NPC to Talk to:", _parameters, npcParameterID, ParameterType.GameObject);
                if (npcParameterID >= 0)
                {
                    npcID = 0;
                    npcToTalkTo = null;
                }
                else
                {
                    npcToTalkTo = (NPC)EditorGUILayout.ObjectField("NPC to Talk to:", npcToTalkTo, typeof(NPC), true);

                    npcID = FieldToID<NPC>(npcToTalkTo, npcID);
                    npcToTalkTo = IDToField<NPC>(npcToTalkTo, npcID, false);
                }

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