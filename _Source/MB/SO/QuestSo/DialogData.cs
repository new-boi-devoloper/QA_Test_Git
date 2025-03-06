using System.Collections.Generic;
using UnityEngine;

namespace MB.SO.QuestSo
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/DialogData/DialogBinding", order = 0)]
    public class DialogData : ScriptableObject
    {
        public List<DialogUnit> DialogUnits;
    }

    public enum DialogFlowUnitType
    {
        SimpleDialog,
        Shop,
        Final
    }

    public enum DialogMeaningUnitType
    {
        Accept,
        Decline,
        None
    }
}