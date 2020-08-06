using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Assets.Scripts.Utils;
using System;
using SQLite4Unity3d;
using System.IO;
using System.Text;

public class DataService
{
    private SQLiteConnection _connection;

    public DataService(string DatabaseName)
    {

        #region DataServiceInit


#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
            var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
            // then save to Application.persistentDataPath
            File.Copy(loadDb, filepath);
#elif UNITY_WP8
            var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
            // then save to Application.persistentDataPath
            File.Copy(loadDb, filepath);
#elif UNITY_WINRT
		    var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		    // then save to Application.persistentDataPath
		    File.Copy(loadDb, filepath);
#else
	        var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	        // then save to Application.persistentDataPath
	        File.Copy(loadDb, filepath);
#endif
            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif

        #endregion

        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

    }

    public void RecreateUserTable()
    {
        _connection.DropTable<User>();
        _connection.CreateTable<User>();

        Debug.Log("Created USER TABLE");
    }

    public void RecreateCategoryTable()
    {
        _connection.DropTable<Category>();
        _connection.CreateTable<Category>();

        Debug.Log("Created CATEGORY TABLE");
    }

    public void RecreatePremiseTable()
    {
        _connection.DropTable<Premise>();
        _connection.CreateTable<Premise>();

        Debug.Log("Created PREMISE TABLE");
    }

    public void RecreateJokeTable()
    {
        _connection.DropTable<Joke>();
        _connection.CreateTable<Joke>();

        Debug.Log("Created JOKE TABLE");
    }

    public void ResetTables()
    {
        Debug.Log("RESET: ");

        RecreateUserTable();
        RecreateCategoryTable();
        RecreatePremiseTable();
        RecreateJokeTable();
    }

    public void WriteCategoriesData(List<Category> categories, bool update = false)
    {
        if (update)
            _connection.UpdateAll(categories);
        else
            _connection.InsertAll(categories);
    }

    public List<Category> GetAllCategories()
    {
        return _connection.Table<Category>().ToList();
    }

    public Category GetCategory(int categoryId)
    {
        return _connection.Table<Category>().Where(c => c.Id == categoryId).FirstOrDefault();
    }

    public Premise GetPremise(int? PremiseId = null)
    {
        var premise = _connection.Table<Premise>().Where(p => p.Id == PremiseId).FirstOrDefault();
        premise.Text = DataUtils.DecodeTextFromBytes(premise.EncodedText);
        return premise;
    }

    private int[] _premisesId;

    public List<Joke> GetJokeHistory()
    {
        var jokes = _connection.Table<Joke>().OrderBy(j => j.PremiseId).ToList();
        Premise premise = null;
        foreach (var joke in jokes)
        {
            if (premise == null || premise.Id != joke.PremiseId)
            {
                premise = _connection.Table<Premise>().FirstOrDefault(p => p.Id == joke.PremiseId);
            }
            joke.Premise = premise;
        }

        return jokes;
    }

    public Premise GetRandomPremise()
    {
        if (_premisesId == null)
        {
            _premisesId = _connection.Table<Premise>().ToList().Select(p => p.Id).ToArray();
        }

        Premise premise = null;
        var randomIndex = UnityEngine.Random.Range(0, _premisesId.Length);
        var randomId = _premisesId[randomIndex];
        premise = GetPremise(randomId);

        return premise;
    }

    private int GetPremiseCount()
    {
        return _connection.ExecuteScalar<int>("SELECT COUNT(Premise.Id) FROM Premise");
    }

    public void SavePremise(Premise premise)
    {
        premise.EncodedText = DataUtils.EncodeTextInBytes(premise.Text);
        if (premise.Id > 0)
        {
            _connection.Update(premise);
        }
        else
        {
            _connection.Insert(premise);
        }
    }

    public void SaveJoke(Joke joke)
    {
        joke.EncodedText = DataUtils.EncodeTextInBytes(joke.Text);
        if (joke.Id > 0)
        {
            _connection.Update(joke);
        }
        else
        {
            _connection.Insert(joke);
        }
    }

    internal void WriteDefaultData()
    {

        Debug.Log("No Default data to write.");
    }

    public void CreateUser(User user)
    {
        _connection.Insert(user);
    }

    public void UpdateUser(User user)
    {
        int rowsAffected = _connection.Update(user);
        Debug.Log("(UPDATE User) rowsAffected : " + rowsAffected);
    }

    public User GetDeviceUser()
    {
        return _connection.Table<User>().Where(x => x.Id == 1).FirstOrDefault();
    }

    public User GetLastUser()
    {
        return _connection.Table<User>().Last();
    }

    /*
    * User - END
    * * --------------------------------------------------------------------------------------------------------------------------------------
    */

    /*
     * Map
     * * --------------------------------------------------------------------------------------------------------------------------------------
     */

    // X : 0 - 11
    // Y : 0 - 8
    //public int CreateMap(Map map)
    //{
    //    _connection.Insert(map);
    //    return map.Id;
    //}

    //public int UpdateMap(Map map)
    //{
    //    _connection.Update(map);
    //    return map.Id;
    //}

    //public Map GetMap(int mapId)
    //{
    //    return _connection.Table<Map>().Where(x => x.Id == mapId).FirstOrDefault();
    //}

    //public int GetNextMapId(int number)
    //{
    //    return _connection.Table<Map>().Where(x => x.Number == number).FirstOrDefault().Id;
    //}

    //public void CreateTiles(List<MapTile> mapTiles)
    //{
    //    _connection.InsertAll(mapTiles);
    //}

    //public IEnumerable<Map> GetMaps()
    //{
    //    return _connection.Table<Map>();
    //}

    //public IEnumerable<MapTile> GetTiles(int mapId)
    //{
    //    return _connection.Table<MapTile>().Where(x => x.MapId == mapId);
    //}

    //public void DeleteMapTiles(int mapId)
    //{
    //    var sql = string.Format("delete from MapTile where MapId = {0}", mapId);
    //    _connection.ExecuteSql(sql);
    //}

    /*
     * Map - END
     * * --------------------------------------------------------------------------------------------------------------------------------------
     */
}
