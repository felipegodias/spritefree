using UnityEngine;

namespace SpriteFree.Core.Editor {

    public class TGCStyles {

        private static GUIStyle labelEven;
        private static GUIStyle labelOdd;

        public static GUIStyle LabelEven => labelEven ?? (labelEven = GUI.skin.FindStyle("CN EntryBackEven"));

        public static GUIStyle LabelOdd => labelOdd ?? (labelOdd = GUI.skin.FindStyle("CN EntryBackOdd"));

    }

}