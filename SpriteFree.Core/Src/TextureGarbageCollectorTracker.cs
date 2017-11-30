using UnityEngine;

namespace SpriteFree.Core {

    public abstract class TextureGarbageCollectorTracker : MonoBehaviour {

        [SerializeField]
        private bool clearTextureOnDestroy;

        [SerializeField]
        private bool clearTextureOnDisable;

        private TextureGarbageCollector textureGarbageCollector;

        private Texture lastUsedTexture;

        public bool ClearTextureOnDestroy => this.clearTextureOnDestroy;

        public bool ClearTextureOnDisable => this.clearTextureOnDisable;

        protected TextureGarbageCollector TextureGarbageCollector =>
            this.textureGarbageCollector ?? (this.textureGarbageCollector = TextureGarbageCollector.Instance);

        protected abstract Texture Texture { get; }

        private void LateUpdate() {
            Texture texture = this.Texture;
            if (this.lastUsedTexture == texture) {
                return;
            }

            if (this.lastUsedTexture != null) {
                this.TextureGarbageCollector.Remove(this.lastUsedTexture, this);
            }

            this.lastUsedTexture = texture;

            if (this.lastUsedTexture == null) {
                return;
            }

            this.TextureGarbageCollector.Add(this.lastUsedTexture, this);
        }

        private void OnDestroy() {
            if (!this.ClearTextureOnDestroy || this.lastUsedTexture == null) {
                return;
            }

            this.TextureGarbageCollector.Remove(this.lastUsedTexture, this);
            this.lastUsedTexture = null;
        }

        private void OnDisable() {
            if (!this.ClearTextureOnDisable || this.lastUsedTexture == null) {
                return;
            }

            this.TextureGarbageCollector.Remove(this.lastUsedTexture, this);
            this.lastUsedTexture = null;
        }

    }

}