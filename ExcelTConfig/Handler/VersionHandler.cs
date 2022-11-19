using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExcelTConfig
{
    public static class VersionHandler
    {
        public static string CreateVersionSql = @"DROP TABLE IF EXISTS `dict_version`;
CREATE TABLE IF NOT EXISTS `dict_version`( 
`id` INT(10) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'id',
`version` INT(10) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'version',
PRIMARY KEY(`id`) 
) ENGINE = MyISAM DEFAULT CHARSET = utf8mb4; ";

        public static string InsertVersionSql = @"DELETE FROM `dict_version`;
INSERT INTO `dict_version` (`id`,`version`) VALUES 
(1,{0}),
(2,{1});";

        public static void WriteVersion(bool bPack)
        {
            int newVersion = -1;
            int newPackVersion = -1;

            WriteVersionJson(bPack, ref newVersion, ref newPackVersion);
            if (newVersion == -1 || newPackVersion == -1)
            {
                Entry.UpdateLogInfo("WriteVersion Error");
                //throw new Exception("WriteVersion Error");
            }

            WriteVersionSql(bPack, newVersion, newPackVersion);
        }

        public static void WriteVersionJson(bool bPack, ref int version, ref int packVersion)
        {
            var versionInfoPath = "Version.json";
            using (FileStream stream = new FileStream(versionInfoPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                int length = (int)stream.Length;
                byte[] bytes = new byte[length];
                int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                int readLeng = 0;
                while (readLeng < length)
                {
                    int len = stream.Read(buffer, readLeng, bufferSize);
                    Array.Copy(buffer, 0, bytes, readLeng, len);
                    readLeng += len;
                }
                string versionText = Encoding.UTF8.GetString(bytes);

                // 清空文件内容
                stream.Seek(0, SeekOrigin.Begin);
                stream.SetLength(0);

                JObject versionInfoJson;
                if (string.IsNullOrEmpty(versionText))
                {
                    versionInfoJson = new JObject();
                }
                else
                {
                    versionInfoJson = JObject.Parse(versionText);
                }
                JToken jToken;
                if (versionInfoJson.TryGetValue("Binary", out jToken))
                {
                    version = (int)jToken;
                    if (!bPack)
                    {
                        version++;
                        versionInfoJson["Binary"] = version;
                    }
                }
                else
                {
                    versionInfoJson["Binary"] = 1;
                }
                if (versionInfoJson.TryGetValue("BinaryPack", out jToken))
                {
                    packVersion = (int)jToken;
                    if (bPack)
                    {
                        packVersion++;
                        versionInfoJson["BinaryPack"] = packVersion;
                    }
                }
                else
                {
                    versionInfoJson["BinaryPack"] = 1;
                }

                byte[] writeBytes = Encoding.UTF8.GetBytes(versionInfoJson.ToString());
                stream.Write(writeBytes, 0, writeBytes.Length);
            }
        }


        static void WriteVersionSql(bool bPack, int newVersion, int newPackVersion)
        {
            var path = Path.Combine(Entry.SqlDataAndTableFolderPath, "rex_version.sql");
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                // 清空文件内容
                stream.Seek(0, SeekOrigin.Begin);
                stream.SetLength(0);

                var text = $"{CreateVersionSql}\r\n\r\n{string.Format(InsertVersionSql, newVersion, newPackVersion)}";
                byte[] writeBytes = Encoding.UTF8.GetBytes(text);

                var path2 = Path.Combine(Entry.InsertSqlFolderPath, "rex_version.sql");
                using (FileStream stream2 = new FileStream(path2, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    // 清空文件内容
                    stream2.Seek(0, SeekOrigin.Begin);
                    stream2.SetLength(0);

                    stream2.Write(writeBytes, 0, writeBytes.Length);
                }

                stream.Write(writeBytes, 0, writeBytes.Length);
            }
        }
    }
}
