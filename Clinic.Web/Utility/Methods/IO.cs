// ***********************************************************
// 封装一些基本IO访问方法集合
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Windy.WebMVC.Web2.Utility
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// 封装基本IO访问方法
        /// </summary>
        public struct IO
        {
            /// <summary>
            /// 创建指定的本地目录
            /// </summary>
            /// <param name="dirPath">目录路径</param>
            /// <returns>true:创建成功;false:创建失败</returns>
            public static bool CreateDirectory(string dirPath)
            {
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                    if (dirInfo.Exists)
                        return true;
                    Directory.CreateDirectory(dirPath);
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 删除指定的本地目录
            /// </summary>
            /// <param name="dirPath">目录路径</param>
            /// <param name="recursive">是否递归子目录</param>
            /// <returns>true:删除成功;false:删除失败</returns>
            public static bool DeleteDirectory(string dirPath, bool recursive)
            {
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                    if (!dirInfo.Exists)
                        return true;
                    Directory.Delete(dirPath, recursive);
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 返回指定目录下的所有子目录集合(不递归)
            /// </summary>
            /// <param name="rootPath">指定的目录</param>
            /// <returns>查找到的目录集合</returns>
            public static DirectoryInfo[] GetDirectories(string dirPath)
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(dirPath);
                    if (!directory.Exists)
                        return new DirectoryInfo[0];
                    return directory.GetDirectories();
                }
                catch (Exception ex)
                {
                    return new DirectoryInfo[0];
                }
            }

            /// <summary>
            /// 返回指定目录下的所有文件集合(不递归)
            /// </summary>
            /// <param name="directory">指定的目录</param>
            /// <returns>查找到的文件集合</returns>
            public static FileInfo[] GetFiles(string dirPath)
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(dirPath);
                    if (!directory.Exists)
                        return new FileInfo[0];
                    return directory.GetFiles();
                }
                catch (Exception ex)
                {
                   return new FileInfo[0];
                }
            }

            /// <summary>
            /// 在指定的目录下查找包含指定通配符的所有文件
            /// </summary>
            /// <param name="directory">指定的目录</param>
            /// <param name="filemask">通配符</param>
            /// <returns>查找到的文件集合</returns>
            public static List<string> SearchDirectory(string directory, string filemask)
            {
                return SearchDirectory(directory, filemask, true, false);
            }

            /// <summary>
            /// 在指定的目录下查找包含指定通配符的所有文件
            /// </summary>
            /// <param name="directory">指定的目录</param>
            /// <param name="filemask">通配符</param>
            /// <param name="recursive">是否递归</param>
            /// <returns>查找到的文件集合</returns>
            public static List<string> SearchDirectory(string directory, string filemask
                , bool recursive)
            {
                return SearchDirectory(directory, filemask, recursive, false);
            }

            /// <summary>
            /// 在指定的目录下查找包含指定通配符的所有文件
            /// </summary>
            /// <param name="directory">指定的目录</param>
            /// <param name="filemask">通配符</param>
            /// <param name="recursive">是否递归</param>
            /// <param name="ignoreHidden">是否忽略隐藏文件</param>
            /// <returns>查找到的文件集合</returns>
            public static List<string> SearchDirectory(string directory, string filemask
                , bool recursive, bool ignoreHidden)
            {
                return SearchDirectory(directory, filemask, recursive, ignoreHidden, false);
            }

            /// <summary>
            /// 在指定的目录下查找包含指定通配符的所有文件
            /// </summary>
            /// <param name="directory">指定的目录</param>
            /// <param name="filemask">通配符</param>
            /// <param name="recursive">是否递归</param>
            /// <param name="ignoreHidden">是否忽略隐藏文件</param>
            /// <param name="returnNameOnly">是否忽略隐藏文件</param>
            /// <returns>查找到的文件集合</returns>
            public static List<string> SearchDirectory(string directory, string filemask
                , bool recursive, bool ignoreHidden, bool returnNameOnly)
            {
                List<string> files = new List<string>();
                SearchDirectory(directory, filemask, recursive, ignoreHidden, returnNameOnly, files);
                return files;
            }

            /// <summary>
            /// 在指定的目录下查找包含指定通配符的所有文件
            /// </summary>
            /// <param name="directory">指定的目录</param>
            /// <param name="filemask">通配符</param>
            /// <param name="recursive">是否递归</param>
            /// <param name="ignoreHidden">是否忽略隐藏文件</param>
            /// <param name="returnNameOnly">是否忽略隐藏文件</param>
            /// <param name="files">查找到的文件集合</param>
            private static void SearchDirectory(string directory, string filemask, bool recursive
                , bool ignoreHidden, bool returnNameOnly, List<string> files)
            {
                if (files == null) files = new List<string>();
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(directory);
                    if (!dirInfo.Exists)
                        return;
                    FileInfo[] fileInfos = dirInfo.GetFiles(filemask);
                    if (fileInfos == null)
                        return;

                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        if (ignoreHidden &&
                            (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                        {
                            continue;
                        }
                        if (returnNameOnly)
                            files.Add(fileInfo.Name);
                        else
                            files.Add(fileInfo.FullName);
                    }

                    if (!recursive)
                        return;

                    DirectoryInfo[] arrCurrDirs = dirInfo.GetDirectories();
                    foreach (DirectoryInfo currDir in arrCurrDirs)
                    {
                        if (ignoreHidden &&
                            (currDir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                        {
                            continue;
                        }
                        SearchDirectory(currDir.FullName, filemask, recursive, ignoreHidden, returnNameOnly, files);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }

            /// <summary>
            /// 将指定的文件拷贝到指定的目录下,并覆盖
            /// </summary>
            /// <param name="szSouFile">源文件全路径</param>
            /// <param name="szDestFile">目的文件全路径</param>
            /// <returns>true:拷贝成功;false:拷贝失败</returns>
            public static bool CopyFile(string szSouFile, string szDestFile)
            {
                try
                {
                    if (!File.Exists(szSouFile))
                        return false;
                    File.Copy(szSouFile, szDestFile, true);
                    return true;
                }
                catch (Exception ex)
                {
                   return false;
                }
            }

            /// <summary>
            /// 创建指定目录下的文件
            /// </summary>
            /// <param name="fileName">文件全路径</param>
            /// <returns>true:创建成功;false:创建失败</returns>
            /// <remarks>注意：会自动创建不存在的目录</remarks>
            public static bool CreateFile(string fileName)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(fileName);
                    if (fileInfo.Exists)
                    {
                        if (fileInfo.Attributes != FileAttributes.Normal)
                            fileInfo.Attributes = FileAttributes.Normal;
                        return true;
                    }
                    //创建目录
                    bool success = CreateDirectory(fileInfo.DirectoryName);
                    if (!success)
                        return false;
                    //创建文件
                    FileStream fileStream = fileInfo.Create();
                    fileStream.Close();
                    fileStream.Dispose();
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 将文本内容写入指定目录下的文件(gb2312编码).
            /// </summary>
            /// <param name="szFilePath">文件全路径</param>
            /// <param name="szTextData">文本数据</param>
            /// <returns>true:写入成功;false:写入失败</returns>
            /// <remarks>注意：会自动创建不存在的目录</remarks>
            public static bool WriteFileText(string szFilePath, string szTextData)
            {
                return WriteFileText(szFilePath, szTextData, Convert.GetDefaultEncoding());
            }

            /// <summary>
            /// 将文本内容写入指定目录下的文件
            /// </summary>
            /// <param name="szFilePath">文件全路径</param>
            /// <param name="szTextData">文本数据</param>
            /// <param name="encoding">字符编码</param>
            /// <returns>true:写入成功;false:写入失败</returns>
            /// <remarks>注意：会自动创建不存在的目录</remarks>
            public static bool WriteFileText(string szFilePath, string szTextData, Encoding encoding)
            {
                if (!CreateDirectory(GetFilePath(szFilePath)))
                    return false;
                try
                {
                    File.WriteAllText(szFilePath, szTextData, encoding);
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 将文本内容写入指定目录下的文件
            /// </summary>
            /// <param name="szFilePath">文件全路径</param>
            /// <param name="arrTextLines">文本数据</param>
            /// <param name="encoding">字符编码</param>
            /// <returns>true:写入成功;false:写入失败</returns>
            /// <remarks>注意：会自动创建不存在的目录</remarks>
            public static bool WriteFileText(string szFilePath, string[] arrTextLines, Encoding encoding)
            {
                if (!CreateDirectory(GetFilePath(szFilePath)))
                    return false;
                try
                {
                    File.WriteAllLines(szFilePath, arrTextLines, encoding);
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 读取指定的文本文件内容(gb2312编码)
            /// </summary>
            /// <param name="szFilePath">文件全路径</param>
            /// <param name="szTextData">文件数据</param>
            /// <returns>true:写入成功;false:写入失败</returns>
            public static bool GetFileText(string szFilePath, ref string szTextData)
            {
                return GetFileText(szFilePath, Convert.GetDefaultEncoding(), ref szTextData);
            }

            /// <summary>
            /// 读取指定的文本文件内容
            /// </summary>
            /// <param name="szFilePath">文件全路径</param>
            /// <param name="encoding">编码</param>
            /// <param name="szTextData">文件数据</param>
            /// <returns>true:写入成功;false:写入失败</returns>
            public static bool GetFileText(string szFilePath, Encoding encoding, ref string szTextData)
            {
                try
                {
                    szTextData = File.ReadAllText(szFilePath, encoding);
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 删除指定目录下的文件
            /// </summary>
            /// <param name="fileName">文件全路径</param>
            /// <returns>true:删除成功;false:删除失败</returns>
            public static bool DeleteFile(string fileName)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(fileName);
                    if (!fileInfo.Exists)
                        return true;
                    if (fileInfo.Attributes != FileAttributes.Normal)
                        fileInfo.Attributes = FileAttributes.Normal;
                    fileInfo.Delete();
                }
                catch (Exception ex)
                {
                   return false;
                }
                return true;
            }

            /// <summary>
            /// 删除指定目录下的文件
            /// </summary>
            /// <param name="fileName">文件全路径</param>
            /// <returns>true:删除成功;false:删除失败</returns>
            public static bool BackupFile(string fileName, string newFileName)
            {
                try
                {
                    System.IO.File.Copy(fileName, newFileName);
                }
                catch (Exception ex)
                {
                   return false;
                }
                return true;
            }

            /// <summary>
            /// 比较两个文件的长度大小是否相等
            /// </summary>
            /// <param name="szFileName1">文件全路径1</param>
            /// <param name="szFileName2">文件全路径2</param>
            /// <returns>true:相等;false:不相等</returns>
            public static bool IsFileLengthEqual(string szFileName1, string szFileName2)
            {
                try
                {
                    System.IO.FileInfo fileInfo1 = new System.IO.FileInfo(szFileName1);
                    System.IO.FileInfo fileInfo2 = new System.IO.FileInfo(szFileName2);
                    if (!fileInfo1.Exists || !fileInfo2.Exists)
                        return false;
                    return (fileInfo1.Length == fileInfo2.Length);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            /// <summary>
            /// 从文件全路径获取文件名称
            /// </summary>
            /// <param name="szFilePath">文件全路径</param>
            /// <param name="bHasExt">是否包含扩展名</param>
            /// <returns>文件名称</returns>
            public static string GetFileName(string szFilePath, bool bHasExt)
            {
                if (szFilePath == null)
                    return string.Empty;
                string szFileName = null;
                int index = szFilePath.LastIndexOf(Path.DirectorySeparatorChar);
                if (index < 0)
                    index = szFilePath.LastIndexOf(Path.AltDirectorySeparatorChar);
                if (index < 0)
                    szFileName = szFilePath;
                else
                    szFileName = szFilePath.Substring(++index);
                if (!bHasExt && !string.IsNullOrEmpty(szFileName))
                {
                    index = szFileName.LastIndexOf(".");
                    if (index > 0)
                        szFileName = szFileName.Substring(0, index);
                }
                return szFileName;
            }

            /// <summary>
            /// 从文件全路径获取文件父路径
            /// </summary>
            /// <param name="szFileFullPath">文件全路径</param>
            /// <returns>文件父路径</returns>
            public static string GetFilePath(string szFileFullPath)
            {
                if (string.IsNullOrEmpty(szFileFullPath))
                    return "";
                int index = szFileFullPath.LastIndexOf(Path.DirectorySeparatorChar);
                if (index < 0)
                    index = szFileFullPath.LastIndexOf(Path.AltDirectorySeparatorChar);
                return (index < 0) ? szFileFullPath : szFileFullPath.Substring(0, index);
            }

            /// <summary>
            /// 获取指定文件的上次修改时间
            /// </summary>
            /// <param name="fileName">文件全路径</param>
            /// <returns>true:获取成功;false:获取失败</returns>
            public static bool GetFileLastModifyTime(string fileName, ref DateTime dtLastModifyTime)
            {
                try
                {
                    System.IO.FileInfo fileInfo = new FileInfo(fileName);
                    if (!fileInfo.Exists)
                        return false;
                    dtLastModifyTime = fileInfo.LastWriteTime;
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            /// <summary>
            /// 从数据读取器中指定字段读取相应的字节内容
            /// </summary>
            /// <param name="reader">数据读取器</param>
            /// <param name="column">指定字段</param>
            /// <param name="byteData">字节内容</param>
            /// <returns>short</returns>
            public static bool GetBytes(System.Data.IDataReader reader, int column, ref byte[] byteData)
            {
                MemoryStream memoryStream = null;
                BinaryWriter binaryWriter = null;
                try
                {
                    if (reader.IsDBNull(column))
                    {
                        byteData = new byte[0];
                        return true;
                    }

                    memoryStream = new MemoryStream();
                    binaryWriter = new BinaryWriter(memoryStream);

                    int nStartIndex = 0;
                    int nBufferSize = 2048;
                    byte[] byteBuffer = new byte[nBufferSize];
                    long nRetLen = reader.GetBytes(column, 0, byteBuffer, 0, nBufferSize);
                    while (nRetLen == nBufferSize)
                    {
                        binaryWriter.Write(byteBuffer);
                        binaryWriter.Flush();

                        nStartIndex += nBufferSize;
                        nRetLen = reader.GetBytes(column, nStartIndex, byteBuffer, 0, nBufferSize);
                    }
                    binaryWriter.Write(byteBuffer, 0, (int)nRetLen);
                    binaryWriter.Flush();
                    byteBuffer = null;

                    byteData = memoryStream.ToArray();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (binaryWriter != null)
                    {
                        binaryWriter.Close();
                    }
                    if (memoryStream != null)
                    {
                        memoryStream.Close();
                        memoryStream.Dispose();
                    }
                }
            }

            /// <summary>
            /// 返回本地文件的字节内容信息
            /// </summary>
            /// <param name="szFileFullPath">本地文件全路径</param>
            /// <param name="byteFileData">文件的字节数据</param>
            /// <returns>bool</returns>
            public static bool GetFileBytes(string szFileFullPath, ref byte[] byteFileData)
            {
                FileInfo fileInfo = null;
                try
                {
                    fileInfo = new FileInfo(szFileFullPath);
                }
                catch (Exception ex)
                {
                    return false;
                }

                if (!fileInfo.Exists)
                {
                    return false;
                }

                try
                {
                    byteFileData = File.ReadAllBytes(szFileFullPath);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            /// <summary>
            /// 把字节数据写入指定文件
            /// </summary>
            /// <param name="szFileFullPath">本地文件全路径</param>
            /// <param name="byteFileData">文件字节数据</param>
            /// <returns>bool</returns>
            /// <remarks>注意：会自动创建不存在的目录</remarks>
            public static bool WriteFileBytes(string szFileFullPath, byte[] byteFileData)
            {
                if (byteFileData == null)
                    byteFileData = new byte[0];

                if (!CreateDirectory(GetFilePath(szFileFullPath)))
                    return false;
                try
                {
                    File.WriteAllBytes(szFileFullPath, byteFileData);
                    return true;
                }
                catch (Exception ex)
                {
                    
                    return false;
                }
            }
        }
    }
}
