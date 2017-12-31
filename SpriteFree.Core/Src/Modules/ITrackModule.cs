using UnityEngine;

namespace SpriteFree.Core.Modules {

    public interface ITrackModule {

        Texture Texture { get; }

        void Reload();

    }

}