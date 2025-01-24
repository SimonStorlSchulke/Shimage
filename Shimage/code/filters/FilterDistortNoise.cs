using Godot;
using System;


public partial class FilterDistortNoise : Filter
{
    public FilterDistortNoise() {

        Name = "Noise Distort";

        type = FilterType.DISTORT;
        
        Props = new Prop[] {
            new PropFloat("noiseDistortAmmount", "Distort", 0.2f),
            new PropFloat("noiseDistortOffset", "Offset", 0.15f, _slider: false, _min: -10000, _max: 10000),
            new PropFloat("noiseDistortScale", "Scale", 10, _slider: false, _max: 10000),
            new PropInt("noiseDistortOctaves", "FractalOctaves", 3, 12),
        };

        Code = @"
        // Noise Distort
        uv = mix(uv, vec2(fbm(uv * noiseDistortScale + noiseDistortOffset, noiseDistortOctaves)), noiseDistortAmmount*noiseDistortAmmount);
        ";
    }
}
