using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BookData
{
    public List<WordData> words;
    public List<string> memos;
    
    public BookData()
    {
        words = new List<WordData>();
        memos = new List<string>();
    }

    public void AddBookData(WordData word, string memo)
    {
        words.Add(word);
        memos.Add(memo);
    }
}

public class PuzzleData
{
    public int maxCount;
    public int[] Answer;
    public PuzzleData()
    {
        Answer = new int[10];
    }
}

public class SaveAndLoad : MonoBehaviour
{
    public static BookData LoadBookData()
    {
        BookData bookData = new BookData();

        string path = SystemPath.GetPath("BookData.json");
        if (!File.Exists(path)){ return null; }
        
        string jsonLoad = File.ReadAllText(path);
        bookData = JsonUtility.FromJson<BookData>(jsonLoad);
        return bookData;

    }
    public static void SaveBookData(List<WordBookData> books)
    {
        BookData bookData = new BookData();
        foreach(var book in books)
        {
            bookData.AddBookData(book.WordData, book.memo);
        }
        string jsonSave = JsonUtility.ToJson(bookData, true);
        Debug.Log(jsonSave);
        File.WriteAllText(SystemPath.GetPath("BookData.json"), jsonSave);
    }
    
    public static PuzzleData LoadPuzzleData(string fileName)
    {
        PuzzleData puzzleData = new PuzzleData();
        string path = SystemPath.GetPath(fileName);
        if (!File.Exists(path)) { return null; }

        string jsonLoad = File.ReadAllText(path);
        puzzleData = JsonUtility.FromJson<PuzzleData>(jsonLoad);
        return puzzleData;
    }
}
