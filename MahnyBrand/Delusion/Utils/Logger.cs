/// MIT License
/// Copyright (c) 2021 mahny
/// twitter: @mahny_b
using System;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;

namespace MahnyBrand.Delusion.Utils
{
    /// <summary>
    /// 簡易ロギングユーティリティ
    /// </summary>
    /// <remarks>
    /// ハウスキープ等はしないので、ログファイルを消すなど適宜掃除してください
    /// </remarks>
    class Logger
    {
        private enum LogLevel
        {
            /// トレース
            Trace,
            /// デバッグ
            Debug,
            /// 情報
            Info,
            /// 警告
            Warn,
            /// エラー
            Error,
            /// 致命的
            Fatal,
            /// ログ出力をしない
            None,
        }
        /// 簡易的なものだし、ファイルハンドラ取り合っても面白くないのでシングルトンですよ
        private static Logger Instance = new Logger();

        /// ログレベル
        private readonly LogLevel Level;

        /// ログファイルパス
        private readonly string LogFilePath;

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

        /// <summary>
        /// ログを出力する
        /// </summary>
        /// <param name="Level">ログレベル</param>
        /// <param name="Message">ログメッセージ</param>
        /// <param name="AssemblyName">アセンブリ名</param>
        /// <param name="Method">メソッド名</param>
        /// <param name="Line">行番号</param>
        private static void Output(LogLevel Level, string Message, string AssemblyName, string Method, int Line)
        {
            if (Level < Instance.Level)
            {
                // 設定レベル未満は出力しない。Noneは最強なので常に出力しない
                return;
            }
            string FilePath = string.IsNullOrEmpty(AssemblyName) ? AssemblyName : Path.GetFileName(AssemblyName).Replace(".cs", "");
            string Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string LevelStr = Level.ToString().ToUpper();

            // ".ctor"はコンストラクタで拾ってしまう文字列。滅殺
            string MethodStr = Method == ".ctor" ? "<Init>" : Method;
            string Contents = $"{Timestamp} [{LevelStr,-5}] ({FilePath}#{MethodStr}:{Line,3}) - {Message}{Environment.NewLine}";

            if (!File.Exists(Instance.LogFilePath))
            {
                StringBuilder Usage = new StringBuilder("コマンドプロンプトで下記コマンドを実行するとログ追記を監視出来ます。").Append(Environment.NewLine)
                    .Append($"powershell -c \"Get-Content -Path \'{Instance.LogFilePath}\' -Wait -Encoding UTF8 -Tail 5\"").Append(Environment.NewLine);
                File.AppendAllText(Instance.LogFilePath, Usage.ToString());
            }
            File.AppendAllText(Instance.LogFilePath, Contents, Encoding.UTF8);
        }

        /// <summary>
        /// トレースログを出力する
        /// </summary>
        /// <param name="Message">ログメッセージ</param>
        /// <param name="AssemblyName">アセンブリ名</param>
        /// <param name="Method">メソッド名</param>
        /// <param name="Line">行番号</param>
        public static void Trace(string Message = "", [CallerFilePath] string AssemblyName = "", [CallerMemberName] string Method = "", [CallerLineNumber] int Line = 0)
        {
            Output(LogLevel.Trace, Message, AssemblyName, Method, Line);
        }

        /// <summary>
        /// デバッグログを出力する
        /// </summary>
        /// <param name="Message">ログメッセージ</param>
        /// <param name="AssemblyName">アセンブリ名</param>
        /// <param name="Method">メソッド名</param>
        /// <param name="Line">行番号</param>
        public static void Debug(string Message = "", [CallerFilePath] string AssemblyName = "", [CallerMemberName] string Method = "", [CallerLineNumber] int Line = 0)
        {
            Output(LogLevel.Debug, Message, AssemblyName, Method, Line);
        }

        /// <summary>
        /// 通常ログを出力する
        /// </summary>
        /// <param name="Message">ログメッセージ</param>
        /// <param name="AssemblyName">アセンブリ名</param>
        /// <param name="Method">メソッド名</param>
        /// <param name="Line">行番号</param>
        public static void Info(string Message = "", [CallerFilePath] string AssemblyName = "", [CallerMemberName] string Method = "", [CallerLineNumber] int Line = 0)
        {
            Output(LogLevel.Info, Message, AssemblyName, Method, Line);
        }

        /// <summary>
        /// 警告ログを出力する
        /// </summary>
        /// <param name="Message">ログメッセージ</param>
        /// <param name="AssemblyName">アセンブリ名</param>
        /// <param name="Method">メソッド名</param>
        /// <param name="Line">行番号</param>
        public static void Warn(string Message = "", [CallerFilePath] string AssemblyName = "", [CallerMemberName] string Method = "", [CallerLineNumber] int Line = 0)
        {
            Output(LogLevel.Warn, Message, AssemblyName, Method, Line);
        }

        /// <summary>
        /// エラーログを出力する
        /// </summary>
        /// <param name="Message">ログメッセージ</param>
        /// <param name="AssemblyName">アセンブリ名</param>
        /// <param name="Method">メソッド名</param>
        /// <param name="Line">行番号</param>
        public static void Error(string Message = "", [CallerFilePath] string AssemblyName = "", [CallerMemberName] string Method = "", [CallerLineNumber] int Line = 0)
        {
            Output(LogLevel.Error, Message, AssemblyName, Method, Line);
        }

        /// <summary>
        /// 致命的なログを出力する
        /// </summary>
        /// <param name="Message">ログメッセージ</param>
        /// <param name="AssemblyName">アセンブリ名</param>
        /// <param name="Method">メソッド名</param>
        /// <param name="Line">行番号</param>
        public static void Fatal(string Message = "", [CallerFilePath] string AssemblyName = "", [CallerMemberName] string Method = "", [CallerLineNumber] int Line = 0)
        {
            Output(LogLevel.Fatal, Message, AssemblyName, Method, Line);
        }
    }
}
