using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIMaskImage))]
public class UIClickMaskImage : Image
{
    UIMaskImage mask_image;
    protected override void Start() {
        mask_image = GetComponent<UIMaskImage>();
    }
	override public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		Vector2 local;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out local);

		Rect rect = GetPixelAdjustedRect();
		// Convert to have lower left corner as reference point.
		local.x += rectTransform.pivot.x * rect.width;
		local.y += rectTransform.pivot.y * rect.height;

        Sprite mask_sprite = mask_image == null || mask_image.mask_sprite == null ? sprite : mask_image.mask_sprite;

        int x = Mathf.FloorToInt(local.x * mask_sprite.rect.width / rect.width);
        int y = Mathf.FloorToInt(local.y * mask_sprite.rect.height / rect.height);

        float Tw = mask_sprite.rect.width;
        float Th = mask_sprite.rect.height;
		int Rw = Mathf.FloorToInt(rect.width);
		int Rh = Mathf.FloorToInt(rect.height);
		if (preserveAspect)
		{
			if (Tw > Th)
			{
				float h = Rw * Th / Tw;
				if (h > Rh)//已宽度缩放，如果得出的高度大于实际上显示区域高度，说明是以高度缩放的
				{
					y = Mathf.FloorToInt(local.y * Th / Rh);
					x = Mathf.FloorToInt(local.x - (Rw - Tw * Rh / Th) / 2f);
					if (x < 0 && x > (Rw - Tw * Rh / Th) / 2f)
					{
						return false;
					}
					else
					{
						x = Mathf.FloorToInt(x * Th / Rh);
					}
				}
				else
				{
					x = Mathf.FloorToInt(local.x * Tw / Rw);
					y = Mathf.FloorToInt(local.y - (Rh - Th * Rw / Tw) / 2f);
					if (y < 0 && y > (Rh - Th * Rw / Tw) / 2f)
					{
						return false;
					}
					else
					{
						y = Mathf.FloorToInt(y * Tw / Rw);
					}
				}
			}
			else
			{
				float w = Rh * Tw / Th;
				if (w > Rw)//已宽度缩放，如果得出的高度大于实际上显示区域高度，说明是以高度缩放的
				{
					x = Mathf.FloorToInt(local.x * Tw / Rw);
					y = Mathf.FloorToInt(local.y - (Rh - Th * Rw / Tw) / 2f);
					if (y < 0 && y > (Rh - Th * Rw / Tw) / 2f)
					{
						return false;
					}
					else
					{
						y = Mathf.FloorToInt(y * Tw / Rw);
					}
				}
				else
				{
					y = Mathf.FloorToInt(local.y * Th / Rh);
					x = Mathf.FloorToInt(local.x - (Rw - Tw * Rh / Th) / 2f);
					if (x < 0 && x > (Rw - Tw * Rh / Th) / 2f)
					{
						return false;
					}
					else
					{
						x = Mathf.FloorToInt(x * Th / Rh);
					}
				}
			}
		}
        x = Mathf.FloorToInt(x + mask_sprite.rect.x);
        y = Mathf.FloorToInt(y + mask_sprite.rect.y);
		try
		{
            var col = mask_sprite.texture.GetPixel(x, y);
            return col.a >= 0.1f;
		}
		catch (UnityException e)
		{
			Debug.LogError("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + e.Message + " Also make sure to disable sprite packing for this sprite.", this);
			return true;
		}
	}
}
