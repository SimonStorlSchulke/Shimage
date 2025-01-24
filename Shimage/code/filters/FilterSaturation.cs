
public partial class FilterSaturation : Filter {
    public FilterSaturation() {

        Name = "Saturation";

        type = FilterType.COLOR;

        Props = new Prop[] {
            new PropFloat("sat", "Fac", 0.5f, _max: 2.0f),
        };

        Code = @"
    // Saturation
    v3_1 = vec3(0.2125, 0.7154, 0.0721);
    v3_2 = vec3(dot(fg.rgb, v3_1));
    fg = mix(v3_2, fg, sat * 2.0);
    ";
    }
}
