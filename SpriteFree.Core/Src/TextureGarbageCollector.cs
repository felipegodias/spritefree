using System.Collections.Generic;
using UnityEngine;

namespace SpriteFree.Core {

    public class TextureGarbageCollector : MonoBehaviour {

        private static TextureGarbageCollector instance;

        private Dictionary<Texture, HashSet<Object>> references = new Dictionary<Texture, HashSet<Object>>();

        private bool isDirty;

        public static TextureGarbageCollector Instance {
            get {
                if (!Application.isPlaying) {
                    return null;
                }

                if (instance != null) {
                    return instance;
                }

                GameObject textureCollectorGameObject = new GameObject(nameof(TextureGarbageCollector));
                DontDestroyOnLoad(textureCollectorGameObject);
                instance = textureCollectorGameObject.AddComponent<TextureGarbageCollector>();
                return instance;
            }
        }

        public bool IsDirty => this.isDirty;

        public void GetReferences(out Texture[] textures, out Object[][] objects) {
            textures = new Texture[this.references.Count];
            objects = new Object[this.references.Count][];
            int i = 0;
            foreach (KeyValuePair<Texture, HashSet<Object>> keyValuePair in this.references) {
                textures[i] = keyValuePair.Key;
                HashSet<Object> texReferences = keyValuePair.Value;
                objects[i] = new Object[texReferences.Count];
                int j = 0;
                foreach (Object texReference in texReferences) {
                    objects[i][j] = texReference;
                    j++;
                }

                i++;
            }

            this.isDirty = false;
        }

        public void Add(Texture texture, Object obj) {
            if (this.references == null) {
                this.references = new Dictionary<Texture, HashSet<Object>>();
            }

            if (!this.references.ContainsKey(texture)) {
                this.references.Add(texture, new HashSet<Object>());
            }

            HashSet<Object> texReferences = this.references[texture];
            if (texReferences.Contains(obj)) {
                return;
            }

            texReferences.Add(obj);
            this.isDirty = true;
        }

        public void Remove(Texture texture, Object obj) {
            if (this.references == null) {
                return;
            }

            if (!this.references.ContainsKey(texture)) {
                return;
            }

            HashSet<Object> texReferences = this.references[texture];

            texReferences.Remove(obj);

            if (texReferences.Count > 0) {
                return;
            }

            this.references.Remove(texture);
            Resources.UnloadAsset(texture);
            this.isDirty = true;
        }

    }

}