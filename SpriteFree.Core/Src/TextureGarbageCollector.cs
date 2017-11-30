﻿using System.Collections.Generic;
using UnityEngine;

namespace SpriteFree.Core {

    public class TextureGarbageCollector : MonoBehaviour {

        private static TextureGarbageCollector instance;

        private Dictionary<Texture, HashSet<Object>> references = new Dictionary<Texture, HashSet<Object>>();

        public static TextureGarbageCollector Instance {
            get {
                if (instance != null) {
                    return instance;
                }

                GameObject textureCollectorGameObject = new GameObject(nameof(TextureGarbageCollector));
                DontDestroyOnLoad(textureCollectorGameObject);
                instance = textureCollectorGameObject.AddComponent<TextureGarbageCollector>();
                return instance;
            }
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
        }

    }

}