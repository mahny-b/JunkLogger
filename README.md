# JunkLogger
It's a very simple logger, so I can use it. It is used for development that does not require the introduction of a full-fledged logger.
log4net等の本格的なロガーを入れるまでもないけど、Excelアドイン開発等でちょっと確認したい時にコピペで使える簡易ロガーです。
商用リリース的なものには向きません。せいぜい社内利用に留めて下さい。
機能拡張を考えるくらいなら素直にlog4net等の導入を検討してください。


# 使い方
1. `MahnyBrand/Delusion/Utils/Logger.cs`をプロジェクト内の任意の場所にコピーして下さい
2. `Program.cs`を参考に実装して下さい
3. 既定ではデスクトップに`Application.log`というファイルを出力します
4. 既定のログレベルは`Debug`です


# 設定
Loggerクラスを直接書き変える事で設定を変更します。

```cs
/// <summary>
/// 疑似的なロギングを行うユーティリティ
/// </summary>
private Logger()
{
    // ★設定はここで自由に書き換えてくだし
    // ログレベル（Noneを指定すると出力しなくなる）
    this.Level = LogLevel.Debug;

    // ログ出力先。デフォルトではデスクトップに出力します
    string LogDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    LogFilePath = Path.Combine(LogDirPath, "Application.log");
}
```

## ログレベル変更
この部分を書き換えます。

```cs
// ログレベル（Noneを指定すると出力しなくなる）
this.Level = LogLevel.Debug;
```

例えば、Infoレベルにする場合はこのようにします。

```cs
// ログレベル（Noneを指定すると出力しなくなる）
this.Level = LogLevel.Info;
```

ログレベルは7段階あります。`None`は特別なレベルでログを出さなくなります。
Trace < Debug < Info < Warn < Error < Fatal < None


## 出力先変更
この部分を書き換えます。

```cs
// ログ出力先。デフォルトではデスクトップに出力します
string LogDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
LogFilePath = Path.Combine(LogDirPath, "Application.log");
```

例えば、出力フォルダをマイドキュメントにし、ファイル名を`app.log`にする場合はこのようにします。

```cs
// ログ出力先。デフォルトではデスクトップに出力します
string LogDirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
LogFilePath = Path.Combine(LogDirPath, "app.log");
```


以上
