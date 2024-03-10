using System;
using System.Runtime.InteropServices;

/// <summary>
/// �� Window �ļ�����
/// </summary>
public class OpenWinFile
{
    /// <summary>
    /// ���ݽ�����
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private class FileDialogConfig
    {
        /// <summary>
        /// ���ô��ڴ�С
        /// </summary>
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;

        /// <summary>
        /// �ļ�ɸѡ
        /// </summary>
        public string filter = null;
        public string customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;

        /// <summary>
        /// �ļ�ȫ��
        /// </summary>
        public string file = null;
        public int maxFile = 0;
        /// <summary>
        /// �ļ���
        /// </summary>
        public string fileTitle = null;
        public int maxFileTitle = 0;

        /// <summary>
        /// ָ��·��
        /// </summary>
        public string initialDir = null;
        /// <summary>
        /// ��������
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
    /// ϵͳ����������
    /// </summary>
    private class LocalDialog
    {
        //����ָ��ϵͳ����       ���ļ��Ի���
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool GetOpenFileName([In, Out] FileDialogConfig ofn);

        /// <summary>
        /// ���ļ�
        /// </summary>
        public static bool GetOFN([In, Out] FileDialogConfig ofn)
        {
            return GetOpenFileName(ofn);
        }

        //����ָ��ϵͳ����        ���Ϊ�Ի���
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool GetSaveFileName([In, Out] FileDialogConfig ofn);

        /// <summary>
        /// �����ļ�
        /// </summary>
        public static bool GetSFN([In, Out] FileDialogConfig ofn)
        {
            return GetSaveFileName(ofn);
        }
    }

    /// <summary>
    /// ѡ���ļ�
    /// </summary>
    /// <returns>ѡ���ļ���ȫ��</returns>
    public static string ChooseWinFile()
    {
        FileDialogConfig openFileName = new FileDialogConfig();
        openFileName.structSize = Marshal.SizeOf(openFileName);

        // ���������ļ����͡�����ָ����Ƶ��ͼƬ�������ļ���*.*Ĭ��Ϊ�����ļ�
        openFileName.filter = "PPT�ļ�(*.ppt/*.pptx)\0*.ppt*";

        // �ļ�ԭʼ·������ʽ��D:\3-ͼƬ-��.jpg��
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;

        // �ļ�����(����ʽ��׺)����ʽ��3-ͼƬ-��.jpg��
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;

        // Ĭ��·��
        openFileName.initialDir = "D:";
        openFileName.title = "��ѡ�� PPT �ļ���";

        // ע�� һ����Ŀ��һ��Ҫȫѡ ����0x00000008�Ҫȱ��
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (LocalDialog.GetOFN(openFileName))
        {
            return openFileName.file;
        }

        return null;
    }

}
