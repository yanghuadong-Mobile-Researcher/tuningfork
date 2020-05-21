//-----------------------------------------------------------------------
// <copyright file="AnnotationMessageEditor.cs" company="Google">
//
// Copyright 2020 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

using UnityEditor;
using UnityEngine;
using Google.Android.PerformanceTuner.Editor.Proto;
using Google.Protobuf.Reflection;

namespace Google.Android.PerformanceTuner.Editor
{
    public class AnnotationMessageEditor : MessageEditor
    {
        const string k_BasicInfo = "       The scene is used as an annotation.\n" +
                                   "       The annotation is set automatically when active scene is changed.";

        const string k_SceneInfo =
            "This parameter is recognized as a scene parameter. " +
            "If your game is not using / not matching unity scene system, you can remove it from your annotation.";

        const string k_LoadingInfo =
            "This parameter is recognized as a loading parameter. " +
            "Loading annotations mark frames that are part of the level loading process. " +
            "It is highly recommended that you use loading annotations so that slower frames " +
            "while the game is loading do not affect your overall metrics.";

        readonly GUIContent m_SceneFieldInfo;

        readonly GUIContent m_LoadingFieldInfo;


        protected override string basicInfo
        {
            get { return k_BasicInfo; }
        }

        readonly string[] m_Headers = new string[2] {"Type", "Parameter name "};

        protected override string[] headers
        {
            get { return m_Headers; }
        }

        public AnnotationMessageEditor(SetupConfig config, FileInfo protoFile, MessageDescriptor descriptor,
            EnumInfoHelper enumInfoHelper) : base(
            config,
            descriptor,
            ProtoMessageType.Annotation,
            DefaultMessages.annotationMessage,
            protoFile,
            enumInfoHelper)
        {
            var icon = (Texture) Resources.Load("baseline_info_outline");
            m_SceneFieldInfo = new GUIContent(icon, k_SceneInfo);
            m_LoadingFieldInfo = new GUIContent(icon, k_LoadingInfo);
        }

        protected override FieldInfo RenderInfo(FieldInfo info, Rect rect, int index)
        {
            Rect infoRect = new Rect(rect);
            infoRect.width = 20;
            // Align icon on the center.
            infoRect.x--;
            infoRect.y--;

            Rect enumRect = new Rect(rect);
            enumRect.x = infoRect.xMax;
            enumRect.width = rect.width / 3 - infoRect.width;

            Rect textRect = new Rect(rect);
            textRect.x = enumRect.xMax;
            textRect.width = rect.width - enumRect.width - infoRect.width;

            if (DefaultMessages.IsSceneField(info))
            {
                EditorGUI.LabelField(infoRect, m_SceneFieldInfo);
            }
            else if (DefaultMessages.IsLoadingStateField(info))
            {
                EditorGUI.LabelField(infoRect, m_LoadingFieldInfo);
            }

            Selection selection = RenderEnumSelection(enumRect, info.enumTypeIndex);
            info.enumTypeIndex = selection.id;
            info.enumType = selection.name;
            info.name = EditorGUI.TextField(textRect, info.name);
            return info;
        }

        protected override void ApplyState(EditorState state)
        {
            // Nothing to apply for annotation message.
        }
    }
}