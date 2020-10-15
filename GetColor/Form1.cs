using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;


namespace GetColor
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// マウスポジション監視用スレッド
        /// </summary>
        private Thread _thread = null;

        /// <summary>
        /// モニタ情報格納用class
        /// </summary>
        private class MonitorInfo
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public int width;
            public int height;
        }

        /// <summary>
        /// 接続されているモニターリスト
        /// </summary>
        private List<MonitorInfo> _monitors;

        /// <summary>
        /// BitBlt用ラスタオペレーションコード定数 SRCCOPY：コピー元をコピー先にそのままコピー
        /// </summary>
        private static System.Int32 SRCCOPY = 0x00CC0020;

        /// <summary>
        /// 画像のビットブロック転送
        /// </summary>
        /// <param name="hDestDC">転送先のデバイスコンテキストへのハンドル</param>
        /// <param name="x">転送先の左上のx座標を論理座標</param>
        /// <param name="y">転送先の左上のy座標を論理座標</param>
        /// <param name="nWidth">転送元および転送先の長方形の幅</param>
        /// <param name="nHeight">転送元および転送先の長方形の高さ</param>
        /// <param name="hSrcDC">転送元のデバイスコンテキストへのハンドル</param>
        /// <param name="xSrc">転送元の左上のx座標を論理座標</param>
        /// <param name="ySrc">転送元の左上のy座標を論理座標</param>
        /// <param name="dwRop">ラスター操作コードを以下の定数で指定します。これらのコードは、最終的な色を作成するために、転送元のカラーデータが転送先のカラーデータと結合される方法を定義します。</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        private static extern int BitBlt(IntPtr hDestDC,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hSrcDC,
            int xSrc,
            int ySrc,
            int dwRop);

        /// <summary>
        /// 指定されたウィンドウのクライアント領域またはスクリーン全体に対応するディスプレイデバイスコンテキストのハンドルを取得
        /// </summary>
        /// <param name="hwnd">ウィンドウハンドル</param>
        /// <returns>ディスプレイデバイスコンテキストのハンドル</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        /// <summary>
        /// デバイスコンテキストを解放
        /// </summary>
        /// <param name="hwnd">解放するデバイスコンテキストに対応するウィンドウのハンドル</param>
        /// <param name="hdc">解放するデバイスコンテキストのハンドル</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);


        /// <summary>
        /// パソコンに接続されている複数のモニターの各大きさと、仮想スクリーン上の位置を取得
        /// </summary>
        /// <param name="hdc">デバイスコンテキストのハンドル</param>
        /// <param name="lprcClip">クリッピング領域のポインタ</param>
        /// <param name="lpfnEnum">コールバック関数のポインタ</param>
        /// <param name="dwData">関数に渡されるパラメータ</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

        /// <summary>
        /// 座標情報
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        /// <summary>
        /// パソコンに接続されている複数のモニター情報取得時のコールバック用関数定義
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nc-winuser-monitorenumproc
        /// </summary>
        /// <param name="hMonitor">ディスプレイモニターへのハンドル</param>
        /// <param name="hdcMonitor">デバイスコンテキストへのハンドル</param>
        /// <param name="lprcMonitor">RECT構造体へのポインタ</param>
        /// <param name="dwData">EnumDisplayMonitorsが列挙関数に直接渡すアプリケーション定義のデータ</param>
        /// <returns>列挙を続行するには、TRUEを返します。列挙を停止するには、FALSEを返します。</returns>
        private delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _monitors = new List<MonitorInfo>();
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
            delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
            {
                MonitorInfo mInfo = new MonitorInfo();
                mInfo.left = lprcMonitor.left;
                mInfo.top = lprcMonitor.top;
                mInfo.right = lprcMonitor.right;
                mInfo.bottom = lprcMonitor.bottom;
                mInfo.width = lprcMonitor.right - lprcMonitor.left;
                mInfo.height = lprcMonitor.bottom - lprcMonitor.top;
                _monitors.Add(mInfo);
                return true;
            }, IntPtr.Zero);
            // ※補足
            // foreach (System.Windows.Forms.Screen s in System.Windows.Forms.Screen.AllScreens)
            // でも同じような情報は取得できるが、EnumDisplayMonitorsのほうが処理速度が早い。
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_thread != null)
            {
                // スレッドを停止せずにフォームを閉じると不要な関数呼び出しが発生する為、強制停止
                _thread.Abort();
                _thread = null;
            }
        }

        /// <summary>
        /// 状態切り替えボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonToggle_Click(object sender, EventArgs e)
        {
            if (_thread != null)
            {
                // カラーコードを取得処理を停止する
                _thread.Abort();
                _thread = null;
                buttonToggle.Text = "スタート (Click Or Space Or Enter)";
            }
            else
            {
                // 定期的なカラーコードを取得処理を開始する
                _thread = new Thread(new ThreadStart(getMousePosition));
                _thread.Start();
                buttonToggle.Text = "ストップ (Space Or Enter)";
            }
        }

        /// <summary>
        /// 100ミリ秒間隔でマウスポジションを取得してテキストデータにカラーコード情報を設定する
        /// </summary>
        private void getMousePosition()
        {
            while (true)
            {
                setPositionColorInfo();
                Thread.Sleep(100);
            }
        }

        private delegate void SetTextDelegate();

        private void setPositionColorInfo()
        {
            if (InvokeRequired)
            {
                Invoke(new SetTextDelegate(setPositionColorInfo));
                return;
            }
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            textBoxX.Text = x.ToString();
            textBoxY.Text = y.ToString();
            MonitorInfo m = getMonitorInfo(_monitors, x, y);
            if (m != null)
            {
                Color color = getPixelColor(m, x, y);
                panelColor.BackColor = color;
                textBoxColor.Text = color.ToString();
                // 16進数カラーコード表記
                textBoxColor16.Text = String.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
            }
            else
            {
                panelColor.BackColor = Color.Black;
                textBoxColor.Text = "カラー情報取得時に予期せぬエラーが発生しました。";
                textBoxColor16.Text = "カラー情報取得時に予期せぬエラーが発生しました。";
            }
        }

        /// <summary>
        /// マウスのポジションがどのモニター上の情報であるか取得する
        /// </summary>
        /// <param name="monitors">接続されているモニターリスト</param>
        /// <param name="x">マウスポインタのX座標</param>
        /// <param name="y">マウスポインタのY座標</param>
        /// <returns></returns>
        private static MonitorInfo getMonitorInfo(List<MonitorInfo> monitors, int x, int y)
        {
            foreach(var m in monitors)
            {
                if ((m.left <= x) && (x <= (m.right)))
                {
                    if ((m.top <= y) && (y <= (m.bottom)))
                    {
                        return m;
                    }
                }
            }
            return null;
        }
 
        /// <summary>
        /// 指定マウスポインタ座標上のカラー情報を取得する
        /// </summary>
        /// <param name="m">接続されているモニター情報</param>
        /// <param name="x">マウスポインタのX座標</param>
        /// <param name="y">マウスポインタのY座標</param>
        /// <returns>指定したXY座標1x1サイズのColor情報</returns>
        private static Color getPixelColor(MonitorInfo m, int x, int y)
        {
            //モニタのデバイスコンテキストを取得
            IntPtr disDC = GetDC(IntPtr.Zero);
            //Bitmapの作成
            Bitmap bmp = new Bitmap(m.width, m.height);
            //Graphicsの作成
            Graphics g = Graphics.FromImage(bmp);
            //Graphicsのデバイスコンテキストを取得
            IntPtr hDC = g.GetHdc();
            //Bitmapに画像をコピーする
            BitBlt(hDC, 0, 0, bmp.Width, bmp.Height,
                disDC, m.left, m.top, SRCCOPY);

            //解放
            g.ReleaseHdc(hDC);
            g.Dispose();
            ReleaseDC(IntPtr.Zero, disDC);

            Color color = bmp.GetPixel((x - m.left), (y - m.top));
            // bmp.Save("C:\\WorkLog\\1.png");  // 一時的検証用

            bmp.Dispose();

            return color;

        }
    }
}
