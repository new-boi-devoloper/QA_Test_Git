using System;
using System.Collections.Generic;
using UnityEngine;

namespace MB.SO.QuestSo
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/DialogData/DialogPiece", order = 0)]
    public class DialogUnit : ScriptableObject
    {
        [field: SerializeField] public bool IsFirst { get; private set; }

        [field: SerializeField] public DialogFlowUnitType DialogFlowUnitType { get; private set; }

        [TextArea]
        [field: SerializeField]
        public string[] DialogText { get; private set; } // Firstly show All texts and then show options

        [field: SerializeField] public List<ResponseToDialogUnit> ResponseToDialog { get; private set; }

        [NonSerialized] public bool IsShowed;

        [Serializable]
        public class ResponseToDialogUnit
        {
            [field: SerializeField] public string ResponseLeadingToDialog { get; private set; }

            [field: SerializeField] public DialogMeaningUnitType DialogMeaningUnitType { get; private set; }

            [field: SerializeField] public DialogUnit NextDialog { get; private set; }
        }
    }
}