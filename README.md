# get-color-for-windows
windows上のマウス座標から、色情報を取得するツール
必要があって調べた内容

 - マルチディスプレイ対応
 - 4Kモニタ対応
 - Visual Studio 2015 
 - .NET Framework 4.5.2
 - C#
 - 利用しているwin32 API
    - GetDC
    - ReleaseDC
    - BitBlt
    - EnumDisplayMonitors
    
![demo](https://github.com/hanamizuki10/get-color-for-windows/blob/main/image.gif?raw=true)
