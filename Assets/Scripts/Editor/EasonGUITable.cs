using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using UnityEngine;

//using Tree = Sirenix.Utilities.Editor.
using Object = UnityEngine.Object;

public class EasonGUITable<T> where T: struct
{
    private PropertyTree _tree;
    private string _dataPropertyPath;
    private string[] _columnPropertyNames;
    private bool[] _disableds;
    private bool[] _flexibles;

    private GUIStyle _style;
    private GUITable _table;

    public string[] columnPropertyNames { get => _columnPropertyNames; }

    public EasonGUITable(PropertyTree tree, string dataPropertyPath, string[] columnPropertyNames)
    {
        _tree = tree;
        _dataPropertyPath = dataPropertyPath;
        _columnPropertyNames = columnPropertyNames;
        _disableds = new bool[columnPropertyNames.Length];
        _flexibles = new bool[columnPropertyNames.Length];
    }
    public void Initialize(int rowCount, string columnLabel = null)
    {
        var colCount = columnPropertyNames.Length;
        _table = GUITable.Create(
            colCount: colCount,
            rowCount: rowCount,
            drawElement: DrawTableElement,
            columnLabel,
            columnLabels: DrawColumnLabel,
            null,
            null, resizable: false);

        var minWidths = new float[colCount];
        _style = new GUIStyle();
        _style.normal.textColor = Color.white;
        _style.alignment = TextAnchor.MiddleCenter;
        for (int i = 0; i < colCount; i++)
        {
            _style.CalcMinMaxWidth(new GUIContent(GetColumnTitle(i) + "    "), out minWidths[i], out var _);
        }
        for (int column = 0; column < colCount; column++)
        {
            var width = _flexibles[column] ? 0 : minWidths[column];
            SetColumnSize(_table, column, minWidths[column], width);
        }
        _table.ReCalculateSizes();
    }
    public void SetDisabled(int column, bool value)
    {
        _disableds[column] = value;
    }
    public void SetFlexibles(int column, bool value)
    {
        _flexibles[column] = value;
    }
    public void SetPropertyName(int column, string value)
    {
        _columnPropertyNames[column] = value;
    }
    private void DrawColumnLabel(Rect rect, int column)
    {
        GUI.Label(rect, GetColumnTitle(column), _style);
    }

    private string GetColumnTitle(int column)
    {

        var dummy = new T();
        var tree = PropertyTree.Create(dummy);
        var property = tree.GetPropertyAtPath($"{columnPropertyNames[column]}");
        return property.NiceName;
    }
    private void DrawTableElement(Rect rect, int column, int row)
    {
        var enabled = GUI.enabled;
        GUI.enabled = column != 0;
        var inspectorProperty = GetMappedProperty(column, row);
        inspectorProperty.ValueEntry.WeakSmartValue = DrawValue(rect, GUIContent.none, inspectorProperty.ValueEntry.WeakSmartValue, inspectorProperty.ValueEntry.TypeOfValue);
        GUI.enabled = enabled;
    }

    InspectorProperty GetMappedProperty(int column, int row)
    {
        return _tree.GetPropertyAtPath($"{_dataPropertyPath}.${row}.{columnPropertyNames[column]}");
    }
    private object DrawValue(Rect rect, GUIContent label, object value, Type type)
    {
        if (type == typeof(int))
        {
            return SirenixEditorFields.IntField(rect, label, (int)value);
        }
        else if (type == typeof(string))
        {
            return SirenixEditorFields.TextField(rect, label, (string)value);
        }
        else if (type == typeof(float))
        {
            return SirenixEditorFields.FloatField(rect, label, (float)value);
        }
        else if (type == typeof(double))
        {
            return SirenixEditorFields.DoubleField(rect, label, (double)value);
        }
        else if (type.IsEnum)
        {
            return SirenixEditorFields.EnumDropdown(rect, label, (Enum)value);
        }

        else if (typeof(Object).IsAssignableFrom(type))
        {
            return SirenixEditorFields.UnityObjectField(rect, label, (Object)value, type, false);
        }
        return null;
    }

    private void SetColumnSize(GUITable table, int column, float min = 0, float width = 0)
    {
        for (int row = 0; row < table.RowCount; row++)
        {
            if (table[column, row] == null) continue;
            table[column, row].MinWidth = min;
            table[column, row].Width = width;
        }
    }

    public void DrawTable()
    {
        _table.DrawTable();
    }
}
