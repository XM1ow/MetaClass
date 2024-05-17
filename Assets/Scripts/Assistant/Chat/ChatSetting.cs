using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChatSetting
{
    /// <summary>
    /// 大模型
    /// </summary>
    [Header("根据需要挂载不同的llm脚本")]
    [SerializeField] public LLM m_ChatModel;
    /// <summary>
    /// TTS语音合成服务
    /// </summary>
    [Header("语音合成脚本")]
    public TTS m_TextToSpeech;
}
