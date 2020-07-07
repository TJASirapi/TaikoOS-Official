using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using FDK;

namespace TJAPlayer3
{
    class CStageメンテナンス : CStage
    {
		// コンストラクタ

		public CStageメンテナンス()
		{
			base.eステージID = CStage.Eステージ.メンテ;
			base.b活性化してない = true;
		}
		// CStage 実装

		public override void On活性化()
		{
			Trace.TraceInformation("メンテナンスステージを活性化します。");
			Trace.Indent();
			try
			{
				Discord.UpdatePresence("", "Maintenance", TJAPlayer3.StartupTime);
				base.On活性化();
			}
			finally
			{
				Trace.TraceInformation("メンテナンスの活性化を完了しました。");
				Trace.Unindent();
			}

		}

		public override void On非活性化()
		{
			Trace.TraceInformation("メンテナンスステージを非活性化します。");
			Trace.Indent();
			try
			{
			}
			finally
			{
				Trace.TraceInformation("メンテナンスステージの非活性化を完了しました。");
				Trace.Unindent();
			}
			base.On非活性化();
		}

		public override void OnManagedリソースの作成()
		{
			if (!base.b活性化してない)
			{
				don = TJAPlayer3.ColorTexture("#ff4000", Width, Height);
				ka = TJAPlayer3.ColorTexture("#00c8ff", Width, Height);
				moji[0] = GenerateTexture(new TitleTextureKey("左ふち", new CPrivateFastFont(new FontFamily(TJAPlayer3.ConfigIni.FontName), fontsize), Color.White, Color.Black));
				moji[1] = GenerateTexture(new TitleTextureKey("左面", new CPrivateFastFont(new FontFamily(TJAPlayer3.ConfigIni.FontName), fontsize), Color.White, Color.Black));
				moji[2] = GenerateTexture(new TitleTextureKey("右面", new CPrivateFastFont(new FontFamily(TJAPlayer3.ConfigIni.FontName), fontsize), Color.White, Color.Black));
				moji[3] = GenerateTexture(new TitleTextureKey("右ふち", new CPrivateFastFont(new FontFamily(TJAPlayer3.ConfigIni.FontName), fontsize), Color.White, Color.Black));
				base.OnManagedリソースの作成();
			}

		}
		public override void OnManagedリソースの解放()
		{
			if (!base.b活性化してない)
			{
				for (int i = 0; i < moji.Length; i++)
				{
					TJAPlayer3.tテクスチャの解放(ref moji[i]);
				}
				TJAPlayer3.tテクスチャの解放(ref don);
				TJAPlayer3.tテクスチャの解放(ref ka);
				base.OnManagedリソースの解放();
			}

		}

		public override int On進行描画()
		{
			if (base.b初めての進行描画)
			{
				base.b初めての進行描画 = false;
			}
			if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LBlue))
				ka.t2D下中央基準描画(TJAPlayer3.app.Device, 640 - (Sabunn + Width) * 4, y);
			if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LRed))
				don.t2D下中央基準描画(TJAPlayer3.app.Device, 640 - (Sabunn + Width) * 3, y);
			if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RRed))
				don.t2D下中央基準描画(TJAPlayer3.app.Device, 640 - (Sabunn + Width) * 2, y);
			if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RBlue))
				ka.t2D下中央基準描画(TJAPlayer3.app.Device, 640 - (Sabunn + Width) * 1, y);
			if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LBlue2P))
				ka.t2D下中央基準描画(TJAPlayer3.app.Device, 640 + (Sabunn + Width) * 1, y);
			if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LRed2P))
				don.t2D下中央基準描画(TJAPlayer3.app.Device, 640 + (Sabunn + Width) * 2, y);
			if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RRed2P))
				don.t2D下中央基準描画(TJAPlayer3.app.Device, 640 + (Sabunn + Width) * 3, y);
			if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RBlue2P))
				ka.t2D下中央基準描画(TJAPlayer3.app.Device, 640 + (Sabunn + Width) * 4, y);

			for (int nPlayer = 0; nPlayer < 2; nPlayer++)
			{
				for (int index = 0; index < 4; index++) {
					if (nPlayer == 0)
						moji[index].t2D下中央基準描画(TJAPlayer3.app.Device, 640 - (Sabunn + Width) * (4 -index), mojiy);
					else
						moji[index].t2D下中央基準描画(TJAPlayer3.app.Device, 640 + (Sabunn + Width) * (index + 1), mojiy);
				}
			}


			if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDXKeys.Key.Escape))
				return 1;
			return 0;
		}

		#region[private]

		private CTexture GenerateTexture (TitleTextureKey title){
			using (var bmp = new Bitmap(title.cPrivateFastFont.DrawPrivateFont(
				title.str文字, title.forecolor, title.backcolor)))
			{
				CTexture tx文字テクスチャ = TJAPlayer3.tテクスチャの生成(bmp, false);
				return tx文字テクスチャ;
			}
		}

		private sealed class TitleTextureKey
		{
			public readonly string str文字;
			public readonly CPrivateFastFont cPrivateFastFont;
			public readonly Color forecolor;
			public readonly Color backcolor;

			public TitleTextureKey(string str文字, CPrivateFastFont cPrivateFastFont, Color forecolor, Color backcolor)
			{
				this.str文字 = str文字;
				this.cPrivateFastFont = cPrivateFastFont;
				this.forecolor = forecolor;
				this.backcolor = backcolor;
			}

			private bool Equals(TitleTextureKey other)
			{
				return string.Equals(str文字, other.str文字) &&
					   cPrivateFastFont.Equals(other.cPrivateFastFont) &&
					   forecolor.Equals(other.forecolor) &&
					   backcolor.Equals(other.backcolor);
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				return obj is TitleTextureKey other && Equals(other);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					var hashCode = str文字.GetHashCode();
					hashCode = (hashCode * 397) ^ cPrivateFastFont.GetHashCode();
					hashCode = (hashCode * 397) ^ forecolor.GetHashCode();
					hashCode = (hashCode * 397) ^ backcolor.GetHashCode();
					return hashCode;
				}
			}
		}
		private CTexture don;
		private CTexture ka;
		private CTexture[] moji = new CTexture[4];
		private const int Width = 100;
		private const int Height = 100;
		private const int y = 550;
		private const int mojiy = 450;
		private const int fontsize = 20;

		private const int Sabunn = 16;
		#endregion
	}
}
