using System.Linq;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    [CreateAssetMenu(fileName = "New Audio Database", menuName = "RTS_Voxel/Audio System/Audio Database")]
    public class AudioDatabaseModel : ScriptableObject
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //AudioDatabaseModel - (0.1)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        public SerializableDictionary<string, AudioClip> clipData = new();

        public AudioClip GetClip(string clipKey) => clipData.First(e => e.Key == clipKey).Value;
    }
}