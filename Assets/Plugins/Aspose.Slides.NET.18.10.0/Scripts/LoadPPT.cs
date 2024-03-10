using System;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;
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
    private void Start ()
    {
        //按钮监听
        firstPageBtn.onClick.AddListener(FirstPage);
        lastPageBtn.onClick.AddListener(LastPage);
        toPrePageBtn.onClick.AddListener(ToPrePage);
        toNestPageBtn.onClick.AddListener(ToNextPage);
        loadBtn.onClick.AddListener(Load);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) FirstPage();
        if (Input.GetKeyDown(KeyCode.Alpha2)) LastPage();
        if (Input.GetKeyDown(KeyCode.Alpha3)) ToPrePage();
        if (Input.GetKeyDown(KeyCode.Alpha4)) ToNextPage();
        if (Input.GetKeyDown(KeyCode.Alpha5)) Load();
    }
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
            Instantiate(prefab, content);
            if (i < content.childCount)
            {
                var slide = presentation.Slides[i];
                var bitmap = slide.GetThumbnail(1f, 1f);
                byte[] bytes = Bitmap2Byte(bitmap);

                var showImage = content.GetChild(i).GetComponent<UnityEngine.UI.Image>();
                int width = 960, height = 540;
                Texture2D texture2D = new Texture2D(width, height);
                texture2D.LoadImage(bytes);
                Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, width, height), Vector2.zero);
                showImage.sprite = sprite;
            }
        }
        //改成自己的文件路径
        FirstPage();
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
    void FirstPage()
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