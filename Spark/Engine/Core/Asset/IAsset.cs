using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Engine.Core.Asset;
public interface IAsset<T>
{
    /// <summary>
    /// Loads an asset from the specified file.
    /// </summary>
    /// <param name="file">The path to the file to load.</param>
    /// <returns>A task that represents the asynchronous load operation. The task result contains the loaded asset.</returns>
    Task<T> LoadAsync(string? file = null);

    /// <summary>
    /// Saves the asset to its current location.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveAsync(string? file = null);

    /// <summary>
    /// Unloads the asset, freeing up any resources it is using.
    /// </summary>
    void Unload();

    /// <summary>
    /// Gets the file path of the asset.
    /// </summary>
    string FilePath { get; }

    /// <summary>
    /// Gets the name of the asset.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Marks for the asset to be saved on program exit.
    /// </summary>
    bool Save { get; set; }
}
