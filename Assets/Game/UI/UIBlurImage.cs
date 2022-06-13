using UnityEngine;
using UnityEngine.UIElements;

public class UIBlurImage : VisualElement
{
    private readonly Material _blurMaterial = new(Shader.Find("Custom/MaskedUIBlur"));

    private Texture _srcTexture;
    private RenderTexture _destTexture;

    public int Radius
    {
        get => _blurMaterial.GetInt("_Radius");
        set
        {
            _blurMaterial.SetInt("_Radius", value);
            BlurTexture();
        }
    }

    public UIBlurImage()
    {

        RegisterCallback<GeometryChangedEvent>(evt =>
        {
            BlurTexture();
        });

        RegisterCallback<AttachToPanelEvent>(evt =>
        {
            BlurTexture();
        });
    }

    private void BlurTexture()
    {
        if (_srcTexture == null)
            _srcTexture = resolvedStyle.backgroundImage.texture ?? resolvedStyle.backgroundImage.sprite?.texture;

        if (_srcTexture == null)
            return;


        style.backgroundImage = ToTexture2D();
        this.MarkDirtyRepaint();
    }

    private Texture2D ToTexture2D()
    {

        RenderTexture currentRT = RenderTexture.active;

        var renderTexture = new RenderTexture(_srcTexture.width, _srcTexture.height, 32);
        Graphics.Blit(_srcTexture, renderTexture, _blurMaterial);

        RenderTexture.active = renderTexture;
        var texture2D = new Texture2D(_srcTexture.width, _srcTexture.height, TextureFormat.RGBA32, false);
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;

        return texture2D;
    }

    public new class UxmlFactory : UxmlFactory<UIBlurImage, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        private readonly UxmlIntAttributeDescription _radius = new()
        {
            name = "radius",
            defaultValue = 10
        };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var image = ve as UIBlurImage;

            image.Radius = _radius.GetValueFromBag(bag, cc);
        }
    }
}