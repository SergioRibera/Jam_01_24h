using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibraryPersonal;
using System;
using System.Linq;

public delegate void EventoClick(bool press);
public class Listener
{
    public string name;
    public EventoClick call;
}

[Serializable]
public class ManagerDataPlayer
{
    public static DataGame DataGame { get; private set; }
    static GeneralInputs generalInputs;

    static List<Listener> listener = new List<Listener>();
    static List<Listener> listenerUp = new List<Listener>();

    public static void Init(Joystick _j)
    {
        generalInputs = new GeneralInputs(_j);
        DataGame = Datos.Load<DataGame>(NameFiles.Data);
    }
    public static void Save()
    {
        DataGame.Save<DataGame>(NameFiles.Data);
    }
    public static void SetListenerDown(string name, EventoClick f)
    {
        listener.Add(new Listener()
        {
            name = name,
            call = f
        });
    }
    public static void SetButton(string name, bool press)
    {
        if (listener.Count > 0)
            foreach (var l in listener.Where(i => i.name == name))
                l.call?.Invoke(press);
        else
            return;
    }
    public static GeneralInputs GetInputs { get { return generalInputs; } }
    public static int GetGameVersion { get { return DataGame.game_Version; } }
    public static int GetPackageVersion { get { return DataGame.packages_Version; } }
    public static int SetGameVersion { set => DataGame.game_Version = value; }
    public static int SetPackageVersion { set => DataGame.packages_Version = value; }

    #region PersonajeDatos

    #region Gets
    public static string GetNombre { get => DataGame.personajeDatos.Nombre; }
    public static int GetCoins { get => DataGame.personajeDatos.coins; }
    public static int GetNivel { get => DataGame.personajeDatos.nivel; }
    public static int GetExperiencia { get => DataGame.personajeDatos.experiencia; }
    public static string GetFechaInicio { get => DataGame.personajeDatos.fechaInicio; }
    public static string GetUltimoInicio { get => DataGame.personajeDatos.ultimoInicio; }
    public static int GetDaysTranscurrent { get => DataGame.personajeDatos.daysTranscurrent; }

    #endregion

    #region Sets
    public static string SetNombre { set { DataGame.personajeDatos.Nombre = value; Save(); } }
    public static int SetCoins { set { DataGame.personajeDatos.coins = value; Save(); } }
    public static int SetNivel { set { DataGame.personajeDatos.nivel = value; Save(); } }
    public static int SetExperiencia { set { DataGame.personajeDatos.experiencia = value; Save(); } }
    public static string SetFechaInicio { set { DataGame.personajeDatos.fechaInicio = value; Save();} }
    public static string SetUltimoInicio { set { DataGame.personajeDatos.ultimoInicio = value; Save(); } }
    public static int SetDaysTranscurrent { set { DataGame.personajeDatos.daysTranscurrent = value; Save(); } }
    #endregion

    #endregion

    #region Settings

    #region Sets

    public static int SetQualityLevel { set { DataGame.settings.qualityLevel = value; Save(); } }
    public static bool SetHalfResolution { set { DataGame.settings.halfResolution = value; Save(); } }
    public static float SetMusic { set { DataGame.settings.music = value; Save(); } }
    public static float SetFx { set { DataGame.settings.fx = value; Save(); } }
    public static GeneralInputs SetIputsPc { set { DataGame.settings.iputsPc = value; Save(); } }
    public static HUD SetConfigHUDGame { set { DataGame.settings.configHUDGame = value; Save(); } }

    #endregion

    #region Gets

    public static int GetQualityLevel { get => DataGame.settings.qualityLevel; }
    public static bool GetHalfResolution { get => DataGame.settings.halfResolution; }
    public static float GetMusic { get => DataGame.settings.music; }
    public static float GetFx { get => DataGame.settings.fx; }
    public static GeneralInputs GetIputsPc { get => DataGame.settings.iputsPc; }
    public static HUD GetConfigHUDGame { get => DataGame.settings.configHUDGame; }

    #endregion

    #endregion
}