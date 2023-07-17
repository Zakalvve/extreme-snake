using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.SearchService;
using UnityEngine;

namespace Assets.Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "My Assets/Level Model")]
    public class LevelModel: ScriptableObject
    {
        public string SceneName;
        public Sprite Thumbnail;
        public string LevelName;
        public int MaxPlayers = 1;
    }
}
