using System;
using SlimDX.DirectInput;
using FDK;
using System.Diagnostics;

namespace TJAPlayer3
{
    internal class CAct特訓モード : CActivity
    {
        public CAct特訓モード()
        {
            base.b活性化してない = true;
        }

        public override void On活性化()
        {
            this.n現在の小節線 = 0;
            this.b特訓PAUSE = false;

            base.On活性化();

            CDTX dTX = TJAPlayer3.DTX;

            var measureCount = 1;

            for (int i = 0; i < dTX.listChip.Count; i++)
            {
                CDTX.CChip pChip = dTX.listChip[i];

                if (pChip.n整数値_内部番号 > measureCount) measureCount = pChip.n整数値_内部番号;
            }

            this.n小節の総数 = measureCount;

            Trace.TraceInformation("TOKKUN: total measures->" + this.n小節の総数);
        }

        public override void On非活性化()
        {
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
                base.OnManagedリソースの作成();
            }
        }

        public override void OnManagedリソースの解放()
        {
            if (!base.b活性化してない)
            {
                base.OnManagedリソースの解放();
            }
        }

        public override int On進行描画()
        {
            if (!base.b活性化してない)
            {
                if (base.b初めての進行描画)
                {
                    base.b初めての進行描画 = false;
                }

                TJAPlayer3.act文字コンソール.tPrint(0, 0, C文字コンソール.Eフォント種別.白, "TRAINING MODE (BETA)");

                TJAPlayer3.act文字コンソール.tPrint(256, 360, C文字コンソール.Eフォント種別.白, TJAPlayer3.stage演奏ドラム画面.actPlayInfo.NowMeasure[0] + "/" + this.n小節の総数);

                if (this.b特訓PAUSE)//2020/06/28 Akane 特訓の操作と処理を一部改善
                {
                    if (TJAPlayer3.Pad.b押されたDGB(Eパッド.LRed)|| TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.Space))
                    {
                       TJAPlayer3.Skin.sound特訓再生音.t再生する();
                        this.t演奏を再開する();                      
                    }
                    if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.LeftArrow) || TJAPlayer3.Pad.b押されたDGB(Eパッド.LBlue))
                    {
                        
                        
                            if (this.n現在の小節線 > 1)
                            {
                                this.n現在の小節線--;
                                TJAPlayer3.stage演奏ドラム画面.actPlayInfo.NowMeasure[0] = this.n現在の小節線;

                                this.t譜面の表示位置を合わせる(true);
                                TJAPlayer3.Skin.sound特訓スクロール音.t再生する();
                            }
                        
                    }
                    if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.RightArrow) || TJAPlayer3.Pad.b押されたDGB(Eパッド.RBlue))
                    {
                        
                            if (this.n現在の小節線 < this.n小節の総数)
                            {
                                this.n現在の小節線++;
                                TJAPlayer3.stage演奏ドラム画面.actPlayInfo.NowMeasure[0] = this.n現在の小節線;

                                this.t譜面の表示位置を合わせる(true);
                                TJAPlayer3.Skin.sound特訓スクロール音.t再生する();
                            }
                        
                    }


                }
                else
                {
                    if (TJAPlayer3.Pad.b押されたDGB(Eパッド.LRed2P) || TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.D8)|| TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.Space))
                    {
                        
                        TJAPlayer3.Skin.sound特訓停止音.t再生する();
                        this.t演奏を停止する();
                        
                    }
                    if (TJAPlayer3.Pad.b押されたDGB(Eパッド.RRed2P)||TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.D9) )
                    {
                        if (TJAPlayer3.ConfigIni.b太鼓パートAutoPlay == false)
                            TJAPlayer3.ConfigIni.b太鼓パートAutoPlay = true;
                        else
                            TJAPlayer3.ConfigIni.b太鼓パートAutoPlay = false;
                    }
                    if (TJAPlayer3.Pad.b押されたDGB(Eパッド.RBlue2P) || TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.D0))
                    {
                        TJAPlayer3.ConfigIni.n譜面スクロール速度.Drums = Math.Min(TJAPlayer3.ConfigIni.n譜面スクロール速度.Drums + 1, 1999);
                    }
                    if (TJAPlayer3.Pad.b押されたDGB(Eパッド.LBlue2P) || TJAPlayer3.Input管理.Keyboard.bキーが押された((int)Key.D9))
                    {
                        TJAPlayer3.ConfigIni.n譜面スクロール速度.Drums = Math.Max(TJAPlayer3.ConfigIni.n譜面スクロール速度.Drums -1, 0);
                    }


                }

                
                if (this.bスクロール中)
                {
                    CSound管理.rc演奏用タイマ.n現在時刻ms = easing.EaseOut(this.スクロールカウンター, (int)this.nスクロール前ms, (int)this.nスクロール後ms, Easing.CalcType.Circular);

                    this.スクロールカウンター.t進行();

                    if ((int)CSound管理.rc演奏用タイマ.n現在時刻ms == (int)this.nスクロール後ms)
                    {
                        this.bスクロール中 = false;
                        CSound管理.rc演奏用タイマ.n現在時刻ms = this.nスクロール後ms;
                    }
                }

                if (!this.b特訓PAUSE && this.n現在の小節線 < TJAPlayer3.stage演奏ドラム画面.actPlayInfo.NowMeasure[0]) this.n現在の小節線 = TJAPlayer3.stage演奏ドラム画面.actPlayInfo.NowMeasure[0];

            }
            return base.On進行描画();
        }

        public void t演奏を停止する()
        {
            CDTX dTX = TJAPlayer3.DTX;

            this.nスクロール後ms = CSound管理.rc演奏用タイマ.n現在時刻ms;

            TJAPlayer3.stage演奏ドラム画面.actAVI.tReset();
            TJAPlayer3.stage演奏ドラム画面.On活性化();
            CSound管理.rc演奏用タイマ.t一時停止();

            for (int i = 0; i < dTX.listChip.Count; i++)
            {
                CDTX.CChip pChip = dTX.listChip[i];
                pChip.bHit = false;
                pChip.bShow = true;
                pChip.b可視 = true;
            }

            TJAPlayer3.DTX.t全チップの再生一時停止();
            TJAPlayer3.stage演奏ドラム画面.bPAUSE = true;
            this.b特訓PAUSE = true;

            this.t譜面の表示位置を合わせる(false);
        }

        public void t演奏を再開する()
        {
            CDTX dTX = TJAPlayer3.DTX;

            this.bスクロール中 = false;
            CSound管理.rc演奏用タイマ.n現在時刻ms = this.nスクロール後ms;

            int n演奏開始Chip = TJAPlayer3.stage演奏ドラム画面.n現在のトップChip;

            int finalStartBar;

            if (this.n現在の小節線 <= 0) finalStartBar = this.n現在の小節線;
            else finalStartBar = this.n現在の小節線 - 1;

            TJAPlayer3.stage演奏ドラム画面.t演奏位置の変更(finalStartBar, 0);

            for (int i = 0; i < dTX.listChip.Count; i++)
            {
                if (i < n演奏開始Chip)
                {
                    dTX.listChip[i].bHit = true;
                    dTX.listChip[i].IsHitted = true;
                    dTX.listChip[i].b可視 = false;
                    dTX.listChip[i].bShow = false;
                }
            }

            TJAPlayer3.stage演奏ドラム画面.t数値の初期化(true, true);
            TJAPlayer3.stage演奏ドラム画面.actAVI.tReset();
            TJAPlayer3.stage演奏ドラム画面.On活性化();

            for (int i = 0; i < TJAPlayer3.ConfigIni.nPlayerCount; i++)
            {
                TJAPlayer3.stage演奏ドラム画面.chip現在処理中の連打チップ[i] = null;
            }

            this.b特訓PAUSE = false;
        }

        public void t譜面の表示位置を合わせる(bool doScroll)
        {
            this.nスクロール前ms = CSound管理.rc演奏用タイマ.n現在時刻ms;

            CDTX dTX = TJAPlayer3.DTX;

            bool bSuccessSeek = false;
            for (int i = 0; i < dTX.listChip.Count; i++)
            {
                CDTX.CChip pChip = dTX.listChip[i];

                if (pChip.n発声位置 < 384 * (n現在の小節線))
                {
                    continue;
                }
                else
                {
                    bSuccessSeek = true;
                    TJAPlayer3.stage演奏ドラム画面.n現在のトップChip = i;
                    break;
                }
            }
            if (!bSuccessSeek)
            {
                TJAPlayer3.stage演奏ドラム画面.n現在のトップChip = 0;
            }

            if (doScroll)
            {
                this.nスクロール後ms = dTX.listChip[TJAPlayer3.stage演奏ドラム画面.n現在のトップChip].n発声時刻ms;
                this.bスクロール中 = true;

                this.スクロールカウンター = new CCounter(0, 350, 1, TJAPlayer3.Timer);
            }
            else
            {
                CSound管理.rc演奏用タイマ.n現在時刻ms = dTX.listChip[TJAPlayer3.stage演奏ドラム画面.n現在のトップChip].n発声時刻ms;
            }
        }

        public int n現在の小節線;
        public int n小節の総数;

        #region [private]
        private long nスクロール前ms;
        private long nスクロール後ms;

        private bool b特訓PAUSE;
        private bool bスクロール中;

        private CCounter スクロールカウンター;
        private Easing easing = new Easing();
        #endregion
    }
}
