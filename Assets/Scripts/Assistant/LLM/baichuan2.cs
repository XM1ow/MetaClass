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
    /// ������Ϣ
    /// </summary>
    /// <returns></returns>
    public override void PostMsg(string _msg, Action<string> _callback)
    {
        //���淢�͵���Ϣ�б�
        m_DataList.Add(new SendData("user", _msg));
        StartCoroutine(Request(_msg, _callback));
    }

    /// <summary>
    /// ��������
    /// </summary> 
    /// <param name="_postWord"></param>
    /// <param name="_callback"></param>
    /// <returns></returns>
    public override IEnumerator Request(string _postWord, System.Action<string> _callback)
    {
        stopwatch.Restart();  //��ʱ��ʼ

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
            //�����ϴ�/����׼��
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            //����SSE��ʽ
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "text/event-stream");

            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("�յ��ظ����ݣ�" + responseText);
                string[] _msgBack = responseText.Split(new string[] { "\", \"docs\":" }, StringSplitOptions.RemoveEmptyEntries);
                _msgBack[0] = _msgBack[0].Remove(0, 18);
                //��Ӽ�¼
                m_DataList.Add(new SendData("assistant", _msgBack[0]));
                //�ص�
                _callback(_msgBack[0]);
                //HandleSSEResponse(responseText,_callback);
            }
            else
            {
                Debug.Log(request.error);
            }

        }

        stopwatch.Stop();  //��ʱ����
        Debug.Log("baichuan2�ظ���ʱ��" + stopwatch.Elapsed.TotalSeconds);
    }

    #region ���ݶ���
    /// <summary>
    /// ��������
    /// </summary>
    [Serializable] public class RequestData
    {
        public string query;  //��������
        public string knowledge_base_name;  //֪ʶ��ѡ��
        public int top_k;  //top_k����
        public float score_threshold;  //֪ʶ��ƥ����ض���ֵ
        public List<List<string>> history;  //��ʷ�Ի�
        public bool stream;  //��ʽ���
        public string model_name;  //��ģ��ѡ��
        public float temperature;  //LLM�����¶�
        public int max_tokens;  //����Token����
        public string prompt_name;  //promptģ������
    }

    #endregion

}
