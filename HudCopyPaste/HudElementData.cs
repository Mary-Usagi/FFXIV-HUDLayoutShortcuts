﻿using FFXIVClientStructs.FFXIV.Component.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace HudCopyPaste {
    /// <summary>
    /// Represents data for a HUD element.
    /// </summary>
    public class HudElementData {
        public int ElementId { get; set; } = -1;
        public string ResNodeDisplayName { get; set; } = string.Empty;
        public string AddonName { get; set; } = string.Empty;
        public short PosX { get; set; } = 0;
        public short PosY { get; set; } = 0;
        public float Scale { get; set; } = 1.0f;
        public int Timestamp { get; set; } = Environment.TickCount;

        public override string ToString() => JsonSerializer.Serialize(this);
        public string PrettyPrint() => $"{ResNodeDisplayName} ({PosX}, {PosY})";

        public HudElementData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HudElementData"/> class from an AtkResNode.
        /// </summary>
        /// <param name="resNode">The AtkResNode pointer.</param>
        internal unsafe HudElementData(AtkResNode* resNode) {
            if (resNode->ParentNode == null) return;
            try {
                ResNodeDisplayName = resNode->ParentNode->GetComponent()->GetTextNodeById(4)->GetAsAtkTextNode()->NodeText.ToString();
            } catch (NullReferenceException) {
                ResNodeDisplayName = "Unknown";
            }
            PosX = resNode->ParentNode->GetXShort();
            PosY = resNode->ParentNode->GetYShort();

            // TODO: Maybe get corresponding addon?
            ElementId = -1;
            AddonName = "";
            Scale = -1;
        }

        internal unsafe static bool HasSameName(AtkResNode* resNode, HudElementData? hudElementData) {
            if (hudElementData == null) return false;
            if (resNode->ParentNode == null) return false;
            try {
                string resNodeDisplayName = resNode->ParentNode->GetComponent()->GetTextNodeById(4)->GetAsAtkTextNode()->NodeText.ToString();
                return resNodeDisplayName == hudElementData.ResNodeDisplayName; // &&
                    //resNode->ParentNode->GetXShort() == hudElementData.PosX &&
                    //resNode->ParentNode->GetYShort() == hudElementData.PosY;
            } catch (NullReferenceException) {
                return false;
            }
        }

        internal unsafe static bool OnlySameName(AtkResNode* resNode, HudElementData? hudElementData) {
            if (hudElementData == null) return false;
            if (resNode->ParentNode == null) return false;
            try {
                string resNodeDisplayName = resNode->ParentNode->GetComponent()->GetTextNodeById(4)->GetAsAtkTextNode()->NodeText.ToString();
                return resNodeDisplayName == hudElementData.ResNodeDisplayName &&
                    (resNode->ParentNode->GetXShort() != hudElementData.PosX ||
                    resNode->ParentNode->GetYShort() != hudElementData.PosY);
            } catch (NullReferenceException) {
                return false;
            }
        }

        internal unsafe static bool IsEqual(AtkResNode* resNode, HudElementData? hudElementData) {
            if (hudElementData == null) return false;
            if (resNode->ParentNode == null) return false;
            try {
                string resNodeDisplayName = resNode->ParentNode->GetComponent()->GetTextNodeById(4)->GetAsAtkTextNode()->NodeText.ToString();
                return resNodeDisplayName == hudElementData.ResNodeDisplayName && 
                    resNode->ParentNode->GetXShort() == hudElementData.PosX &&
                    resNode->ParentNode->GetYShort() == hudElementData.PosY;
            } catch (NullReferenceException) {
                return false;
            }
        }
    }
}