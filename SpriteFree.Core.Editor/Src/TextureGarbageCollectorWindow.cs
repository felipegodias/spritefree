using UnityEditor;
using UnityEngine;

namespace SpriteFree.Core.Editor {

    public class TextureGarbageCollectorWindow : EditorWindow {

        private TextureGarbageCollector textureGarbageCollector;

        private Texture[] textures;

        private Object[][] objects;

        private Texture selectedTexture;

        private Object selectedObject;

        private Vector2 scroll;

        [MenuItem("Window/Texture Garbage Collector")]
        public static void OpenWindow() {
            GetWindow<TextureGarbageCollectorWindow>(false, "Texture GC");
        }

        private void Update() {
            this.Repaint();
        }

        private void OnGUI() {
            this.textureGarbageCollector = TextureGarbageCollector.Instance;

            if (this.textureGarbageCollector == null) {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("Application not running", EditorStyles.centeredGreyMiniLabel);
                GUILayout.FlexibleSpace();
                return;
            }

            if (this.textureGarbageCollector.IsDirty) {
                this.textureGarbageCollector.GetReferences(out this.textures, out this.objects);
            }

            if (this.textures == null || this.textures.Length == 0) {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("Nothing to show", EditorStyles.centeredGreyMiniLabel);
                GUILayout.FlexibleSpace();
                return;
            }

            float screenView = 0.75f;
            int texturesScreenSize = (int) (Screen.width * screenView);
            int referencesScreenSize = (int) (Screen.width * (1 - screenView));

            this.DrawTitles(texturesScreenSize, referencesScreenSize);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

            EditorGUILayout.BeginVertical(GUILayout.Width(texturesScreenSize));

            this.DrawTextureList(texturesScreenSize);

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUILayout.Width(referencesScreenSize));

            this.DrawReferenceList();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawTitles(int texturesScreenSize, int referencesScreenSize) {
            EditorGUILayout.BeginHorizontal();

            GUIContent textureName = new GUIContent(" Texture Name");
            Rect textureNameRect = GUILayoutUtility.GetRect(textureName, EditorStyles.label,
                GUILayout.Width(texturesScreenSize * 0.6f));
            EditorGUI.LabelField(textureNameRect, textureName);

            GUIContent memory = new GUIContent(" Memory");
            Rect memoryRect =
                GUILayoutUtility.GetRect(memory, EditorStyles.label, GUILayout.Width(texturesScreenSize * 0.2f));
            EditorGUI.LabelField(memoryRect, memory);
            memoryRect.y -= 1;
            memoryRect.width = 1;
            memoryRect.height += 3;
            EditorGUI.DrawRect(memoryRect, Color.black);

            GUIContent refCount = new GUIContent(" Ref Count");
            Rect refCountRect = GUILayoutUtility.GetRect(refCount, EditorStyles.label,
                GUILayout.Width(texturesScreenSize * 0.2f));
            EditorGUI.LabelField(refCountRect, refCount);

            refCountRect.y -= 1;
            refCountRect.width = 1;
            refCountRect.height += 3;
            EditorGUI.DrawRect(refCountRect, Color.black);

            GUIContent referencedBy = new GUIContent(" Referenced By:");
            Rect referencedByRect =
                GUILayoutUtility.GetRect(referencedBy, EditorStyles.label, GUILayout.Width(referencesScreenSize));
            EditorGUI.LabelField(referencedByRect, referencedBy);

            referencedByRect.y -= 1;
            referencedByRect.width = 1;
            referencedByRect.height = Screen.height;
            EditorGUI.DrawRect(referencedByRect, Color.black);

            EditorGUILayout.EndHorizontal();

            Rect lineRect = GUILayoutUtility.GetRect(Screen.width, Screen.width, 1, 1);
            EditorGUI.DrawRect(lineRect, Color.black);
        }

        private void DrawTextureList(int screenSize) {
            this.scroll = GUILayout.BeginScrollView(this.scroll, GUILayout.Width(screenSize + 16));

            for (int i = 0; i < this.textures.Length; i++) {
                Texture texture = this.textures[i];
                GUIContent content = new GUIContent();
                Rect rect = GUILayoutUtility.GetRect(content, EditorStyles.label, GUILayout.Width(screenSize));
                rect.x -= 3;
                rect.y -= 2;
                rect.width += 15;
                GUIStyle guiStyle = i % 2 == 0 ? TGCStyles.LabelEven : TGCStyles.LabelOdd;
                if (Event.current.type == EventType.Repaint) {
                    guiStyle.Draw(rect, content, false, false, this.selectedTexture == texture, false);
                }

                Rect textureNameRect = new Rect(rect.x + 3, rect.y, rect.width * 0.6f, rect.height);
                EditorGUI.LabelField(textureNameRect, " " + texture.name);

                Rect memoryRect = new Rect(rect.x + rect.width * 0.6f, rect.y, rect.width * 0.2f, rect.height);
                EditorGUI.LabelField(memoryRect, " --");

                Rect refCountRect = new Rect(rect.x + rect.width * 0.8f, rect.y, rect.width * 0.2f, rect.height);
                Object[] objs = this.objects[i];
                EditorGUI.LabelField(refCountRect, " " + objs.Length);

                if (GUI.Button(rect, "", new GUIStyle())) {
                    this.selectedTexture = texture;
                    Selection.SetActiveObjectWithContext(this.selectedTexture, this.selectedTexture);
                }
            }

            GUILayout.EndScrollView();
        }

        private void DrawReferenceList() {
            if (this.selectedTexture == null) {
                return;
            }

            int index = -1;
            for (int i = 0; i < this.textures.Length; i++) {
                if (this.selectedTexture == this.textures[i]) {
                    index = i;
                    break;
                }
            }

            if (index == -1) {
                return;
            }

            Object[] objs = this.objects[index];

            foreach (Object o in objs) {
                EditorGUILayout.LabelField(o.name);
            }
        }

    }

}