using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChatSetting
{
    /// <summary>
    /// ��ģ��
    /// </summary>
    [Header("������Ҫ���ز�ͬ��llm�ű�")]
    [SerializeField] public LLM m_ChatModel;
    /// <summary>
    /// TTS�����ϳɷ���
    /// </summary>
    [Header("�����ϳɽű�")]
    public TTS m_TextToSpeech;
}
