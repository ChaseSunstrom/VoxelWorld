using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spark.Engine.Core.Asset;
public class AssetManager
{
    private readonly Dictionary<string, IAsset<object>> _assets = new();

    public async Task<T> LoadAssetAsync<T>(string assetName, Func<T> assetFactory, string? filePath = null) where T : IAsset<T>
    {
        if (_assets.ContainsKey(assetName))
        {
            return (T)_assets[assetName];
        }

        T asset = assetFactory();
        await asset.LoadAsync(filePath);
        _assets[assetName] = (IAsset<object>)asset;
        return asset;
    }

    public async Task SaveAssetAsync<T>(string assetName, string? filePath = null) where T : IAsset<T>
    {
        if (_assets.ContainsKey(assetName))
        {
            await ((T)_assets[assetName]).SaveAsync(filePath);
        }
        else
        {
            throw new InvalidOperationException("Asset not found.");
        }
    }

    public void UnloadAsset(string assetName)
    {
        if (_assets.ContainsKey(assetName))
        {
            _assets[assetName].Unload();
            _assets.Remove(assetName);
        }
        else
        {
            throw new InvalidOperationException("Asset not found.");
        }
    }

    public void UnloadAllAssets()
    {
        foreach (var asset in _assets.Values)
        {
            asset.Unload();
        }
        _assets.Clear();
    }

    public T GetAsset<T>(string assetName) where T : IAsset<T>
    {
        if (_assets.ContainsKey(assetName))
        {
            return (T)_assets[assetName];
        }
        else
        {
            throw new InvalidOperationException("Asset not found.");
        }
    }

    public void AddAsset<T>(T asset) where T : IAsset<T>
    {
        if (_assets.ContainsKey(asset.Name))
        {
            throw new InvalidOperationException("Asset with the same name already exists.");
        }

        _assets[asset.Name] = (IAsset<object>)asset;
    }

    public async Task SaveAllAssetsAsync()
    {
        foreach (var asset in _assets.Values)
        {
            if (asset.Save)
            {
                await asset.SaveAsync();
            }
        }
    }
}
