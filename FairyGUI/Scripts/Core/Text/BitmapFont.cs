﻿using System;
using System.Collections.Generic;
using CryEngine;

namespace FairyGUI
{
	/// <summary>
	/// 
	/// </summary>
	public class BitmapFont : BaseFont
	{
		/// <summary>
		/// 
		/// </summary>
		public class BMGlyph
		{
			public int offsetX;
			public int offsetY;
			public int width;
			public int height;
			public int advance;
			public int lineHeight;
			public Vector2[] uv = new Vector2[4];
			public int channel;//0-n/a, 1-r,2-g,3-b,4-alpha
		}

		/// <summary>
		/// 
		/// </summary>
		public int size;

		/// <summary>
		///  Can this font be tinted? Will be true for dynamic font and fonts generated by BMFont.
		/// </summary>
		public bool colorEnabled;

		/// <summary>
		/// 
		/// </summary>
		public bool scaleEnabled;

		/// <summary>
		/// The texture of this font object.
		/// </summary>
		public NTexture mainTexture;

		Dictionary<int, BMGlyph> _dict;
		float scale;

		public BitmapFont(PackageItem item)
		{
			this.name = UIPackage.URL_PREFIX + item.owner.id + item.id;
			_dict = new Dictionary<int, BMGlyph>();
			this.scale = 1;
		}

		public void AddChar(char ch, BMGlyph glyph)
		{
			_dict[ch] = glyph;
		}

		override public void SetFormat(TextFormat format, float fontSizeScale)
		{
			if (scaleEnabled)
				this.scale = (float)format.size / size * fontSizeScale;
			else
				this.scale = fontSizeScale;
		}

		public bool GetGlyphSize(char ch, out float width, out float height)
		{
			BMGlyph bg;
			if (ch == ' ')
			{
				width = (int)Math.Ceiling(size * scale / 2);
				height = (int)Math.Ceiling(size * scale);
				return true;
			}
			else if (_dict.TryGetValue((int)ch, out bg))
			{
				width = (int)Math.Ceiling(bg.advance * scale);
				height = (int)Math.Ceiling(bg.lineHeight * scale);
				return true;
			}
			else
			{
				width = 0;
				height = 0;
				return false;
			}
		}

		public bool GetGlyph(char ch, GlyphInfo glyph)
		{
			BMGlyph bg;
			if (ch == ' ')
			{
				glyph.width = (int)Math.Ceiling(size * scale / 2);
				glyph.height = (int)Math.Ceiling(size * scale);
				glyph.vert.x = 0;
				glyph.vert.Width = 0;
				glyph.vert.y = 0;
				glyph.vert.Height = 0;
				glyph.uv[0] = glyph.uv[1] = glyph.uv[2] = glyph.uv[3] = Vector2.Zero;
				glyph.channel = 0;
				return true;
			}
			else if (_dict.TryGetValue((int)ch, out bg))
			{
				glyph.width = (int)Math.Ceiling(bg.advance * scale);
				glyph.height = (int)Math.Ceiling(bg.lineHeight * scale);
				glyph.vert.x = bg.offsetX * scale;
				glyph.vert.Width = bg.width * scale;
				glyph.vert.y = bg.offsetY * scale;
				glyph.vert.Height = bg.height * scale;
				glyph.uv[0] = bg.uv[0];
				glyph.uv[1] = bg.uv[1];
				glyph.uv[2] = bg.uv[2];
				glyph.uv[3] = bg.uv[3];
				glyph.channel = bg.channel;
				return true;
			}
			else
				return false;
		}
	}
}
