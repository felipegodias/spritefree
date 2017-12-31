using SpriteFree.Core.Modules;
using UnityEngine;

namespace SpriteFree.Core {

    public class TGCTracker : MonoBehaviour {

        [SerializeField]
        private bool clearTextureOnDestroy;

        [SerializeField]
        private bool clearTextureOnDisable;

        [SerializeField]
        private bool clearTextureOnInvisible;

        private TextureGarbageCollector textureGarbageCollector;

        private ITrackModule trackModule;

        private Texture lastUsedTexture;

        public bool ClearTextureOnDestroy => this.clearTextureOnDestroy;

        public bool ClearTextureOnDisable => this.clearTextureOnDisable;

        public bool ClearTextureOnInvisible => this.clearTextureOnInvisible;

        protected TextureGarbageCollector TextureGarbageCollector =>
            this.textureGarbageCollector ?? (this.textureGarbageCollector = TextureGarbageCollector.Instance);

        private void Awake() {
            this.LoadModule();
        }

        private void LateUpdate() {
            Texture texture = this.trackModule.Texture;
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
        }

        private void OnDisable() {
            if (!this.ClearTextureOnDisable || this.lastUsedTexture == null) {
                return;
            }

            this.TextureGarbageCollector.Remove(this.lastUsedTexture, this);
        }

        private void OnBecameVisible() {
            if (!this.ClearTextureOnInvisible || this.lastUsedTexture == null) {
                return;
            }

            this.lastUsedTexture =  this.trackModule.Texture;
            this.TextureGarbageCollector.Add(this.lastUsedTexture, this);
            this.trackModule.Reload();
        }

        private void OnBecameInvisible() {
            if (!this.ClearTextureOnInvisible || this.lastUsedTexture == null) {
                return;
            }

            this.TextureGarbageCollector.Remove(this.lastUsedTexture, this);
        }

        private void LoadModule() {
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) {
                this.trackModule = new SpriteRendererTrackModule(spriteRenderer);
                return;
            }
        }

    }

}