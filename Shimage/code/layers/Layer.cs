using Godot;
using System;
using System.Collections.Generic;

public interface ILayer {
    void UpdateMaterial();
    void UpdateLayer();
    float GetBlendFactor();
    void SetBlendFactor(float fac);
    void ApplyProp(Variant value, string name);
    Vector2 GlobalToPixelCoord(Vector2 globalCoords);
    Vector2 GlobalToUVCoord(Vector2 globalCoords);
    Vector2 UVToGlobalCoord(Vector2 pixelCoords);
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
        ToolsLayer.activeTool.DeactivateTool();
    }

    public static void SetFilters(this ILayer layer, List<Filter> filters) {
        layer.Filters = filters;
    }
}
