using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using ImGuiNET;
using MathStuff;
using MathStuff.vectors;

namespace Quartz.ui.imgui;

//TODO: add other managed fields serialization
//TODO: add string field serialization
public static class ImGuiElements {
	public static bool ButtonColored(string txt, color col) {
		color texCol = col.WithLightness(col.lightness * 1.3f);
		float opacity = col.aF;
		
		ImGui.PushStyleColor(ImGuiCol.Button, col.WithAlpha((byte)(140 * opacity)).ToVec4());
		ImGui.PushStyleColor(ImGuiCol.ButtonHovered, col.WithAlpha((byte)(210 * opacity)).ToVec4());
		ImGui.PushStyleColor(ImGuiCol.ButtonActive, col.WithAlpha((byte)(255 * opacity)).ToVec4());
		ImGui.PushStyleColor(ImGuiCol.Text, texCol.ToVec4());
		bool b = ImGui.Button(txt);
		ImGui.PopStyleColor(4);
		return b;
	}
	
	public static bool ButtonYellow(string txt, float opacity = 1) {
		ImGui.PushStyleColor(ImGuiCol.Button, ((color)"#f59b42aa").WithAlpha((byte)(150 * opacity)).ToVec4());
		ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ((color)"#f59b42ee").WithAlpha((byte)(230 * opacity)).ToVec4());
		ImGui.PushStyleColor(ImGuiCol.Text, "#ffbf80".ToVec4());
		bool b = ImGui.Button(txt);
		ImGui.PopStyleColor(3);
		return b;
	}
	
	public static bool ButtonPurple(string txt, float opacity = 1) {
		ImGui.PushStyleColor(ImGuiCol.Button, ((color)"#7842f5aa").WithAlpha((byte)(150 * opacity)).ToVec4());
		ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ((color)"#7842f5ee").WithAlpha((byte)(230 * opacity)).ToVec4());
		ImGui.PushStyleColor(ImGuiCol.Text, "#c1a6ff".ToVec4());
		bool b = ImGui.Button(txt);
		ImGui.PopStyleColor(3);
		return b;
	}
	
	public static bool ButtonRed(string txt, float opacity = 1) {
		ImGui.PushStyleColor(ImGuiCol.Button, ((color)"#f54263aa").WithAlpha((byte)(150 * opacity)).ToVec4());
		ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ((color)"#f54263ee").WithAlpha((byte)(230 * opacity)).ToVec4());
		ImGui.PushStyleColor(ImGuiCol.Text, "#ffa6ad".ToVec4());
		bool b = ImGui.Button(txt);
		ImGui.PopStyleColor(3);
		return b;
	}
	
	public static bool ButtonGray(string txt, float opacity = 1) {
		ImGui.PushStyleColor(ImGuiCol.Button, ((color)"#464a57aa").WithAlpha((byte)(150 * opacity)).ToVec4());
		ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ((color)"#464a57ee").WithAlpha((byte)(230 * opacity)).ToVec4());
		ImGui.PushStyleColor(ImGuiCol.Text, "#bfccf5".ToVec4());
		bool b = ImGui.Button(txt);
		ImGui.PopStyleColor(3);
		return b;
	}
	
	public static unsafe void SerializeFields(Type type, void* objPtr, bool isReadonly = false) {
		if (isReadonly) ImGui.BeginDisabled();
		FieldInfo[] fields = type.GetFields();
		for (int i = 0; i < fields.Length; i++) {
			SerializeField(fields[i], type, fields[i].GetHashCode(), objPtr);
		}
		if (isReadonly) ImGui.EndDisabled();
	}
	
	public static unsafe void SerializeField(FieldInfo field, Type type, int id, void* objPtr) {
		int offset = (int)Marshal.OffsetOf(type, field.Name);
		Type fType = field.FieldType;
		objPtr = (byte*)objPtr + offset;
		
		if (fType == typeof(bool)) SerializeBool(field.Name, id, (bool*) objPtr);
		else if (fType == typeof(float)) SerializeFloat(field.Name, id, (float*) objPtr);
		else if (fType == typeof(float2) || fType == typeof(Vector2)) SerializeFloat2(field.Name, id, (Vector2*) objPtr);
		else if (fType == typeof(float3) || fType == typeof(Vector3)) SerializeFloat3(field.Name, id, (Vector3*) objPtr);
		else if (fType == typeof(float4) || fType == typeof(Vector4)) SerializeFloat4(field.Name, id, (Vector4*) objPtr);
		else if (fType == typeof(int)) SerializeInt(field.Name, id, (int*) objPtr);
		else if (fType == typeof(byte)) SerializeScalar(field.Name, id, objPtr, ImGuiDataType.U8);
		else if (fType == typeof(ushort)) SerializeScalar(field.Name, id, objPtr, ImGuiDataType.U16);
		else if (fType == typeof(uint)) SerializeScalar(field.Name, id, objPtr, ImGuiDataType.U32);
		else if (fType == typeof(ulong)) SerializeScalar(field.Name, id, objPtr, ImGuiDataType.U64);
		else if (fType == typeof(sbyte)) SerializeScalar(field.Name, id, objPtr, ImGuiDataType.S8);
		else if (fType == typeof(short)) SerializeScalar(field.Name, id, objPtr, ImGuiDataType.S16);
		else if (fType == typeof(long)) SerializeScalar(field.Name, id, objPtr, ImGuiDataType.S64);
		else if (fType == typeof(double)) SerializeScalar(field.Name, id, objPtr, ImGuiDataType.Double);
		else if (fType.IsEnum) SerializeEnum(field.Name, id, fType, objPtr);
	}

	public static unsafe void SerializeField(FieldInfo field, int id, object? owner) {
		Type fType = field.FieldType;

		if (fType == typeof(bool)) {
			bool b = (bool) field.GetValue(owner)!;
			if (SerializeBool(field.Name, id, ref b)) field.SetValue(owner, b);
		} 
		else if (fType == typeof(float)) {
			float b = (float) field.GetValue(owner)!;
			if (SerializeFloat(field.Name, id, ref b)) field.SetValue(owner, b);
		}
	}

	public static unsafe void SerializeEnum(string display, int id, Type type, void* ptr) {
		int sizeBytes = Marshal.SizeOf(Enum.GetUnderlyingType(type));
		ImGui.PushID(id);
		
		long cur = Convert.ToInt64(Marshal.PtrToStructure((IntPtr) ptr, Enum.GetUnderlyingType(type))!);
		
		ImGui.Text(display);
		ImGui.SameLine();
		if (ImGui.Button(Enum.ToObject(type, cur).ToString())) ImGui.OpenPopup(display);
		if (ImGui.BeginPopup(display))
		{
			Array values = Enum.GetValues(type);
			for (int i = 0; i < values.Length; i++) {
				ImGui.PushID(id + i + 1);
				if (ImGui.Selectable(values.GetValue(i)!.ToString())) {
					long v = Convert.ToInt64(values.GetValue(i)!);
					switch (sizeBytes) {
						case 1: *(byte*)ptr = (byte)v; break;
						case 2: *(ushort*)ptr = (ushort)v; break;
						case 4: *(uint*)ptr = (uint)v; break;
						case 8: *(long*)ptr = v; break;
					}
				}
				ImGui.PopID();
			}
			ImGui.EndPopup();
		}
		ImGui.PopID();
	}

	public static unsafe void SerializeBool(string display, int id, bool* ptr) {
		ImGui.PushID(id);
		ImGui.Text(display);
		ImGui.SameLine();
		ImGui.Checkbox("", ref *ptr);
		ImGui.PopID();
	}
	
	public static bool SerializeBool(string display, int id, ref bool ptr) {
		ImGui.PushID(id);
		ImGui.Text(display);
		ImGui.SameLine();
		bool b = ImGui.Checkbox("", ref ptr);
		ImGui.PopID();
		return b;
	}
	
	public static unsafe void SerializeFloat(string display, int id, float* ptr, float min = float.MinValue, float max = float.MaxValue) {
		ImGui.PushID(id);
		ImGui.Text(display);
		ImGui.SameLine();
		ImGui.DragFloat("", ref *ptr, .25f, min, max);
		ImGui.PopID();
	}
	
	public static bool SerializeFloat(string display, int id, ref float ptr, float min = float.MinValue, float max = float.MaxValue) {
		ImGui.PushID(id);
		ImGui.Text(display);
		ImGui.SameLine();
		bool b = ImGui.DragFloat("", ref ptr, .25f, min, max);
		ImGui.PopID();
		return b;
	}
	
	public static unsafe void SerializeFloat2(string display, int id, Vector2* ptr, float min = float.MinValue, float max = float.MaxValue) {
		ImGui.PushID(id);
		ImGui.Text(display);
		ImGui.SameLine();
		ImGui.DragFloat2("", ref *ptr, .25f, min, max);
		ImGui.PopID();
	}
	
	public static unsafe void SerializeFloat3(string display, int id, Vector3* ptr, float min = float.MinValue, float max = float.MaxValue) {
		ImGui.PushID(id);
		ImGui.Text(display);
		ImGui.SameLine();
		ImGui.DragFloat3("", ref *ptr, .25f, min, max);
		ImGui.PopID();
	}
	
	public static unsafe void SerializeFloat4(string display, int id, Vector4* ptr, float min = float.MinValue, float max = float.MaxValue) {
		ImGui.PushID(id);
		ImGui.Text(display);
		ImGui.SameLine();
		ImGui.DragFloat4("", ref *ptr, .25f, min, max);
		ImGui.PopID();
	}
	
	public static unsafe void SerializeInt(string display, int id, int* ptr, int min = int.MinValue, int max = int.MaxValue) {
		ImGui.PushID(id);
		ImGui.Text(display);
		ImGui.SameLine();
		ImGui.DragInt("", ref *ptr, .25f, min, max);
		ImGui.PopID();
	}
	
	public static unsafe void SerializeScalar(string display, int id, void* ptr, ImGuiDataType type, int min = int.MinValue, int max = int.MaxValue) {
		ImGui.PushID(id);
		ImGui.Text(display);
		ImGui.SameLine();
		ImGui.DragScalar("", type, (IntPtr)ptr, .25f);
		ImGui.PopID();
	}
	
	public static unsafe void SerializeVector(string display, int id, void* ptr, ImGuiDataType type, int vecLen, int min = int.MinValue, int max = int.MaxValue) {
		ImGui.PushID(id);
		ImGui.Text(display);
		ImGui.SameLine();
		ImGui.DragScalarN("", type, (IntPtr)ptr, vecLen, .25f, (IntPtr) min, (IntPtr) max);
		ImGui.PopID();
	}

	public static void Tooltip(string text) {
		if (!ImGui.IsItemHovered()) return;
		ImGui.BeginTooltip();
		ImGui.Text(text);
		ImGui.EndTooltip();
	}
	
	public static bool BeginTooltip() {
		if (!ImGui.IsItemHovered()) return false;
		ImGui.BeginTooltip();
		return true;
	}
	
	public static void EndTooltip() {
		ImGui.EndTooltip();
	}
}