using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Book_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/ExcelData/Book.xlsx";
    private static readonly string[] sheetNames = { "Sheet1", };
    
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (!filePath.Equals(asset))
                continue;

            using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}

                foreach (string sheetName in sheetNames)
                {
                    var exportPath = "Assets/ExcelData/" + sheetName + ".asset";
                    
                    // check scriptable object
                    var data = (Sheet1)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Sheet1));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Sheet1>();
                        AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
                        data.hideFlags = HideFlags.NotEditable;
                    }
                    data.param.Clear();

					// check sheet
                    var sheet = book.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Debug.LogError("[QuestData] sheet not found:" + sheetName);
                        continue;
                    }

                	// add infomation
                    for (int i=1; i<= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        ICell cell = null;
                        
                        var p = new Sheet1.Param();
			
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.hint1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.hint2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.hint3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.hint4 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.hint5 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.answer1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.answer2 = (cell == null ? "" : cell.StringCellValue);

                        data.param.Add(p);
                    }
                    
                    // save scriptable object
                    ScriptableObject obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
                    EditorUtility.SetDirty(obj);
                }
            }

        }
    }
}
