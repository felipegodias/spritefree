using UnityEditor;
using UnityEngine;

namespace SpriteFree.Core.Editor {

    public class TextureGarbageCollectorWindow : EditorWindow {

        private TextureGarbageCollector textureGarbageCollector;

        private Texture[] textures;

        private Object[][] objects;

        private Vector2 scroll;

        [MenuItem("Window/Texture Garbage Collector")]
        public static void OpenWindow() {
            GetWindow<TextureGarbageCollectorWindow>(false, "Texture GC");
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

            this.scroll = GUILayout.BeginScrollView(this.scroll);

            for (int i = 0; i < this.textures.Length; i++) {
                Texture texture = this.textures[i];
                EditorGUILayout.LabelField(texture.name);
                Object[] texReferences = this.objects[i];
                EditorGUI.indentLevel++;
                foreach (Object texReference in texReferences) {
                    EditorGUILayout.LabelField(texReference.name);
                }
                EditorGUI.indentLevel--;
            }

            GUILayout.EndScrollView();
        }

    }

}