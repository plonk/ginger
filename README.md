# ginger

PeerCastStation や PeerCast YT を JSON RPC で操作するソフトです。

# Linux での起動方法

mono と Gtk# が必要です。PeerCastStation を使っている人なら mono は入
れていると思いますが、Ubuntu のパッケージ名で言うと、mono-runtime と
libmono-system-net-http4.0-cil が必要です。(ディスク事情が許すなら
mono-complete パッケージで全部入れてしまうのが楽です。)

Gtk# のパッケージ名は、gtk-sharp2 です。全て入っていれば、mono
ginger.exe で起動するはずです。

# Windows での起動方法

Gtk# をインストールする必要があります。同梱の gtk-sharp-2.12.42.msi を
実行してインストールしてください。

ginger.exe を実行すると起動します。
