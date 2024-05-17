using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class baichuan2 : LLM
{

    [SerializeField] private string knowledgeBaseName = "test";
    [SerializeField] private int topK = 3;
    [SerializeField] private float scoreThreshold = 1;
    [SerializeField] private List<List<string>> history = new List<List<string>>();
    [SerializeField] private bool stream = false;
    [SerializeField] private string modelName = "baichuan2-13b-chat";
    [SerializeField] private float temperature = 0.7f;
    [SerializeField] private int maxTokens = 0;
    [SerializeField] private string promptName = "default";

    public baichuan2()
    {
        url = "http://localhost:7861/chat/knowledge_base_chat";
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <returns></returns>
    public override void PostMsg(string _msg, Action<string> _callback)
    {
        //缓存发送的信息列表
        m_DataList.Add(new SendData("user", _msg));
        StartCoroutine(Request(_msg, _callback));
    }

    /// <summary>
    /// 发送数据
    /// </summary> 
    /// <param name="_postWord"></param>
    /// <param name="_callback"></param>
    /// <returns></returns>
    public override IEnumerator Request(string _postWord, System.Action<string> _callback)
    {
        stopwatch.Restart();  //计时开始

        string jsonPayload = JsonConvert.SerializeObject(new RequestData
        {
            query = _postWord,
            knowledge_base_name = knowledgeBaseName,
            top_k = topK,
            score_threshold = scoreThreshold,
            history = history,
            stream = false,
            model_name = modelName,
            temperature = temperature,
            max_tokens = maxTokens,
            prompt_name = promptName
        });


        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            //数据上传/下载准备
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            //接收SSE格式
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "text/event-stream");

            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("收到回复数据：" + responseText);
                string[] _msgBack = responseText.Split(new string[] { "\", \"docs\":" }, StringSplitOptions.RemoveEmptyEntries);
                _msgBack[0] = _msgBack[0].Remove(0, 18);
                //添加记录
                m_DataList.Add(new SendData("assistant", _msgBack[0]));
                //回调
                _callback(_msgBack[0]);
                //HandleSSEResponse(responseText,_callback);
            }
            else
            {
                Debug.Log(request.error);
            }

        }

        stopwatch.Stop();  //计时结束
        Debug.Log("baichuan2回复耗时：" + stopwatch.Elapsed.TotalSeconds);
    }

    #region 数据定义
    /// <summary>
    /// 发送数据
    /// </summary>
    [Serializable] public class RequestData
    {
        public string query;  //提问内容
        public string knowledge_base_name;  //知识库选择
        public int top_k;  //top_k个数
        public float score_threshold;  //知识库匹配相关度阈值
        public List<List<string>> history;  //历史对话
        public bool stream;  //流式输出
        public string model_name;  //大模型选用
        public float temperature;  //LLM采样温度
        public int max_tokens;  //限制Token数量
        public string prompt_name;  //prompt模板名称
    }

    #endregion

}
