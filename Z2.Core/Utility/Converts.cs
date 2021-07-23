using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.Utility
{
    public static class Converts
    {
        /// <summary>
        /// List&lt;Dictionary&lt;string, object&gt;&gt;型をDataTable型へキャストする
        /// </summary>
        /// <param name="ListData">List&lt;Dictionary&lt;string, object&gt;&gt;型</param>
        /// <returns>DataTable型</returns>
        //public static DataTable ToDataTable(List<Dictionary<string, object>> ListData)
        //{
        //    DataTable result = new DataTable();

        //    if (ListData == null || ListData.Count == 0)
        //    {
        //        return null;
        //    }

        //    // カラムの作成
        //    Dictionary<string, object> column = ListData[0];

        //    foreach (string item in ListData[0].Keys)
        //    {
        //        // カラム名の取得
        //        string key = item;

        //        // 型の取得
        //        Type type = column[key].GetType();

        //        result.Columns.Add(key, type);
        //    }

        //    // 値の設定
        //    foreach (Dictionary<string, object> rowitem in ListData)
        //    {
        //        DataRow dtrow = result.NewRow();

        //        foreach (string columnitem in rowitem.Keys)
        //        {
        //            // カラム名の取得
        //            string key = columnitem;

        //            dtrow[key] = rowitem[key];
        //        }

        //        result.Rows.Add(dtrow);
        //    }

        //    return result;
        //}

        /// <summary>
        /// 和暦変換フォーマットタイプ
        /// </summary>
        public enum DateFormatTypeEnum
        {
            /// <summary>ggyy年MM月dd日</summary>
            DateOnly = 0,
            /// <summary>ggyy年MM月dd日 HH:mm</summary>
            DateAndTime,
            /// <summary>HH:mm</summary>
            TimeOnly,
            /// <summary>yyyy年MM月</summary>
            YearMonthOnly
        }
        /// <summary>
        /// DateTiem型をString型の和暦表示にキャストする
        /// </summary>
        /// <param name="Source">対象の日付</param>
        /// <param name="FormatType">和暦変換フォーマット</param>
        /// <returns></returns>
        public static string ToDateJp(object Source, DateFormatTypeEnum FormatType)
        {
            string result = String.Empty;

            if (Source == null || Convert.ToString(Source).Length == 0)
            {
                return result;
            }

            DateTime date = Convert.ToDateTime(Source);

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("ja-JP", true);
            culture.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();

            if (FormatType == DateFormatTypeEnum.DateOnly)
            {
                result = date.ToString("ggy年M月d日", culture);
            }
            else if (FormatType == DateFormatTypeEnum.DateAndTime)
            {
                result = date.ToString("ggy年M月d日　HH:mm", culture);
            }
            else if (FormatType == DateFormatTypeEnum.TimeOnly)
            {
                result = date.ToString("HH:mm", culture);
            }
            else if (FormatType == DateFormatTypeEnum.YearMonthOnly)
            {
                result = date.ToString("ggyy年M月", culture);
            }

            return result;
        }

        /// <summary>
        /// ファイルをBase64形式エンコードする
        /// </summary>
        /// <param name="FilePath">変換するファイルのフルパス</param>
        /// <returns></returns>
        public static string ToBase64Encode(string FilePath)
        {
            string result = String.Empty;

            //Base64で文字列に変換するファイル
            string inFileName = FilePath;
            System.IO.FileStream inFile;
            byte[] bs;

            //ファイルをbyte型配列としてすべて読み込む
            inFile = new System.IO.FileStream(inFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            bs = new byte[inFile.Length];
            int readBytes = inFile.Read(bs, 0, (int)inFile.Length);
            inFile.Close();

            //Base64で文字列に変換
            result = System.Convert.ToBase64String(bs);

            return result;
        }

        /// <summary>
        /// Base64形式の文字データをBase64デコードする
        /// </summary>
        /// <param name="Source">変換するファイルの文字列データ</param>
        /// <param name="OutFilePath">ファイル出力先のフルパス</param>
        /// <returns></returns>
        public static bool ToBase64Decode(string Source, string OutFilePath)
        {
            byte[] bs;

            //バイト型配列に戻す
            bs = System.Convert.FromBase64String(Source);

            //ファイルに保存する
            //保存するファイル名
            string outFileName = OutFilePath;
            //ファイルに書き込む
            System.IO.FileStream outFile = new System.IO.FileStream(outFileName,
                System.IO.FileMode.Create, System.IO.FileAccess.Write);
            outFile.Write(bs, 0, bs.Length);

            outFile.Close();

            return true;
        }

        /// <summary>
        /// 小文字カナを大文字カナに変換する
        /// </summary>
        /// <param name="Source">対象の文字列</param>
        /// <returns>変換した文字列</returns>
        public static string ToUpperKana(string Source)
        {
            StringBuilder sb = new StringBuilder(Source);
            sb = sb.Replace('ｧ', 'ｱ');
            sb = sb.Replace('ｨ', 'ｲ');
            sb = sb.Replace('ｩ', 'ｳ');
            sb = sb.Replace('ｪ', 'ｴ');
            sb = sb.Replace('ｫ', 'ｵ');
            sb = sb.Replace('ｬ', 'ﾔ');
            sb = sb.Replace('ｭ', 'ﾕ');
            sb = sb.Replace('ｮ', 'ﾖ');
            sb = sb.Replace('ｯ', 'ﾂ');

            return Convert.ToString(sb);
        }

        /// <summary>
        /// 四捨五入
        /// </summary>
        /// <param name="Source">対象の数値</param>
        /// <param name="Digits">桁の指定</param>
        /// <returns>数値</returns>
        public static decimal ToRound(decimal Source, int Digits)
        {
            decimal result = 0;

            double dCoef = System.Math.Pow(10, Digits);
            double dSource = Convert.ToDouble(Source);

            if (dSource > 0)
            {
                result = Convert.ToDecimal(
                    System.Math.Floor((dSource * dCoef) + 0.5) / dCoef);
            }
            else
            {
                result = Convert.ToDecimal(
                    System.Math.Ceiling((dSource * dCoef) - 0.5) / dCoef);
            }

            return result;
        }

        /// <summary>
        /// 切り上げ
        /// </summary>
        /// <param name="Source">対象の数値</param>
        /// <param name="Digits">桁の指定</param>
        /// <returns>数値</returns>
        public static decimal ToRoundUp(decimal Source, int Digits)
        {
            decimal result = 0;
            double dCoef = System.Math.Pow(10, Digits);
            double dSource = Convert.ToDouble(Source);

            if (dSource > 0)
            {
                result = Convert.ToDecimal(System.Math.Ceiling(dSource * dCoef) / dCoef);
            }
            else
            {
                result = Convert.ToDecimal(System.Math.Floor(dSource * dCoef) / dCoef);
            }

            return result;
        }

        /// <summary>
        /// 切り捨て
        /// </summary>
        /// <param name="Source">対象の数値</param>
        /// <param name="Digits">桁の指定</param>
        /// <returns>数値</returns>
        public static decimal ToRoundDown(decimal Source, int Digits)
        {
            decimal result = 0;
            double dCoef = System.Math.Pow(10, Digits);
            double dSource = Convert.ToDouble(Source);

            if (dSource > 0)
            {
                result = Convert.ToDecimal(System.Math.Floor(dSource * dCoef) / dCoef);
            }
            else
            {
                result = Convert.ToDecimal(System.Math.Ceiling(dSource * dCoef) / dCoef);
            }

            return result;
        }

        /// <summary>
        /// Decimal型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <returns></returns>
        public static decimal ToTryDecimal(object Source)
        {
            return _ToTryDecimal(Source, 0m);
        }

        /// <summary>
        /// Decimal型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure">失敗した場合のデフォルト値</param>
        /// <returns>数値</returns>
        public static decimal ToTryDecimal(object Source, decimal Failure)
        {
            return _ToTryDecimal(Source, Failure);
        }

        /// <summary>
        /// Decimal型に強制キャストする
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        private static decimal _ToTryDecimal(object Source, decimal Failure)
        {
            decimal result = Failure;
            decimal CheckResult = 0;

            if (decimal.TryParse(Convert.ToString(Source), out CheckResult) == true)
            {
                result = CheckResult;
            }

            return result;
        }

        /// <summary>
        /// Int型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        public static long ToTryInt64(object Source)
        {
            return _ToTryInt(Source, 0);
        }

        // <summary>
        /// Int型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure">失敗した場合のデフォルト値</param>
        /// <returns>数値</returns>
        public static long ToTryInt64(object Source, long Failure)
        {
            return _ToTryInt64(Source, Failure);
        }

        /// <summary>
        /// Int型に強制キャストする
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        private static long _ToTryInt64(object Source, long Failure)
        {
            long result = Failure;
            long CheckResult = 0;

            if (long.TryParse(Convert.ToString(Source), out CheckResult) == true)
            {
                result = CheckResult;
            }

            return result;
        }


        /// <summary>
        /// Int型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        public static int ToTryInt(object Source)
        {
            return _ToTryInt(Source, 0);
        }

        // <summary>
        /// Int型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure">失敗した場合のデフォルト値</param>
        /// <returns>数値</returns>
        public static int ToTryInt(object Source, int Failure)
        {
            return _ToTryInt(Source, Failure);
        }

        /// <summary>
        /// Int型に強制キャストする
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        private static int _ToTryInt(object Source, int Failure)
        {
            int result = Failure;
            int CheckResult = 0;

            if (int.TryParse(Convert.ToString(Source), out CheckResult) == true)
            {
                result = CheckResult;
            }

            return result;
        }

        /// <summary>
        /// Int型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <returns>数値</returns>
        public static int ToTryInt(bool Source)
        {
            int result = 0;

            if (Source == true)
            {
                result = 1;
            }

            return result;
        }

        /// <summary>
        /// string型に強制キャストする(VB6のNVLと同等の機能)
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure">失敗した場合のデフォルト値</param>
        /// <returns>文字</returns>
        public static string ToTryString(object Source)
        {
            return _ToTryString(Source, "");
        }

        /// <summary>
        /// string型に強制キャストする(VB6のNVLと同等の機能)
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure">失敗した場合のデフォルト値</param>
        /// <returns></returns>
        public static string ToTryString(object Source, string Failure)
        {
            return _ToTryString(Source, Failure);
        }

        /// <summary>
        /// string型に強制キャストする(VB6のNVLと同等の機能)
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        private static string _ToTryString(object Source, string Failure)
        {
            string result = Failure;

            if (String.IsNullOrEmpty(Convert.ToString(Source)) == true) //NULL, Emptyの場合
            {
                return result;
            }

            if (String.IsNullOrWhiteSpace(Convert.ToString(Source)) == true) //NULL, 空白の場合
            {
                return result;
            }

            return Convert.ToString(Source);
        }

        /// <summary>
        /// Bool型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <returns>bool(1or-1:true, 0:false)</returns>
        public static bool ToTryBool(object Source)
        {
            return _ToTryBool(Source, false);
        }

        /// <summary>
        /// Bool型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure">失敗した場合のデフォルト値</param>
        /// <returns>bool</returns>
        public static bool ToTryBool(object Source, bool Failure)
        {
            return _ToTryBool(Source, Failure);
        }

        /// <summary>
        /// Bool型に強制キャストする
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        private static bool _ToTryBool(object Source, bool Failure)
        {
            bool result = Failure;
            int CheckResult = 0;

            bool BoolResult = false;

            if (bool.TryParse(Convert.ToString(Source), out BoolResult) == true)
            {
                result = BoolResult;
            }
            else if (int.TryParse(Convert.ToString(Source), out CheckResult) == true)
            {
                if (CheckResult == -1 || CheckResult == 1)
                {
                    result = true;
                }
                else if (CheckResult == 0)
                {
                    result = false;
                }
                else
                {
                    // Failure
                }
            }

            return result;
        }

        /// <summary>
        /// TODO:暫定で定義。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="failure"></param>
        /// <returns></returns>
        public static double ToTryDouble(object source, double failure)
        {
            double tmp;
            if (Double.TryParse(Convert.ToString(source), out tmp))
            {
                return tmp;
            }
            return failure;
        }

        /// <summary>
        /// TODO:暫定で定義。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="failure"></param>
        /// <returns></returns>
        public static DateTime ToTryDateTime(object source, DateTime failure)
        {
            DateTime tmp;
            if (DateTime.TryParse(Convert.ToString(source), out tmp))
            {
                return tmp;
            }
            return failure;
        }

        /// <summary>
        /// TODO:暫定で定義。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="failure"></param>
        /// <returns></returns>
        public static DateTime? ToTryDateTimeNullable(object source, DateTime? failure)
        {
            DateTime tmp;
            if (DateTime.TryParse(Convert.ToString(source), out tmp))
            {
                return tmp;
            }
            return failure;
        }
    }

}
