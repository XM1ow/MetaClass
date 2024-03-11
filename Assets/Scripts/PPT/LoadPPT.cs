using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//※※※※ 别忘了在PlayerSetting中改成.net4.x的api ※※※※
public class LoadPPT : MonoBehaviour
{
    public Transform content;
    [SerializeField] Image prefab;

    [SerializeField] Button firstPageBtn;   //第一个按钮
    [SerializeField] Button lastPageBtn;   //第一个按钮
    [SerializeField] Button toPrePageBtn;   //上一页按钮
    [SerializeField] Button toNestPageBtn;   //下一页按钮
    [SerializeField] Button loadBtn;   //下一页按钮

    public NetworkSlidesSyncManager slidesSyncManager;
    private void Start ()
    {
        //按钮监听
        firstPageBtn.onClick.AddListener(FirstPage);
        lastPageBtn.onClick.AddListener(LastPage);
        toPrePageBtn.onClick.AddListener(ToPrePage);
        toNestPageBtn.onClick.AddListener(ToNextPage);
        loadBtn.onClick.AddListener(Load);
        // network sync components init
        slidesSyncManager = GetComponent<NetworkSlidesSyncManager>();
        slidesSyncManager.localSlidesPlayer = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) FirstPage();
        if (Input.GetKeyDown(KeyCode.Alpha2)) LastPage();
        if (Input.GetKeyDown(KeyCode.Alpha3)) ToPrePage();
        if (Input.GetKeyDown(KeyCode.Alpha4)) ToNextPage();
        if (Input.GetKeyDown(KeyCode.Alpha5)) Load();
    }

    private List<GameObject> _slideGameObjects = new ();
    private const int BUFFER_SIZE = 4096;
    private List<byte> _byteBuffer = new List<byte>();
    void Load ()
    {
        if (IsInvoking("AutoPlayImage"))
            CancelInvoke("AutoPlayImage");
        string pptPath = OpenWinFile.ChooseWinFile();
        if (pptPath == null) return;
        //读取ppt文件
        var presentation = new Aspose.Slides.Presentation(pptPath);
        //遍历文档（只做示例使用自己根据需求拓展）
        for (int i = 0; i < presentation.Slides.Count; i++)
        {
            var slide = presentation.Slides[i];
            var bitmap = slide.GetThumbnail(1f, 1f);
            byte[] bytes = Bitmap2Byte(bitmap);
            _currentReadingProcess = 0;
            if (NetworkManager.singleton.isNetworkActive)
            {                    
                int bufferIndex = 0;
                var length = bytes.Length;
                foreach (var b in bytes)
                {
                    if (_currentReadingProcess < length)
                    {
                        _byteBuffer.Add(b);
                        _currentReadingProcess++;
                        bufferIndex++;
                    }
                    if(bufferIndex >= BUFFER_SIZE || _currentReadingProcess >= length)
                    {
                        // send one package
                        slidesSyncManager.CmdSendBytePackage(_byteBuffer.ToArray(), i, _currentReadingProcess >= bytes.Length);
                        _byteBuffer.Clear();
                        bufferIndex = 0;
                    }
                }
            }
            else
            {
                foreach (var b in bytes)
                {
                    _byteTemp.Add(b);
                }
                MakeSlidePrefab(i);
            }
        }
        if(NetworkManager.singleton.isNetworkActive)
            slidesSyncManager.CmdShowFirstPage();
        else
            FirstPage();
    }

    private List<byte> _byteTemp = new ();
    private int _currentReadingProcess;

    public void ReadBufferToTemp(byte[] buffer)
    {
        foreach (var b in buffer)
        {
            _byteTemp.Add(b);
        }
    }
    public void MakeSlidePrefab(int index)
    {
        if (_byteTemp.Count < _currentReadingProcess)
        {
            Debug.LogError($"byte temp length is {_byteTemp.Count} and currentReadingProcess is {_currentReadingProcess}, Package Lost!");
        }
        Debug.Log($"Building slide byte length {_byteTemp.Count}");
        GameObject slideGameObject;
        if (index < _slideGameObjects.Count)
        {
            slideGameObject = _slideGameObjects[index];
        }
        else
        {
            slideGameObject = Instantiate(prefab, content).gameObject;
            _slideGameObjects.Add(slideGameObject);
        }
        var showImage = slideGameObject.GetComponent<Image>();
        int width = 960, height = 540;
        Texture2D texture2D = new Texture2D(width, height);
        texture2D.LoadImage(_byteTemp.ToArray());
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, width, height), Vector2.zero);
        showImage.sprite = sprite;
        _byteTemp.Clear();
    }

    //将ppt转为图片数据
    public byte[] Bitmap2Byte (System.Drawing.Bitmap bitmap)
    {
        //方式2：
        using (MemoryStream stream = new MemoryStream())
        {
            bitmap.Save(stream, ImageFormat.Png);
            byte[] data = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(data, 0, Convert.ToInt32(stream.Length));
            return data;
        }
    }

    #region 首尾页

    //显示首页
    public void FirstPage()
    {
        if (IsInvoking("AutoPlayImage"))
            CancelInvoke("AutoPlayImage");
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(0).gameObject.SetActive(true);
            if (i!=0)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    //显示尾页
    void LastPage()
    {
        if (IsInvoking("AutoPlayImage"))
            CancelInvoke("AutoPlayImage");
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(content.childCount-1).gameObject.SetActive(true);
            if (i != content.childCount - 1)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region 上一张，下一张

    int index = 0;    //计数
    //上一张
    void ToPrePage()
    {
        if (IsInvoking("AutoPlayImage"))
            CancelInvoke("AutoPlayImage");
        index--;
        if (index <= 0)
            index = 0;
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(index).gameObject.SetActive(true);
            if (i !=index)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    //下一张
    void ToNextPage()
    {
        if (IsInvoking("AutoPlayImage"))
            CancelInvoke("AutoPlayImage");
        index++;
        if (index >= content.childCount - 1)
            index = content.childCount-1;
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(index).gameObject.SetActive(true);
            if (i != index)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    #endregion
}