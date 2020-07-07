using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using FDK;
using System.Reflection;

namespace TJAPlayer3
{
	internal class CStageタイトル : CStage
	{
		// コンストラクタ

		public CStageタイトル()
		{
			base.eステージID = CStage.Eステージ.タイトル;
			base.b活性化してない = true;
			base.list子Activities.Add(this.actFIfromSetup = new CActFIFOBlack());
			base.list子Activities.Add(this.actFI = new CActFIFOBlack());
			base.list子Activities.Add(this.actFO = new CActFIFOBlack());
		}


		// CStage 実装

		public override void On活性化()
		{
			Trace.TraceInformation("タイトルステージを活性化します。");
			Trace.Indent();
			try
			{
				for (int i = 0; i < 4; i++)
				{
					this.ctキー反復用[i] = new CCounter(0, 0, 0, TJAPlayer3.Timer);
				}
				this.ct上移動用 = new CCounter();
				this.ct下移動用 = new CCounter();

				base.On活性化();
				Discord.UpdatePresence("", "Title", TJAPlayer3.StartupTime);
			}
			finally
			{
				Trace.TraceInformation("タイトルステージの活性化を完了しました。");
				Trace.Unindent();
			}
		}
		public override void On非活性化()
		{
			Trace.TraceInformation("タイトルステージを非活性化します。");
			Trace.Indent();
			try
			{
				for (int i = 0; i < 4; i++)
				{
					this.ctキー反復用[i] = null;
				}
				this.ct上移動用 = null;
				this.ct下移動用 = null;
			}
			finally
			{
				Trace.TraceInformation("タイトルステージの非活性化を完了しました。");
				Trace.Unindent();
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない) { 
				texttexture[0] = this.GenerateTitleTexture(this.ttk曲名テクスチャを生成する("演奏ゲーム", Color.White, Color.SaddleBrown));
				texttexture[1] = this.GenerateTitleTexture(this.ttk曲名テクスチャを生成する("コンフィグ", Color.White, Color.SaddleBrown));
				texttexture[2] = this.GenerateTitleTexture(this.ttk曲名テクスチャを生成する("やめる", Color.White, Color.SaddleBrown));
				texttexture[3] = this.GenerateTitleTexture(this.ttk曲名テクスチャを生成する("演奏ゲーム", Color.White, Color.Black));
				texttexture[4] = this.GenerateTitleTexture(this.ttk曲名テクスチャを生成する("コンフィグ", Color.White, Color.Black));
				texttexture[5] = this.GenerateTitleTexture(this.ttk曲名テクスチャを生成する("やめる", Color.White, Color.Black));
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				for (int i = 0; i < texttexture.Length; i++)
				{
					TJAPlayer3.tテクスチャの解放(ref texttexture[i]);
				}
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if (!base.b活性化してない)
			{
				#region [ 初めての進行描画 ]
				//---------------------
				if (base.b初めての進行描画)
				{
					if (TJAPlayer3.r直前のステージ == TJAPlayer3.stage起動)
					{
						this.actFIfromSetup.tフェードイン開始();
						base.eフェーズID = CStage.Eフェーズ.タイトル_起動画面からのフェードイン;
					}
					else
					{
						this.actFI.tフェードイン開始();
						base.eフェーズID = CStage.Eフェーズ.共通_フェードイン;
					}
					base.b初めての進行描画 = false;
				}
				//---------------------
				#endregion

				// 進行

				#region [ カーソル上移動 ]
				//---------------------
				if (this.ct上移動用.b進行中)
				{
					this.ct上移動用.t進行();
					if (this.ct上移動用.b終了値に達した)
					{
						this.ct上移動用.t停止();
					}
				}
				//---------------------
				#endregion
				#region [ カーソル下移動 ]
				//---------------------
				if (this.ct下移動用.b進行中)
				{
					this.ct下移動用.t進行();
					if (this.ct下移動用.b終了値に達した)
					{
						this.ct下移動用.t停止();
					}
				}
				//---------------------
				#endregion

				// キー入力

				if (base.eフェーズID == CStage.Eフェーズ.共通_通常状態)        // 通常状態
				{
					if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDXKeys.Key.Escape))
						return (int)E戻り値.EXIT;

					this.ctキー反復用.Up.tキー反復(TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDXKeys.Key.UpArrow) || TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDXKeys.Key.LeftArrow) || TJAPlayer3.Pad.b押されている(E楽器パート.DRUMS, Eパッド.LBlue) || TJAPlayer3.Pad.b押されている(E楽器パート.DRUMS, Eパッド.LBlue2P) && TJAPlayer3.ConfigIni.nPlayerCount >= 2, new CCounter.DGキー処理(this.tカーソルを上へ移動する));
					this.ctキー反復用.R.tキー反復(TJAPlayer3.Pad.b押されているGB(Eパッド.HH), new CCounter.DGキー処理(this.tカーソルを上へ移動する));
					if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.SD))
						this.tカーソルを上へ移動する();

					this.ctキー反復用.Down.tキー反復(TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDXKeys.Key.DownArrow) || TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDXKeys.Key.RightArrow) || TJAPlayer3.Pad.b押されている(E楽器パート.DRUMS, Eパッド.RBlue) || TJAPlayer3.Pad.b押されている(E楽器パート.DRUMS, Eパッド.RBlue2P) && TJAPlayer3.ConfigIni.nPlayerCount >= 2, new CCounter.DGキー処理(this.tカーソルを下へ移動する));
					this.ctキー反復用.B.tキー反復(TJAPlayer3.Pad.b押されているGB(Eパッド.BD), new CCounter.DGキー処理(this.tカーソルを下へ移動する));
					if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LT))
						this.tカーソルを下へ移動する();

					if ((TJAPlayer3.Pad.b押されたDGB(Eパッド.CY) || TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RD)) || (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LC) || (TJAPlayer3.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDXKeys.Key.Return)) || TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LRed) || TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RRed) || (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LRed2P) || TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RRed2P)) && TJAPlayer3.ConfigIni.nPlayerCount >= 2))
					{
						if ((this.n現在のカーソル行 == (int)E戻り値.GAMESTART - 1) && TJAPlayer3.Skin.soundゲーム開始音.b読み込み成功)
						{
							if (!((TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDXKeys.Key.LeftControl) || TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDXKeys.Key.RightControl)) && TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDXKeys.Key.A)))
								TJAPlayer3.Skin.soundゲーム開始音.t再生する();
						}
						else
						{
							TJAPlayer3.Skin.sound決定音.t再生する();
						}
						if (this.n現在のカーソル行 == (int)E戻り値.EXIT - 1)
						{
							return (int)E戻り値.EXIT;
						}
						this.actFO.tフェードアウト開始();
						base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
					}
					//					if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) Key.Space ) )
					//						Trace.TraceInformation( "DTXMania Title: SPACE key registered. " + CDTXMania.ct.nシステム時刻 );
				}

				// 描画

				if (TJAPlayer3.Tx.Title_Background != null)
					TJAPlayer3.Tx.Title_Background.t2D描画(TJAPlayer3.app.Device, 0, 0);

				#region[ バージョン表示 ]
				//string strVersion = "KTT:J:A:I:2017072200";
				string strCreator = "https://github.com/Mr-Ojii/TJAPlayer3";
				AssemblyName asmApp = Assembly.GetExecutingAssembly().GetName();
