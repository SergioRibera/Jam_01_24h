using System;
using System.Collections.Generic;

#region CLASES
[Serializable]
public class DataGame
{
    public PersonajeDatos personajeDatos;
    public Settings settings;
    public int game_Version;
    public int packages_Version;

    public DataGame()
    {
        this.personajeDatos = new PersonajeDatos();
        this.settings = new Settings();
    }
}
[Serializable]
public class PersonajeDatos
{
    public string Nombre;
    public int coins = 0;
    public int nivel = 0;
    public int experiencia = 0;
    public string fechaInicio;
    public string ultimoInicio;
    public int daysTranscurrent;

    public override string ToString()
    {
        return string.Format("{0}: {1} nivel {2}", Nombre, nivel, experiencia);
    }
}

[Serializable]
public class Settings
{
    public int qualityLevel = 4;
    public bool halfResolution = false;
    public float music = 1;
    public float fx = 1;
    public GeneralInputs iputsPc;
    public HUD configHUDGame;
}
[Serializable]
public class HUD
{
    public List<ItemHUD> itemsHUD = new List<ItemHUD>();
    public List<ItemHUD> itemsHUDDEFAULT = new List<ItemHUD>();

}
[Serializable]
public class ItemHUD
{
    public int id;
    public float posX, posY;
    public float escX, escY;
}

[Serializable]
public struct NameFiles
{
    public const string Data = "Data";
    public const string RemoteSettings = "RemoteSettings";
}
#endregion