using UnityEngine;

namespace SpriteFree.Core.Modules {

    public class SpriteRendererTrackModule : ITrackModule {

        private readonly SpriteRenderer spriteRenderer;

        public Texture Texture => this.spriteRenderer.sprite.texture;

        public SpriteRendererTrackModule(SpriteRenderer spriteRenderer) {
            this.spriteRenderer = spriteRenderer;
        }

        public void Reload() {
            Sprite sprite = this.spriteRenderer.sprite;
            this.spriteRenderer.sprite = null;
            this.spriteRenderer.sprite = sprite;
        }

    }

}