#if DEBUG
				TJAPlayer3.act文字コンソール.tPrint(4, 44, C文字コンソール.Eフォント種別.白, "DEBUG BUILD?");
#endif
				TJAPlayer3.act文字コンソール.tPrint(4, 4, C文字コンソール.Eフォント種別.白, asmApp.Name + " Ver." + TJAPlayer3.VERSION + " (" + strCreator + ")");
				TJAPlayer3.act文字コンソール.tPrint(4, 24, C文字コンソール.Eフォント種別.白, "Skin:" + TJAPlayer3.Skin.Skin_Name + " Ver." + TJAPlayer3.Skin.Skin_Version + " (" + TJAPlayer3.Skin.Skin_Creator + ")");
				//CDTXMania.act文字コンソール.tPrint(4, 24, C文字コンソール.Eフォント種別.白, strSubTitle);
				TJAPlayer3.act文字コンソール.tPrint(4, (720 - 24), C文字コンソール.Eフォント種別.白, "TJAPlayer3-f forked TJAPlayer3(AioiLight) forked TJAPlayer2 forPC(kairera0467)");
				#endregion


				if (TJAPlayer3.Tx.Title_InBar != null && TJAPlayer3.Tx.Title_AcBar != null)
				{
					for (int i = 0; i < 3; i++)
					{
						TJAPlayer3.Tx.Title_InBar.t2D描画(TJAPlayer3.app.Device, MENU_XT[i] - TJAPlayer3.Tx.Title_InBar.szテクスチャサイズ.Width / 2, MENU_YT);
					}


					if (this.ct下移動用.b進行中)
					{
						TJAPlayer3.Tx.Title_AcBar.vc拡大縮小倍率.X = this.ct下移動用.n現在の値 * 0.01f;
						TJAPlayer3.Tx.Title_AcBar.t2D描画(TJAPlayer3.app.Device, MENU_XT[this.n現在のカーソル行] - TJAPlayer3.Tx.Title_AcBar.sz画像サイズ.Width / 2 * this.ct下移動用.n現在の値 * 0.01f, MENU_YT);
					}
					else if (this.ct上移動用.b進行中)
					{
						TJAPlayer3.Tx.Title_AcBar.vc拡大縮小倍率.X = this.ct上移動用.n現在の値 * 0.01f;
						TJAPlayer3.Tx.Title_AcBar.t2D描画(TJAPlayer3.app.Device, MENU_XT[this.n現在のカーソル行] - TJAPlayer3.Tx.Title_AcBar.sz画像サイズ.Width / 2 * this.ct上移動用.n現在の値 * 0.01f, MENU_YT);
					}
					else
					{
						TJAPlayer3.Tx.Title_AcBar.vc拡大縮小倍率.X = 1.0f;
						TJAPlayer3.Tx.Title_AcBar.t2D描画(TJAPlayer3.app.Device, MENU_XT[this.n現在のカーソル行] - TJAPlayer3.Tx.Title_AcBar.sz画像サイズ.Width / 2, MENU_YT);
					}

					TJAPlayer3.Tx.Title_Txt[this.n現在のカーソル行] = texttexture[this.n現在のカーソル行 + 3];
					
					if (this.n現在のカーソル行 != (int)E戻り値.GAMESTART - 1)
					{
						TJAPlayer3.Tx.Title_Txt[0] = texttexture[0];
					}
					if (this.n現在のカーソル行 != (int)E戻り値.CONFIG - 1)
					{
						TJAPlayer3.Tx.Title_Txt[1] = texttexture[1];
					}
					if (this.n現在のカーソル行 != (int)E戻り値.EXIT - 1)
					{
						TJAPlayer3.Tx.Title_Txt[2] = texttexture[2];
					}

					for (int i = 0; i < 3; i++)
					{
						TJAPlayer3.Tx.Title_Txt[i].t2D描画(TJAPlayer3.app.Device, MENU_XT[i] - TJAPlayer3.Tx.Title_Txt[i].szテクスチャサイズ.Width / 2, MENU_YT + 30);

					}
				}
				else
				{
					if (TJAPlayer3.Tx.Title_Menu != null)
					{
						int x = MENU_X;
						int y = MENU_Y + (this.n現在のカーソル行 * MENU_H);
						if (this.ct上移動用.b進行中)
						{
							y += (int)((double)MENU_H / 2 * (Math.Cos(Math.PI * (((double)this.ct上移動用.n現在の値) / 100.0)) + 1.0));
						}
						else if (this.ct下移動用.b進行中)
						{
							y -= (int)((double)MENU_H / 2 * (Math.Cos(Math.PI * (((double)this.ct下移動用.n現在の値) / 100.0)) + 1.0));
						}
						TJAPlayer3.Tx.Title_Menu.vc拡大縮小倍率.X = 1f;
						TJAPlayer3.Tx.Title_Menu.vc拡大縮小倍率.Y = 1f;
						TJAPlayer3.Tx.Title_Menu.Opacity = 0xff;
						TJAPlayer3.Tx.Title_Menu.t2D描画(TJAPlayer3.app.Device, x, y, new Rectangle(0, MENU_H * 4, MENU_W, MENU_H));
					}
					if (TJAPlayer3.Tx.Title_Menu != null)
					{
						//this.txメニュー.t2D描画( CDTXMania.app.Device, 0xce, 0xcb, new Rectangle( 0, 0, MENU_W, MWNU_H ) );
						// #24525 2011.3.16 yyagi: "OPTION"を省いて描画。従来スキンとの互換性確保のため。
						TJAPlayer3.Tx.Title_Menu.t2D描画(TJAPlayer3.app.Device, MENU_X, MENU_Y, new Rectangle(0, 0, MENU_W, MENU_H));
						TJAPlayer3.Tx.Title_Menu.t2D描画(TJAPlayer3.app.Device, MENU_X, MENU_Y + MENU_H, new Rectangle(0, MENU_H * 2, MENU_W, MENU_H * 2));
					}
				}

				// URLの座標が押されたらブラウザで開いてやる 兼 マウスクリックのテスト
				// クライアント領域内のカーソル座標を取得する。
				// point.X、point.Yは負の値になることもある。
				var point = TJAPlayer3.app.Window.PointToClient(System.Windows.Forms.Cursor.Position);
				// クライアント領域の横幅を取得して、1280で割る。もちろんdouble型。
				var scaling = 1.000 * TJAPlayer3.app.Window.ClientSize.Width / 1280;
				if (TJAPlayer3.Input管理.Mouse.bキーが押された((int)SlimDXKeys.MouseObject.Button1))
				{
					if (point.X >= 180 * scaling && point.X <= 490 * scaling && point.Y >= 0 && point.Y <= 20 * scaling)
						System.Diagnostics.Process.Start(strCreator);
				}

				//CDTXMania.act文字コンソール.tPrint(0, 80, C文字コンソール.Eフォント種別.白, point.X.ToString());
				//CDTXMania.act文字コンソール.tPrint(0, 100, C文字コンソール.Eフォント種別.白, point.Y.ToString());
				//CDTXMania.act文字コンソール.tPrint(0, 120, C文字コンソール.Eフォント種別.白, scaling.ToString());


				CStage.Eフェーズ eフェーズid = base.eフェーズID;
				switch (eフェーズid)
				{
					case CStage.Eフェーズ.共通_フェードイン:
						if (this.actFI.On進行描画() != 0)
						{
							TJAPlayer3.Skin.soundタイトル音.t再生する();
							base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
						}
						break;

					case CStage.Eフェーズ.共通_フェードアウト:
						if (this.actFO.On進行描画() == 0)
						{
							break;
						}
						base.eフェーズID = CStage.Eフェーズ.共通_終了状態;
						switch (this.n現在のカーソル行)
						{
							case (int)E戻り値.GAMESTART - 1:
								if (!((TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDXKeys.Key.LeftControl) || TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDXKeys.Key.RightControl)) && TJAPlayer3.Input管理.Keyboard.bキーが押されている((int)SlimDXKeys.Key.A)))
									return (int)E戻り値.GAMESTART;
								else
									return (int)E戻り値.MAINTENANCE;

							case (int)E戻り値.CONFIG - 1:
								return (int)E戻り値.CONFIG;

							case (int)E戻り値.EXIT - 1:
								return (int)E戻り値.EXIT;
								//return ( this.n現在のカーソル行 + 1 );
						}
						break;

					case CStage.Eフェーズ.タイトル_起動画面からのフェードイン:
						if (this.actFIfromSetup.On進行描画() != 0)
						{
							TJAPlayer3.Skin.soundタイトル音.t再生する();
							base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
						}
						break;
				}
			}
			return 0;
		}
		public enum E戻り値
		{
			継続 = 0,
			GAMESTART,
			//			OPTION,
			CONFIG,
			EXIT,
			MAINTENANCE
		}


		// その他

		#region [ private ]
		//-----------------
		[StructLayout(LayoutKind.Sequential)]
		private struct STキー反復用カウンタ
		{
			public CCounter Up;
			public CCounter Down;
			public CCounter R;
			public CCounter B;
			public CCounter this[int index]
			{
				get
				{
					switch (index)
					{
						case 0:
							return this.Up;

						case 1:
							return this.Down;

						case 2:
							return this.R;

						case 3:
							return this.B;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch (index)
					{
						case 0:
							this.Up = value;
							return;

						case 1:
							this.Down = value;
							return;

						case 2:
							this.R = value;
							return;

						case 3:
							this.B = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}

		private CTexture GenerateTitleTexture(TitleTextureKey titleTextureKey)
		{
			using (var bmp = new Bitmap(titleTextureKey.cPrivateFastFont.DrawPrivateFont(
				titleTextureKey.str文字, titleTextureKey.forecolor, titleTextureKey.backcolor, true)))
			{
				CTexture tx文字テクスチャ = TJAPlayer3.tテクスチャの生成(bmp, false);
				if (tx文字テクスチャ.szテクスチャサイズ.Height > titleTextureKey.maxHeight)
				{
					tx文字テクスチャ.vc拡大縮小倍率.Y = (float)(((double)titleTextureKey.maxHeight) / tx文字テクスチャ.szテクスチャサイズ.Height);
				}

				return tx文字テクスチャ;
			}
		}
		


		private sealed class TitleTextureKey
		{
			public readonly string str文字;
			public readonly CPrivateFastFont cPrivateFastFont;
			public readonly Color forecolor;
			public readonly Color backcolor;
			public readonly int maxHeight;

			public TitleTextureKey(string str文字, CPrivateFastFont cPrivateFastFont, Color forecolor, Color backcolor, int maxHeight)
			{
				this.str文字 = str文字;
				this.cPrivateFastFont = cPrivateFastFont;
				this.forecolor = forecolor;
				this.backcolor = backcolor;
				this.maxHeight = maxHeight;
			}

			private bool Equals(TitleTextureKey other)
			{
				return string.Equals(str文字, other.str文字) &&
					   cPrivateFastFont.Equals(other.cPrivateFastFont) &&
					   forecolor.Equals(other.forecolor) &&
					   backcolor.Equals(other.backcolor) &&
					   maxHeight == other.maxHeight;
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
					hashCode = (hashCode * 397) ^ maxHeight;
					return hashCode;
				}
			}

			public static bool operator ==(TitleTextureKey left, TitleTextureKey right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(TitleTextureKey left, TitleTextureKey right)
			{
				return !Equals(left, right);
			}
		}

		private TitleTextureKey ttk曲名テクスチャを生成する(string str文字, Color forecolor, Color backcolor) {
			return new TitleTextureKey(str文字, new CPrivateFastFont(new FontFamily(TJAPlayer3.ConfigIni.FontName), 28), forecolor, backcolor, 410);
		}

		CTexture[] texttexture = new CTexture[6];
		private CActFIFOBlack actFI;
		private CActFIFOBlack actFIfromSetup;
		private CActFIFOBlack actFO;
		private STキー反復用カウンタ ctキー反復用;
		private CCounter ct下移動用;
		private CCounter ct上移動用;
		private const int MENU_H = 39;
		private const int MENU_W = 227;
		private const int MENU_X = 506;
		private const int MENU_Y = 513;
		//縦スタイル用
		private readonly int[] MENU_XT = {300,640,980 };
		private const int MENU_YT = 100;
		//------------------------------------
		private int n現在のカーソル行;
	
		private void tカーソルを下へ移動する()
		{
			if ( this.n現在のカーソル行 != (int) E戻り値.EXIT - 1 )
			{
				TJAPlayer3.Skin.soundカーソル移動音.t再生する();
				this.n現在のカーソル行++;
				this.ct下移動用.t開始( 0, 100, 1, TJAPlayer3.Timer );
				if( this.ct上移動用.b進行中 )
				{
					this.ct下移動用.n現在の値 = 100 - this.ct上移動用.n現在の値;
					this.ct上移動用.t停止();
				}
			}
		}
		private void tカーソルを上へ移動する()
		{
			if ( this.n現在のカーソル行 != (int) E戻り値.GAMESTART - 1 )
			{
				TJAPlayer3.Skin.soundカーソル移動音.t再生する();
				this.n現在のカーソル行--;
				this.ct上移動用.t開始( 0, 100, 1, TJAPlayer3.Timer );
				if( this.ct下移動用.b進行中 )
				{
					this.ct上移動用.n現在の値 = 100 - this.ct下移動用.n現在の値;
					this.ct下移動用.t停止();
				}
			}
		}
		//-----------------
		#endregion
	}
}
