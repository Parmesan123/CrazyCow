using Services;
using UnityEngine;

namespace Saving
{
    public class SaveHandler : MonoBehaviour
    {
        private const string SAVE_FILENAME = "Save.json";

        public GameData SaveData;
    
        private ISaveMaker _saveMaker;

        private void Awake()
        {
            _saveMaker = new JsonSaveMaker();
        }

        public void Save()
        {
            _saveMaker.Save(SaveData, SAVE_FILENAME);
        }

        public void Load()
        {
            SaveData = _saveMaker.Load<GameData>(SAVE_FILENAME) ?? new GameData();
        }
    }   
}