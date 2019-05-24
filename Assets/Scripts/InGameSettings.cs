using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSettings : MonoBehaviour {
    public MapGenerator mapGenerator;
    public Slider noiseSlider;
    public Slider octavesSlider;
    public Slider octaveFrequencyIncreaseSlider;
    public Slider octaveAmplitudeDecreaseSlider;
    public Slider meshHeightScaleSlider;
    public Slider LODSlider;

    public void Start() {
        ResetValues();
        mapGenerator.GenerateMap(true);
    }

    public void SetNoiseScale(float noiseScale) {
        mapGenerator.noiseScale = noiseScale;

        // Regenerate the map
        mapGenerator.GenerateMap(false);
    }

    public void SetOctaves(float octaves) {
        mapGenerator.octaves = (int)octaves;

        // Regenerate the map
        mapGenerator.GenerateMap(false);
    }

    public void SetOctaveFrequencyIncrease(float octaveFrequencyIncrease) {
        mapGenerator.octaveFrequencyIncrease = octaveFrequencyIncrease;

        // Regenerate the map
        mapGenerator.GenerateMap(false);
    }

    public void SetOctaveAmplitudeDecrease(float octaveAmplitudeDecrease) {
        mapGenerator.octaveAmplitudeDecrease = octaveAmplitudeDecrease;

        // Regenerate the map
        mapGenerator.GenerateMap(false);
    }

    public void SetMeshHeightScale(float meshHeightScale) {
        mapGenerator.meshHeightScale = meshHeightScale;

        // Regenerate the map
        mapGenerator.GenerateMap(false);
    }

    public void SetLOD(float LOD) {
        mapGenerator.LOD = (int)LOD;

        // Regenerate the map
        mapGenerator.GenerateMap(false);
    }

    public void ResetValues() {
        // Reset the script values.
        mapGenerator.ResetValues();
        
        // Reset the slider values.
        noiseSlider.value = MapGenerator.DEFAULT_NOISE_SCALE;
        octavesSlider.value = MapGenerator.DEFAULT_OCTAVES;
        octaveFrequencyIncreaseSlider.value = MapGenerator.DEFAULT_OCTAVE_FREQUENCY_INCREASE;
        octaveAmplitudeDecreaseSlider.value = MapGenerator.DEFAULT_OCTAVE_AMPLITUDE_DECREASE;
        meshHeightScaleSlider.value = MapGenerator.DEFAULT_MESH_HEIGHT_SCALE;
        LODSlider.value = MapGenerator.DEFAULT_LOD;

        // Regenerate the map
        mapGenerator.GenerateMap(false);
    }

    public void GenerateRandom() {
        // Regenerate the map
        mapGenerator.GenerateMap(true);
    }
}
