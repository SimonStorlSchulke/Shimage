using Godot;
using System;

public class FilterVibrancy : Filter {
    public FilterVibrancy() {

        Name = "Vibrancy";

        type = FilterType.COLOR;

        Props = new Prop[] {
            new PropFloat("vib_fac", "Fac", 0.5f, _max: 2.0f),
            new PropFloat("vib_pow", "Raise Low Sat", 2.0f, _slider: false, _max: 10),
        };

        Code = @"
    // Vibrancy
    f_1 = fg.r*0.299 + fg.g*0.587 + fg.b*0.114; //lum
    f_2 = min(min(fg.r, fg.g), fg.b); //mn
    f_3 = max(max(fg.r, fg.g), fg.b); //mx
    f_4 = (1.0-(f_3 - f_2)) * (1.0-f_3) * f_1 * 5.0; //sat
    v3_1 = vec3(0.2125, 0.7154, 0.0721);
    v3_2 = vec3(dot(fg.rgb, v3_1));
    f_5 = pow(f_4, vib_pow) * vib_fac;
    fg = mix(v3_2, fg, 1.0 + clamp(f_5, 0.0, 1.0));
    ";
    }
}
