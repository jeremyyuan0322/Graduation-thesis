using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerName : MonoBehaviour
{
    public void EnterPlayerName(Text enterText)
    {////(2)
        SaveData.PName = enterText.text;////(3)
    }
}
