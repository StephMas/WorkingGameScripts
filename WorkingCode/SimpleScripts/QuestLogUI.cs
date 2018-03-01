using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AC;


    public class QuestLogUI : MonoBehaviour
    {

    //The quest manager to grab from
    public EasyQuestManager questManager;

    //The fields to populate on the quest log
    public GameObject questLog;
    public TMP_Text qName;
    public TMP_Text qDescription;
    public TMP_Text qReward;
    public Transform logViewportContent;
    public GameObject questButtonPrefab;
    
    //The fields to populate on the tracker
    public TMP_Text qNameTracker;
    public TMP_Text qShortDescriptionTracker;


    //Other parameters
    public int selectedQuest = 0;
    public int trackedQuest = 0;
    public bool questLogOpen = false;
 

    //Adventure Creator parameters
    //The global variable that will always keep the quest ID linked for the currently tracked quest
    private GVar aCQuestVar;

    public void Awake()
    {
        //On awake, get the right quest manager and update the fields as appropriate
        UpdateCurrentQuestInfo(); //Update the info shown on screen of the current quest
        questManager.OnCTQChanged += UpdateCurrentQuestInfo; //Update the on screen quest info when the current quest changes
        questManager.OnQuestProgress += UpdateCurrentQuestInfo; //Update the on screen qquest info when a quest gains progress
        questManager.OnQuestFinish += UpdateCurrentQuestInfo;
        questManager.OnQuestsDataLoaded += UpdateCurrentQuestInfo;
    }

    public void Start()
    {
        //Check to make sure the right quest is being tracked. The script will set the initial value.
        aCQuestVar = AC.GlobalVariables.GetVariable(0);
        trackedQuest = aCQuestVar.val;

        //Get the right components for the quest log
        questLog.GetComponent<EasyQuestManager>();
    }


    //Open and close the quest log (function). Will also initialize the appropriate prefabs.
    public void OpenClose()
    {
        if (questLogOpen)
        {
            CloseLog();
        }
        else
        {
            OpenLog();
        }
    }

    //On click of the prefab this changes the selected quest in the quest log and shows it's info. 
    public void QuestPrefabClick(int _selectedQuestID)
    {
        selectedQuest = _selectedQuestID;
        qName.text = questManager.QLList[questManager.Questlog[_selectedQuestID]].QuestName;
        qDescription.text = questManager.QLList[questManager.Questlog[_selectedQuestID]].Description;

        //Set the track button to show
        

        //update the AC variable
        AC.GlobalVariables.SetIntegerValue(0, _selectedQuestID, true);
    }


    //Set a tracked quest (function) to the linked AC variable.
    public void SetQuestAsTracked()
    {
        selectedQuest = AC.GlobalVariables.GetIntegerValue(0);

        if (questManager != null)
        {
            questManager.SetCurrentQuest(selectedQuest);          
        }
    }
   


    //Open the Quest Log
    public void OpenLog()
    {
        questLogOpen = true;

        questLog.SetActive(true);
        selectedQuest = 0;

        //Iterate through the whole quest log

        for (int i = 0; i < questManager.Questlog.Count; i++)
        {
            //Create a UI object
            QuestPrefabUI _UI = GameObject.Instantiate(questButtonPrefab).GetComponent<QuestPrefabUI>();
            _UI.transform.SetParent(logViewportContent, false);
            int temp = i;
            _UI.questNameText.text = questManager.QLList[questManager.Questlog[i]].QuestName; //Make the text the name of the quest
            _UI.selectQuestButton.onClick.AddListener(() => { QuestPrefabClick(temp); }); //Make the UI button change the questlog's info to it's info
        }

    }

    public void CloseLog()
    {
        //Destroy all of the children
        foreach (Transform child in logViewportContent)
        {
            Destroy(child.gameObject);
        }

        questLogOpen = false;
        questLog.SetActive(false);
    }

    //Update the info of the on screen quest.
    public void UpdateCurrentQuestInfo()
    {
        if (questManager.Questlog.Count > 0)
        {
            //Make sure CurrentQuest isn't outside the range of our questlog
            if (questManager.CurrentQuest > (questManager.Questlog.Count - 1))
            {
                questManager.CurrentQuest = (questManager.Questlog.Count - 1);
            }

            qNameTracker.text = questManager.QLList[questManager.Questlog[questManager.CurrentQuest]].QuestName;

            EasyQuestData easyQuestData = questManager.QLList[questManager.Questlog[questManager.CurrentQuest]];

            if (!easyQuestData.FinishedQuest)

            { //Don't try and update the text if the quest is finished.
                if (easyQuestData.ObjectivesInOrder && easyQuestData.CObjective <= (easyQuestData.Objs.Count - 1))
                {
                    qShortDescriptionTracker.text = easyQuestData.Objs[easyQuestData.CObjective].sDescript + " \n"; //Show the short description

                        //Show the objective info
                        if (easyQuestData.Objs[easyQuestData.CObjective].qType == QuestType.Collect || easyQuestData.Objs[easyQuestData.CObjective].qType == QuestType.Defeat)
                        {
                            qShortDescriptionTracker.text += "Collected " + easyQuestData.Objs[easyQuestData.CObjective].Name + ": " + easyQuestData.Objs[easyQuestData.CObjective].Amount + "/" + easyQuestData.Objs[easyQuestData.CObjective].AmountNeeded;
                            qReward.text += "Collected " + easyQuestData.Objs[easyQuestData.CObjective].Name + ": " + easyQuestData.Objs[easyQuestData.CObjective].Amount + "/" + easyQuestData.Objs[easyQuestData.CObjective].AmountNeeded;
                    }
                        if (easyQuestData.Objs[easyQuestData.CObjective].qType == QuestType.GoTo || easyQuestData.Objs[easyQuestData.CObjective].qType == QuestType.MoveObj)
                        {
                            qShortDescriptionTracker.text += "Location: " + new Vector3(easyQuestData.Objs[easyQuestData.CObjective].DestinationX,
                                easyQuestData.Objs[easyQuestData.CObjective].DestinationY, easyQuestData.Objs[easyQuestData.CObjective].DestinationZ).ToString();
                            qReward.text += "Location: " + new Vector3(easyQuestData.Objs[easyQuestData.CObjective].DestinationX,
                                easyQuestData.Objs[easyQuestData.CObjective].DestinationY, easyQuestData.Objs[easyQuestData.CObjective].DestinationZ).ToString();
                    }
                    }
                    else if (!easyQuestData.ObjectivesInOrder && easyQuestData.CObjective <= (easyQuestData.Objs.Count - 1))
                    {
                        qShortDescriptionTracker.text = easyQuestData.Objs[easyQuestData.CObjective].sDescript + " \n"; //Show the short description

                        for (int i = 0; i < easyQuestData.Objs.Count; i++)
                        {
                            if (!easyQuestData.Objs[i].Complete)
                            {
                                //Show the objective info
                                if (easyQuestData.Objs[i].qType == QuestType.Collect || easyQuestData.Objs[i].qType == QuestType.Defeat)
                                {
                                    qShortDescriptionTracker.text += "Collected " + easyQuestData.Objs[i].Name + ": " + easyQuestData.Objs[i].Amount + "/" + easyQuestData.Objs[i].AmountNeeded + "\n";
                                    qReward.text += "Collected " + easyQuestData.Objs[i].Name + ": " + easyQuestData.Objs[i].Amount + "/" + easyQuestData.Objs[i].AmountNeeded + "\n";
                                }
                                if (easyQuestData.Objs[i].qType == QuestType.GoTo || easyQuestData.Objs[i].qType == QuestType.MoveObj)
                                {
                                    qShortDescriptionTracker.text += "Location: " + new Vector3(easyQuestData.Objs[i].DestinationX,
                                        easyQuestData.Objs[i].DestinationY, easyQuestData.Objs[i].DestinationZ).ToString() + "\n";
                                    qReward.text += "Location: " + new Vector3(easyQuestData.Objs[i].DestinationX,
                                        easyQuestData.Objs[i].DestinationY, easyQuestData.Objs[i].DestinationZ).ToString() + "\n";
                            }
                            }
                        }
                    }
                }
                else
                {
                    qNameTracker.text = "";
                    qShortDescriptionTracker.text = "";
                }


            }
            else
            {
                //Else, we have no quest.
                qNameTracker.text = "";
                qShortDescriptionTracker.text = "";
            }
        }

    //Used to listen to an event. However, the QID is unneccesary here.
    public void UpdateCurrentQuestInfo(int _questID)
    {
        UpdateCurrentQuestInfo();
    }
}

