using UnityEngine;
using UnityEngine.Assertions;

public class ColorizePixels : BaseBehaviour {

	//These are the colors it will change to. The colors that change, are decided in the loop.
	public Color _color1 = new Color32(96, 60, 40, 255);
	public Color _color2 = new Color32(247, 207, 134, 255);
	public Color _color3 = new Color32(55, 55, 55, 255);
	public Color _color4 = new Color32(240, 240, 240, 255);
	public Color _color5 = new Color32(240, 240, 240, 255);
	public Color _color6 = new Color32(240, 240, 240, 255);
	public Color _color7 = new Color32(127, 127, 127, 255);
	public Color _color8 = new Color32(247, 207, 134, 255);
	public Color _color9 = new Color32(255, 255, 0, 255);
	public Color _color0 = Color.black;
	readonly string[] heroAnimations = { "SWORD_Idle", "SWORD_Run", "SWORD_Jump", "SWORD_Attack" };

	void Start() 
	{
		ColorizeTexture();
	}

	public void ColorizeTexture()
	{
		foreach (string textureName in heroAnimations)
		{
			var texture = Resources.Load<Texture2D>("Sprites/Hero/" + textureName);
			Assert.IsNotNull(texture);

			Color[] pixelColors = texture.GetPixels(0, 0, texture.width, texture.height);

			int y = 0;
			while (y < pixelColors.Length)
			{
				if (pixelColors[y] == new Color32(255, 0, 0, 255))
				{
					pixelColors[y] = (_color1);
				}
				else if (pixelColors[y] == new Color32(0, 255, 0, 255))
				{
					pixelColors[y] = (_color2);
				}
				else if (pixelColors[y] == new Color32(0, 0, 255, 255))
				{
					pixelColors[y] = (_color3);
				}
				else
				{
					pixelColors[y] = (pixelColors[y]);
				}

				++y;
			}

			texture.SetPixels(pixelColors);
			texture.Apply();
		}

	}
}
