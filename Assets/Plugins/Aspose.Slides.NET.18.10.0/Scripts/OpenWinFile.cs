using System;
using System.Runtime.InteropServices;

/// <summary>
/// 打开 Window 文件窗口
/// </summary>
public class OpenWinFile
{
    /// <summary>
    /// 数据接收类
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private class FileDialogConfig
    {
        /// <summary>
        /// 设置窗口大小
        /// </summary>
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;

        /// <summary>
        /// 文件筛选
        /// </summary>
        public string filter = null;
        public string customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;

        /// <summary>
        /// 文件全名
        /// </summary>
        public string file = null;
        public int maxFile = 0;
        /// <summary>
        /// 文件名
        /// </summary>
        public string fileTitle = null;
        public int maxFileTitle = 0;

        /// <summary>
        /// 指定路径
        /// </summary>
        public string initialDir = null;
        /// <summary>
        /// 窗口名称
        /// </summary>
        public string title = null;

        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;

        public string defExt = null;
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public string templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }

    /// <summary>
    /// 系统函数调用类
    /// </summary>
    private class LocalDialog
    {
        //链接指定系统函数       打开文件对话框
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool GetOpenFileName([In, Out] FileDialogConfig ofn);

        /// <summary>
        /// 打开文件
        /// </summary>
        public static bool GetOFN([In, Out] FileDialogConfig ofn)
        {
            return GetOpenFileName(ofn);
        }

        //链接指定系统函数        另存为对话框
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool GetSaveFileName([In, Out] FileDialogConfig ofn);

        /// <summary>
        /// 保存文件
        /// </summary>
        public static bool GetSFN([In, Out] FileDialogConfig ofn)
        {
            return GetSaveFileName(ofn);
        }
    }

    /// <summary>
    /// 选择文件
    /// </summary>
    /// <returns>选择文件的全名</returns>
    public static string ChooseWinFile()
    {
        FileDialogConfig openFileName = new FileDialogConfig();
        openFileName.structSize = Marshal.SizeOf(openFileName);

        // 这里设置文件类型。可以指定视频，图片等其他文件。*.*默认为所有文件
        openFileName.filter = "PPT文件(*.ppt/*.pptx)\0*.ppt*";

        // 文件原始路径，格式（D:\3-图片-花.jpg）
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;

        // 文件名字(带格式后缀)，格式（3-图片-花.jpg）
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;

        // 默认路径
        openFileName.initialDir = "D:";
        openFileName.title = "请选择 PPT 文件：";

        // 注意 一下项目不一定要全选 但是0x00000008项不要缺少
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (LocalDialog.GetOFN(openFileName))
        {
            return openFileName.file;
        }

        return null;
    }

}
