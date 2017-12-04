using UnityEngine;

namespace SpriteFree.Core.Modules {

    public class SpriteRendererTrackModule : ITrackModule {

        private readonly SpriteRenderer spriteRenderer;

        public Texture Texture => this.spriteRenderer.sprite.texture;

        public SpriteRendererTrackModule(SpriteRenderer spriteRenderer) {
            this.spriteRenderer = spriteRenderer;
        }

    }

}