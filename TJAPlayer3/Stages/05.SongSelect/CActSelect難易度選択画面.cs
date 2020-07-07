using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Drawing.Text;
using FDK;
using SampleFramework;
using System.Reflection;

namespace TJAPlayer3
{
	/// <summary>
	/// 難易度選択画面。
	/// この難易度選択画面はAC7～AC14のような方式であり、WiiまたはAC15移行の方式とは異なる。
	/// </summary>
	internal class CActSelect難易度選択画面 : CActivity
	{
		// プロパティ

		// CActivity 実装

		public override void On活性化()
		{
			if( this.b活性化してる )
				return;
			try
			{
				this.ct分岐表示用タイマー = new CCounter(1, 2, 2500, TJAPlayer3.Timer);
				選択済み = new bool[2] { false, false };
				裏カウント = new int[2] { 0, 0 };
				this.nスクロールタイマ = -1;
			}
			finally { 
			
			}

			base.On活性化();
		}
		public override void On非活性化()
		{
			if( this.b活性化してない )
				return;

			try
			{
				this.ct分岐表示用タイマー = null;
			}
			finally { 
			}

			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( this.b活性化してない )
				return;

			base.OnManagedリソースの作成();
		}
		public override void OnManagedリソースの解放()
		{
			if( this.b活性化してない )
				return;

			base.OnManagedリソースの解放();
		}
		public override int On進行描画()
		{
			if (this.b活性化してない)
				return 0;

			#region [ 初めての進行描画 ]
			//-----------------
			if (this.b初めての進行描画)
			{
				this.nスクロールタイマ = (long)(CSound管理.rc演奏用タイマ.n現在時刻 * (((double)TJAPlayer3.ConfigIni.n演奏速度) / 20.0));
			}
			//-----------------
			#endregion

			this.ct分岐表示用タイマー.t進行Loop();

			// 描画。


			#region[難易度マーク]

			for (int i = 0; i < TJAPlayer3.ConfigIni.nPlayerCount; i++)
			{
				if (選択済み[i])
				{
					if (TJAPlayer3.Tx.Difficulty_Mark[確定された難易度[i]] != null)
					{
						TJAPlayer3.Tx.Difficulty_Mark[確定された難易度[i]].Opacity = 100;
						TJAPlayer3.Tx.Difficulty_Mark[確定された難易度[i]].vc拡大縮小倍率 = new SharpDX.Vector3(0.75f);
						TJAPlayer3.Tx.Difficulty_Mark[確定された難易度[i]].t2D描画(TJAPlayer3.app.Device, i * 1075 - 30, 300);
					}
				}else if (現在の選択行[i] >= 3)
                {
					if (裏表示 && 現在の選択行[i] - 3 == 3)
					{
						if (TJAPlayer3.Tx.Difficulty_Mark[4] != null)
						{
							TJAPlayer3.Tx.Difficulty_Mark[4].Opacity = 100;
							TJAPlayer3.Tx.Difficulty_Mark[4].vc拡大縮小倍率 = new SharpDX.Vector3(0.75f);
							TJAPlayer3.Tx.Difficulty_Mark[4].t2D描画(TJAPlayer3.app.Device, i * 1075 - 30, 300);
						}
					}
					else
					{
						if (TJAPlayer3.Tx.Difficulty_Mark[現在の選択行[i] - 3] != null)
						{
							TJAPlayer3.Tx.Difficulty_Mark[現在の選択行[i] - 3].Opacity = 100;
							TJAPlayer3.Tx.Difficulty_Mark[現在の選択行[i] - 3].vc拡大縮小倍率 = new SharpDX.Vector3(0.75f);
							TJAPlayer3.Tx.Difficulty_Mark[現在の選択行[i] - 3].t2D描画(TJAPlayer3.app.Device, i * 1075 - 30, 300);
						}
					}
				}
			}
			#endregion
			#region[難易度選択裏バー描画]
				if (TJAPlayer3.Tx.Difficulty_Center_Bar != null)
			{
				int width = TJAPlayer3.Skin.Difficulty_Bar_Center_X_WH_WH_Y_Y[3];
				int height = TJAPlayer3.Skin.Difficulty_Bar_Center_X_WH_WH_Y_Y[4];

				int xdiff = TJAPlayer3.Skin.Difficulty_Bar_Center_X_WH_WH_Y_Y[0] - TJAPlayer3.Skin.Difficulty_Bar_Center_X_WH_WH_Y_Y[3] / 2;
				int ydiff = TJAPlayer3.Skin.Difficulty_Bar_Center_X_WH_WH_Y_Y[6];
				for (int i = 0; i < width / 20 + 1; i++)
				{
					for (int j = 0; j < height / 20 + 1; j++)
					{
						if (i == 0 && j == 0)
						{
							TJAPlayer3.Tx.Difficulty_Center_Bar.t2D描画(TJAPlayer3.app.Device, i * 20 + xdiff, j * 20 + ydiff, new Rectangle(0, 0, 20, 20));
						}
						else if (i == 0 && j == (height / 20))
						{
							TJAPlayer3.Tx.Difficulty_Center_Bar.t2D描画(TJAPlayer3.app.Device, i * 20 + xdiff, j * 20 - (20 - height % 20) + ydiff, new Rectangle(0, 40, 20, 20));
						}
						else if (i == (width / 20) && j == 0)
						{
							TJAPlayer3.Tx.Difficulty_Center_Bar.t2D描画(TJAPlayer3.app.Device, i * 20 - (20 - width % 20) + xdiff, j * 20 + ydiff, new Rectangle(40, 0, 20, 20));
						}
						else if (i == (width / 20) && j == (height / 20))
						{
							TJAPlayer3.Tx.Difficulty_Center_Bar.t2D描画(TJAPlayer3.app.Device, i * 20 - (20 - width % 20) + xdiff, j * 20 - (20 - height % 20) + ydiff, new Rectangle(40, 40, 20, 20));
						}
						else if (i == 0)
						{
							TJAPlayer3.Tx.Difficulty_Center_Bar.t2D描画(TJAPlayer3.app.Device, i * 20 + xdiff, j * 20 + ydiff, new Rectangle(0, 20, 20, 20));
						}
						else if (j == 0)
						{
							TJAPlayer3.Tx.Difficulty_Center_Bar.t2D描画(TJAPlayer3.app.Device, i * 20 + xdiff, j * 20 + ydiff, new Rectangle(20, 0, 20, 20));
						}
						else if (i == (width / 20))
						{
							TJAPlayer3.Tx.Difficulty_Center_Bar.t2D描画(TJAPlayer3.app.Device, i * 20 - (20 - width % 20) + xdiff, j * 20 + ydiff, new Rectangle(40, 20, 20, 20));
						}
						else if (j == (height / 20))
						{
							TJAPlayer3.Tx.Difficulty_Center_Bar.t2D描画(TJAPlayer3.app.Device, i * 20 + xdiff, j * 20 - (20 - height % 20) + ydiff, new Rectangle(20, 40, 20, 20));
						}
						else
						{
							TJAPlayer3.Tx.Difficulty_Center_Bar.t2D描画(TJAPlayer3.app.Device, i * 20 + xdiff, j * 20 + ydiff, new Rectangle(20, 20, 20, 20));
						}
					}
				}
			}
			#endregion
			#region[タイトル文字列]
			int xAnime = 200;
			int yAnime = 30;

			if (TJAPlayer3.stage選曲.act曲リスト.ttk選択している曲のサブタイトル != null)
			{
				int nサブタイY = (int)(TJAPlayer3.Skin.SongSelect_Overall_Y + 440 - (TJAPlayer3.stage選曲.act曲リスト.サブタイトルtmp.sz画像サイズ.Height * TJAPlayer3.stage選曲.act曲リスト.サブタイトルtmp.vc拡大縮小倍率.Y));
				TJAPlayer3.stage選曲.act曲リスト.サブタイトルtmp.t2D描画(TJAPlayer3.app.Device, 707 + xAnime, nサブタイY - yAnime);
				if (TJAPlayer3.stage選曲.act曲リスト.ttk選択している曲の曲名 != null)
				{
					TJAPlayer3.stage選曲.act曲リスト.タイトルtmp.t2D描画(TJAPlayer3.app.Device, 750 + xAnime, TJAPlayer3.Skin.SongSelect_Overall_Y + 23 - yAnime);
				}
			}
			else if (TJAPlayer3.stage選曲.act曲リスト.ttk選択している曲の曲名 != null)
			{
				TJAPlayer3.stage選曲.act曲リスト.タイトルtmp.t2D描画(TJAPlayer3.app.Device, 750 + xAnime, TJAPlayer3.Skin.SongSelect_Overall_Y + 23 - yAnime);
			}
			#endregion
			#region[バーテクスチャ]
			for (int i = 0; i < 3; i++)
			{
				if (TJAPlayer3.Tx.Difficulty_Bar_Etc[i] != null)
					TJAPlayer3.Tx.Difficulty_Bar_Etc[i].t2D描画(TJAPlayer3.app.Device, i * 75 + 225, 150);
			}

			for (int i = 0; i < 4; i++)
			{
				if (裏表示 && i == 3)
				{
					if (TJAPlayer3.Tx.Difficulty_Bar[4] != null)
					{
						if (TJAPlayer3.stage選曲.act曲リスト.r現在選択中のスコア.譜面情報.b譜面が存在する[4])
							TJAPlayer3.Tx.Difficulty_Bar[4].color4 = new SharpDX.Color4(1f, 1f, 1f, 1f);
						else
							TJAPlayer3.Tx.Difficulty_Bar[4].color4 = new SharpDX.Color4(0.5f, 0.5f, 0.5f, 1f);
						if (TJAPlayer3.Tx.Difficulty_Bar[4] != null)
							TJAPlayer3.Tx.Difficulty_Bar[4].t2D描画(TJAPlayer3.app.Device, i * 100 + 440, 90);
					}
				}
				else
				{
					if (TJAPlayer3.Tx.Difficulty_Bar[i] != null)
					{
						if (TJAPlayer3.stage選曲.act曲リスト.r現在選択中のスコア.譜面情報.b譜面が存在する[i])
							TJAPlayer3.Tx.Difficulty_Bar[i].color4 = new SharpDX.Color4(1f, 1f, 1f, 1f);
						else
							TJAPlayer3.Tx.Difficulty_Bar[i].color4 = new SharpDX.Color4(0.5f, 0.5f, 0.5f, 1f);
						if (TJAPlayer3.Tx.Difficulty_Bar[i] != null)
							TJAPlayer3.Tx.Difficulty_Bar[i].t2D描画(TJAPlayer3.app.Device, i * 100 + 440, 90);
					}
				}
			}
            #endregion
            #region[星]
            if (TJAPlayer3.Tx.Difficulty_Star != null)//Difficulty_Starがないなら、通す必要なし！
			{
				for (int i = 0; i < 4; i++)
				{
					if (裏表示 && i == 3)
					{
						for (int j = 0; j < TJAPlayer3.stage選曲.act曲リスト.r現在選択中のスコア.譜面情報.nレベル[4]; j++)
						{
							TJAPlayer3.Tx.Difficulty_Star.t2D描画(TJAPlayer3.app.Device, i * 100 + 475, 483 - (j * 20), new Rectangle(TJAPlayer3.Tx.Difficulty_Star.szテクスチャサイズ.Width / 2, 0, TJAPlayer3.Tx.Difficulty_Star.szテクスチャサイズ.Width / 2, TJAPlayer3.Tx.Difficulty_Star.szテクスチャサイズ.Height));
						}
					}
					else
					{
						for (int j = 0; j < TJAPlayer3.stage選曲.act曲リスト.r現在選択中のスコア.譜面情報.nレベル[i]; j++)
						{
							TJAPlayer3.Tx.Difficulty_Star.t2D描画(TJAPlayer3.app.Device, i * 100 + 475, 483 - (j * 20), new Rectangle(0, 0, TJAPlayer3.Tx.Difficulty_Star.szテクスチャサイズ.Width / 2, TJAPlayer3.Tx.Difficulty_Star.szテクスチャサイズ.Height));
						}
					}
				}
			}
			#endregion
			#region[譜面分岐]
			if (TJAPlayer3.Tx.Difficulty_Branch != null)//Difficulty_Branchがないなら、通す必要なし！
			{
				TJAPlayer3.Tx.Difficulty_Branch.Opacity = (int)((ct分岐表示用タイマー.n現在の値 % 2) * 255.0);
				for (int i = 0; i < 4; i++)
				{
					if (裏表示 && i == 3)
					{
						if(TJAPlayer3.stage選曲.act曲リスト.r現在選択中のスコア.譜面情報.b譜面分岐[4])
							TJAPlayer3.Tx.Difficulty_Branch.t2D描画(TJAPlayer3.app.Device, i * 100 + 470, 310, new Rectangle(TJAPlayer3.Tx.Difficulty_Branch.szテクスチャサイズ.Width / 2, 0, TJAPlayer3.Tx.Difficulty_Branch.szテクスチャサイズ.Width / 2, TJAPlayer3.Tx.Difficulty_Branch.szテクスチャサイズ.Height));
					}
					else
					{
						if(TJAPlayer3.stage選曲.act曲リスト.r現在選択中のスコア.譜面情報.b譜面分岐[i])
							TJAPlayer3.Tx.Difficulty_Branch.t2D描画(TJAPlayer3.app.Device, i * 100 + 470, 310, new Rectangle(0, 0, TJAPlayer3.Tx.Difficulty_Branch.szテクスチャサイズ.Width / 2, TJAPlayer3.Tx.Difficulty_Branch.szテクスチャサイズ.Height));
					}
				}
			}
			#endregion
			#region[王冠]
			if (TJAPlayer3.Tx.Crown_t != null)//王冠テクスチャがないなら、通す必要なし！
			{
				TJAPlayer3.Tx.Crown_t.Opacity = 255;
				TJAPlayer3.Tx.Crown_t.vc拡大縮小倍率 = new SharpDX.Vector3(0.35f);
				for (int i = 0; i < 4; i++)
				{
					if (裏表示 && i == 3)
					{
						if(TJAPlayer3.stage選曲.act曲リスト.r現在選択中のスコア.譜面情報.b譜面が存在する[4])
							TJAPlayer3.Tx.Crown_t.t2D描画(TJAPlayer3.app.Device, i * 100 + 474, 60, new Rectangle(TJAPlayer3.stage選曲.act曲リスト.r現在選択中のスコア.譜面情報.n王冠[4] * 100, 0, 100, 100));
					}
					else
					{
						if (TJAPlayer3.stage選曲.act曲リスト.r現在選択中のスコア.譜面情報.b譜面が存在する[i])
							TJAPlayer3.Tx.Crown_t.t2D描画(TJAPlayer3.app.Device, i * 100 + 474, 60, new Rectangle(TJAPlayer3.stage選曲.act曲リスト.r現在選択中のスコア.譜面情報.n王冠[i] * 100, 0, 100, 100));
					}
				}
			}
			#endregion
			#region[プレイヤーアンカー]
			for (int i = 0; i < TJAPlayer3.ConfigIni.nPlayerCount; i++) {
				if (TJAPlayer3.ConfigIni.nPlayerCount >= 2 && 現在の選択行[0] == 現在の選択行[1] && !選択済み[0] && !選択済み[1]) {
					if (現在の選択行[i] < 3)
					{
						if (TJAPlayer3.Tx.Difficulty_Anc_Box_Etc[i] != null)
							TJAPlayer3.Tx.Difficulty_Anc_Box_Etc[i].t2D描画(TJAPlayer3.app.Device, 現在の選択行[i] * 75 + 210 + i * TJAPlayer3.Tx.Difficulty_Anc_Box_Etc[i].szテクスチャサイズ.Width / 2, 105, new Rectangle(i * TJAPlayer3.Tx.Difficulty_Anc_Box_Etc[i].szテクスチャサイズ.Width / 2, 0, TJAPlayer3.Tx.Difficulty_Anc_Box_Etc[i].szテクスチャサイズ.Width / 2, TJAPlayer3.Tx.Difficulty_Anc_Box_Etc[i].szテクスチャサイズ.Height)) ;

						if (TJAPlayer3.Tx.Difficulty_Anc_Same[i] != null)
							TJAPlayer3.Tx.Difficulty_Anc_Same[i].t2D描画(TJAPlayer3.app.Device, 現在の選択行[i] * 75 + 210 + (int)(TJAPlayer3.Tx.Difficulty_Anc_Same[i].szテクスチャサイズ.Width * (i - 0.5)), 0);
					}
					else
					{
						if (TJAPlayer3.Tx.Difficulty_Anc_Box[i] != null)
							TJAPlayer3.Tx.Difficulty_Anc_Box[i].t2D描画(TJAPlayer3.app.Device, (現在の選択行[i] - 3) * 100 + 441 + i * TJAPlayer3.Tx.Difficulty_Anc_Box[i].szテクスチャサイズ.Width / 2, 138, new Rectangle(i * TJAPlayer3.Tx.Difficulty_Anc_Box[i].szテクスチャサイズ.Width / 2, 0, TJAPlayer3.Tx.Difficulty_Anc_Box[i].szテクスチャサイズ.Width / 2, TJAPlayer3.Tx.Difficulty_Anc_Box[i].szテクスチャサイズ.Height));

						if (TJAPlayer3.Tx.Difficulty_Anc_Same[i] != null)
							TJAPlayer3.Tx.Difficulty_Anc_Same[i].t2D描画(TJAPlayer3.app.Device, (現在の選択行[i] - 3) * 100 + 441 + (int)(TJAPlayer3.Tx.Difficulty_Anc_Same[i].szテクスチャサイズ.Width * (i - 0.5)), -10);
					}
				}
				else
				{
					if (!選択済み[i])
					{
						if (現在の選択行[i] < 3)
						{
							if (TJAPlayer3.Tx.Difficulty_Anc_Box_Etc[i] != null)
								TJAPlayer3.Tx.Difficulty_Anc_Box_Etc[i].t2D描画(TJAPlayer3.app.Device, 現在の選択行[i] * 75 + 210, 105);

							if (TJAPlayer3.Tx.Difficulty_Anc[i] != null)
								TJAPlayer3.Tx.Difficulty_Anc[i].t2D描画(TJAPlayer3.app.Device, 現在の選択行[i] * 75 + 210, 0);
						}
						else
						{
							if (TJAPlayer3.Tx.Difficulty_Anc_Box[i] != null)
								TJAPlayer3.Tx.Difficulty_Anc_Box[i].t2D描画(TJAPlayer3.app.Device, (現在の選択行[i] - 3) * 100 + 441, 138);

							if (TJAPlayer3.Tx.Difficulty_Anc[i] != null)
								TJAPlayer3.Tx.Difficulty_Anc[i].t2D描画(TJAPlayer3.app.Device, (現在の選択行[i] - 3) * 100 + 441, -10);
						}
					}
				}
			}
            #endregion

            //裏鬼表示用
            if ((裏カウント[0] >= 10 || 裏カウント[1] >= 10) && TJAPlayer3.stage選曲.act曲リスト.r現在選択中のスコア.譜面情報.b譜面が存在する[4]) {
				裏表示 = !裏表示;
				裏カウント[0] = 0;
				裏カウント[1] = 0;
				if(裏表示)
					TJAPlayer3.stage選曲.act曲リスト.n現在のアンカ難易度レベル[0] = 4;
				else
					TJAPlayer3.stage選曲.act曲リスト.n現在のアンカ難易度レベル[0] = 3;
			}

			return 0;
		}


		

		// その他

		#region [ private ]
		//-----------------
		internal int[] 現在の選択行 = new int[2] { TJAPlayer3.ConfigIni.nDefaultCourse + 3, TJAPlayer3.ConfigIni.nDefaultCourse + 3 };
		internal bool[] 選択済み = new bool[2];
		internal int[] 確定された難易度 = new int[2];
		internal int[] 裏カウント = new int[2];
		internal bool 裏表示 = false;
		private long nスクロールタイマ;
		private CCounter ct分岐表示用タイマー;
		//-----------------
		#endregion
	}
}
