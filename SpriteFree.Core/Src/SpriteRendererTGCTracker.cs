using UnityEngine;

namespace SpriteFree.Core {

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererTGCTracker : TextureGarbageCollectorTracker {

        private SpriteRenderer spriteRenderer;

        private SpriteRenderer SpriteRenderer =>
            this.spriteRenderer ?? (this.spriteRenderer = this.GetComponent<SpriteRenderer>());

        protected override Texture Texture {
            get {
                Sprite sprite = this.SpriteRenderer.sprite;
                return sprite == null ? null : sprite.texture;
            }
        }

    }

}