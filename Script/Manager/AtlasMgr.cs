using Cysharp.Threading.Tasks;
using Script.Inject;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Script.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Script.Manager
{
    public enum AtlasType
    {
        NONE,
        UI_PUZZLE,
    }

    public class AtlasMgr : ScriptMgr
    {
        private ResourceMgr _resourceMgr;
        
        private Dictionary<AtlasType, SpriteAtlas> _atlasDictionary;

        public override void Initialize()
        {
            base.Initialize();

            _atlasDictionary = new Dictionary<AtlasType, SpriteAtlas>();
            RegisterAtlas();
        }

        private void RegisterAtlas()
        {
            LoadAtlas(AddressableID.ATLAS_PUZZLE_ATLAS);
        }

        private async void LoadAtlas(string id)
        {
            var atlas = await _resourceMgr.GetObjById<SpriteAtlas>(id);
            if (atlas == null)
            {
                Log.EF(LogCategory.ATLAS, "Cannot load resource from atlas Id '{0}', cannot register atlas to dictionary", id);
                return;
            }

            _atlasDictionary.Add(AtlasType.UI_PUZZLE, atlas);
        }

        public override void Inject()
        {
            base.Inject();
            _resourceMgr = Injector.GetInstance<ResourceMgr>();
        }

        public Sprite GetSprite(AtlasType type, string id)
        {
            var sprite = GetAtlas(type)?.GetSprite(id);

            if(sprite == null)
            {
                Log.EF(LogCategory.ATLAS, "Cannot load sprite from atlas '{0}' - '{1}'", type, id);
            }

            return sprite;
        }

        private SpriteAtlas GetAtlas(AtlasType type)
        {
            if (_atlasDictionary.TryGetValue(type, out var atlas) == false)
            {
                Log.EF(LogCategory.ATLAS, "Cannot load atlas from type '{0}'", type);
                return null;
            }

            return atlas;
        }
    }
}
