﻿using System;

namespace TestMyMinesweeper
{
    class Common
    {
        public static string GetEnvironmentDirPath()
        {
            if (System.IO.Directory.Exists(Environment.CurrentDirectory + "/Resource"))
            {
                // テストスイートの場合、2回目以降？はすでに設定済み
                return Environment.CurrentDirectory;
            }

            string master = Environment.CurrentDirectory + "../../../";  // テストの単体実行時
            if (!System.IO.Directory.Exists(master + "/Resource"))
            {
                // テストスイートによる全テスト実行時
                master = Environment.CurrentDirectory + "../../../../TestMyMinesweeper";
            }

            return master;
        }

        public static string GetFilePathOfDependentEnvironment(string filePath)
        {
            return Environment.CurrentDirectory + filePath;
        }
    }
}
