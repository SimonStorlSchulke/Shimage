using Godot;
using System;
using System.Collections.Generic;

public interface ILayer {
    void UpdateMaterial();
    void UpdateLayer();
    float GetBlendFactor();
    void SetBlendFactor(float fac);
    void ApplyProp(object value, string name);
    ShaderUtil.BlendMode GetBlendmode();
    void SetBlendmode(ShaderUtil.BlendMode blendmode);
    List<Filter> Filters { get; set; }
}

public static class LayerExtensions {
    public static void AddFilter(this ILayer layer, Filter filter) {
        if (layer.Filters == null) {
            layer.Filters = new List<Filter>();
        }
        filter.MakePropsUnique();
        layer.Filters.Add(filter);
        layer.UpdateLayer();
    }

    public static void RemoveFilter(this ILayer layer, Filter filter) {
        layer.Filters.Remove(filter);
        FilterManager.instance.RemoveChild(filter.UI);
        filter.UI.QueueFree();
        filter.QueueFree();
        layer.UpdateLayer();
    }

    public static void SetFilters(this ILayer layer, List<Filter> filters) {
        layer.Filters = filters;
    }
}
