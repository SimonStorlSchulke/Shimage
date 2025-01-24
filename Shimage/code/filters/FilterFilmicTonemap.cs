using Godot;
using System;


public partial class FilterFilmicTonemap : Filter {
    public FilterFilmicTonemap() {

        Name = "Filmic Tonemap";

        type = FilterType.COLOR;

        Props = new Prop[] {
            new PropFloat("fm_brighthess", "Brightness", 0.8f, _slider: false, _max: 1000),
            new PropFloat("fm_toe", "Toe", 5f, _slider: false, _max: 1000),
            new PropFloat("fm_shoulder", "Shoulder", 1.8f, _slider: false, _max: 1000),
            new PropFloat("fm_add", "Add", 0.15f, _slider: false, _max: 1000),
        };

        Code = @"
        // Filmic Tonemap
        fg *= vec3(fm_brighthess);
        v3_1 = max(vec3(0.0), fg.rgb - 0.004);
        v3_2 = (v3_1 * (6.2 * v3_1 + fm_brighthess)) / (v3_1 * (fm_toe * v3_1 + fm_shoulder) + fm_add);
        v3_2 = pow(v3_2, vec3(2.2));
        fg = v3_2;";
    }
}
