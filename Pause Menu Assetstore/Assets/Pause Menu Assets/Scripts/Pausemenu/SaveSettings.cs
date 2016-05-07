﻿using UnityEngine;
using System.IO;
using System;
/// <summary>
///  Copyright (c) 2016 Eric Zhu 
/// </summary>
namespace GreatArcStudios
{
    [System.Serializable]
    public class SaveSettings : MonoBehaviour
    {
        /// <summary>
        /// Change the file name if something else floats your boat
        /// </summary>
        public string fileName = "GameSettings.json";
        public float musicVolume;
        public float effectsVolume;
        public float masterVolume;
        public float shadowDistINI;
        public float renderDistINI;
        public float aaQualINI;
        public float densityINI;
        public float treeMeshAmtINI;
        public float fovINI;
        public float terrainHeightMapLOD;
        public int msaaINI;
        public int vsyncINI;
        public int textureLimit;
        public int curQualityLevel;
        public bool aoBool;
        public bool dofBool;
        public bool useSimpleTerrian;    
        /// <summary>
        /// The string that will be saved.
        /// </summary>
        static string jsonString;
        /// <summary>
        /// The string that will be read.
        /// </summary>
        static string readString;
        /// <summary>
        /// Load the game settings and check if the game settings file is missing. In that case throw an exception. 
        /// </summary>
        public void Start()
        {
            try
            {
                LoadGameSettings();
            }
            catch (FileNotFoundException)
            {
                Debug.Log("Game settings not found in: " + Application.persistentDataPath + "/" + fileName);
            }

        }

        /// <summary>
        /// Load the same settings
        /// </summary>
        public void LoadGameSettings()
        {
            readString = File.ReadAllText(Application.persistentDataPath + "/" + fileName);
            ReadJson read = ReadJson.createJSONOBJ(readString);
            QualitySettings.antiAliasing = (int)read.aaQualINI;
            PauseManager.densityINI = read.densityINI;
            QualitySettings.shadowDistance = read.shadowDistINI;
            PauseManager.mainCamShared.farClipPlane = read.renderDistINI;
            PauseManager.treeMeshAmtINI = read.treeMeshAmtINI;
            PauseManager.mainCamShared.fieldOfView = read.fovINI;
            QualitySettings.antiAliasing = read.msaaINI;
            QualitySettings.vSyncCount = read.vsyncINI;
            PauseManager.lastTexLimit = read.textureLimit;
            QualitySettings.masterTextureLimit = read.textureLimit;
            PauseManager.beforeMaster = read.masterVolume;
            PauseManager.lastAudioMult = read.effectsVolume;
            PauseManager.lastMusicMult = read.musicVolume;
            PauseManager.dofBool = read.dofBool;
            PauseManager.aoBool = read.aoBool;
            QualitySettings.SetQualityLevel(read.curQualityLevel);
            try
            {
                if (read.useSimpleTerrain)
                {
                    PauseManager.readTerrain.heightmapMaximumLOD = (int)read.terrainHeightMapLOD;
                }
                else
                {
                    PauseManager.readSimpleTerrain.heightmapMaximumLOD = (int)read.terrainHeightMapLOD;
                }
                PauseManager.readUseSimpleTerrain = read.useSimpleTerrain;
            }
            catch
            {
                Debug.Log("Cannot read terain heightmap LOD because the terrain was not assigned.");
            }
        }
        /// <summary>
        /// Get the quality/music settings before saving 
        /// </summary>
        public void SaveGameSettings()
        {
            if (File.Exists(Application.persistentDataPath + "/" + fileName))
            {
                File.Delete(Application.persistentDataPath + "/" + fileName);
            }
            aaQualINI = QualitySettings.antiAliasing;
            densityINI = PauseManager.densityINI;
            shadowDistINI = PauseManager.shadowDistINI;
            renderDistINI = PauseManager.mainCamShared.farClipPlane;
            treeMeshAmtINI = PauseManager.treeMeshAmtINI;
            fovINI = PauseManager.mainCamShared.fieldOfView;
            msaaINI = PauseManager.msaaINI;
            vsyncINI = PauseManager.vsyncINI;
            textureLimit = PauseManager.lastTexLimit;
            masterVolume = PauseManager.beforeMaster;
            effectsVolume = PauseManager.lastAudioMult;
            musicVolume = PauseManager.lastMusicMult;
            aoBool = PauseManager.aoBool;
            dofBool = PauseManager.dofBool;
            curQualityLevel = QualitySettings.GetQualityLevel();
            try
            {
                if (PauseManager.readUseSimpleTerrain)
                {
                    terrainHeightMapLOD = PauseManager.readTerrain.heightmapMaximumLOD;
                }
                else
                {
                    terrainHeightMapLOD = PauseManager.readSimpleTerrain.heightmapMaximumLOD;
                }
            }
            catch
            {
                Debug.Log("Cannot save terain heightmap LOD because the terrain was not assigned.");
            }
            useSimpleTerrian = PauseManager.readUseSimpleTerrain;
            jsonString = JsonUtility.ToJson(this);
            Debug.Log(jsonString);
            File.WriteAllText(Application.persistentDataPath + "/" + fileName, jsonString);
        }


    }
}